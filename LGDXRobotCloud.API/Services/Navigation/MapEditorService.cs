using ImTools;
using LGDXRobotCloud.API.Exceptions;
using LGDXRobotCloud.API.Services.Administration;
using LGDXRobotCloud.Data.DbContexts;
using LGDXRobotCloud.Data.Entities;
using LGDXRobotCloud.Data.Models.Business.Administration;
using LGDXRobotCloud.Data.Models.Business.Navigation;
using LGDXRobotCloud.Utilities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LGDXRobotCloud.API.Services.Navigation;

public record WaypointsTraffic
{
  public Dictionary<int, Waypoint> Waypoints { get; set; } = null!;
  public Dictionary<int, HashSet<int>> WaypointTraffics { get; set; } = null!;
}

public interface IMapEditorService
{
  Task<MapEditorBusinessModel> GetMapAsync(int realmId);
  Task<bool> UpdateMapAsync(int realmId, MapEditorUpdateBusinessModel MapEditUpdateBusinessModel);

  Task<WaypointsTraffic> GetWaypointTrafficAsync(int realmId);
}

public class MapEditorService(
    IActivityLogService activityLogService,
    IMemoryCache memoryCache,
    LgdxContext context
  ) : IMapEditorService
{
  private readonly IActivityLogService _activityLogService = activityLogService ?? throw new ArgumentNullException(nameof(activityLogService));
  private readonly IMemoryCache _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
  private readonly LgdxContext _context = context ?? throw new ArgumentNullException(nameof(context));


  public async Task<MapEditorBusinessModel> GetMapAsync(int realmId)
  {
    // Check if realm exists
    var realm = await _context.Realms.AsNoTracking()
      .Where(r => r.Id == realmId)
      .FirstOrDefaultAsync()
        ?? throw new LgdxNotFound404Exception();

    var waypoints = await _context.Waypoints.AsNoTracking()
      .Where(w => w.RealmId == realmId)
      .Select(w => new WaypointBusinessModel
      {
        Id = w.Id,
        Name = w.Name,
        RealmId = w.RealmId,
        RealmName = w.Realm.Name,
        FeatureId = w.FeatureId,
        ClassName = w.ClassName,
        X = w.X,
        Y = w.Y,
        Rotation = w.Rotation,
        IsDocking = w.IsDocking,
      })
      .ToListAsync();
    var waypointTraffics = await _context.WaypointTraffics.AsNoTracking()
      .Where(w => w.RealmId == realmId)
      .Select(w => new WaypointTrafficBusinessModel
      {
        Id = w.Id,
        FeatureId = w.FeatureId,
        WaypointFromId = w.WaypointFromId,
        WaypointToId = w.WaypointToId,
        Overridable = w.Overridable,
        Cost = w.Cost,
        SpeedLimit = w.SpeedLimit,
        AbsoluteSpeedLimit = w.AbsoluteSpeedLimit,
      })
      .ToListAsync();
    return new MapEditorBusinessModel
    {
      Waypoints = waypoints,
      WaypointTraffics = waypointTraffics,
    };
  }

  public async Task<bool> UpdateMapAsync(int realmId, MapEditorUpdateBusinessModel mapEditorUpdateBusinessModel)
  {
    _memoryCache.Remove($"MapEditorService_InternalWaypointsTraffic_{realmId}");

    // Check Waypoints
    var inputWaypoints = mapEditorUpdateBusinessModel.Waypoints
      .ToList();

    var inputWaypointTraffics = mapEditorUpdateBusinessModel.WaypointTraffics
      .OrderBy(w => w.WaypointFromId)
      .ThenBy(w => w.WaypointToId)
      .ToList();
    
    var realm = await _context.Realms.AsNoTracking()
      .Where(r => r.Id == realmId)
      .FirstOrDefaultAsync() 
        ?? throw new LgdxValidation400Expection(nameof(realmId), "Realm does not exist.");

    // Feature ID is required when the realm has route control
    if (realm.HasRouteControl && inputWaypoints.Any(w => w.FeatureId == null))
    {
      throw new LgdxValidation400Expection(nameof(inputWaypoints), "Feature ID is required when the realm has route control.");
    }

    // Feature ID cannot duplicate
    HashSet<int> featureIds = [];
    foreach (var waypoint in inputWaypoints)
    {
      if (waypoint.FeatureId != null)
      {
        if (featureIds.Contains((int)waypoint.FeatureId))
        {
          throw new LgdxValidation400Expection(nameof(waypoint.FeatureId), "Feature ID cannot duplicate.");
        }
        featureIds.Add((int)waypoint.FeatureId);
      }
    }
    foreach (var waypointTraffic in inputWaypointTraffics)
    {
      if (featureIds.Contains(waypointTraffic.FeatureId))
      {
        throw new LgdxValidation400Expection(nameof(waypointTraffic.FeatureId), "Feature ID cannot duplicate.");
      }
      featureIds.Add(waypointTraffic.FeatureId);
    }

    /*
     * Waypoints
     */
    var newWaypoints = mapEditorUpdateBusinessModel.Waypoints
      .Where(w => w.Id == null)
      .ToList();
    var existingWaypoints = mapEditorUpdateBusinessModel.Waypoints
      .Where(w => w.Id != null)
      .ToList();

    var databaseWaypoints = await _context.Waypoints
      .Where(w => w.RealmId == realmId)
      .OrderBy(w => w.Id)
      .ToListAsync();
    var updateWaypoints = databaseWaypoints
      .Where(w => existingWaypoints.Any(ew => ew.Id == w.Id))
      .ToList();
    var deleteWaypoints = databaseWaypoints
      .Where(w => !existingWaypoints.Any(ew => ew.Id == w.Id))
      .ToList();

    var addingWaypoints = newWaypoints.Select(w => new Waypoint
    {
      Name = w.Name,
      RealmId = realmId,
      FeatureId = w.FeatureId,
      ClassName = w.ClassName,
      X = w.X,
      Y = w.Y,
      Rotation = w.Rotation,
      IsDocking = w.IsDocking,
    });
    await _context.Waypoints.AddRangeAsync(addingWaypoints);
    foreach (var waypoint in updateWaypoints)
    {
      var w = updateWaypoints.First(w => w.Id == waypoint.Id);
      waypoint.Name = w.Name;
      waypoint.FeatureId = w.FeatureId;
      waypoint.ClassName = w.ClassName;
      waypoint.X = w.X;
      waypoint.Y = w.Y;
      waypoint.Rotation = w.Rotation; 
      waypoint.IsDocking = w.IsDocking;
    }
    _context.Waypoints.RemoveRange(deleteWaypoints);

    /*
     * Traffic
     */
    var newWaypointTraffics = mapEditorUpdateBusinessModel.WaypointTraffics
      .Where(w => w.Id == null)
      .ToList();
    var existingWaypointTraffics = mapEditorUpdateBusinessModel.WaypointTraffics
      .Where(w => w.Id != null)
      .ToList();

    var databaseWaypointTraffics = await _context.WaypointTraffics
      .Where(w => w.RealmId == realmId)
      .ToListAsync();
    var updateWaypointTraffics = databaseWaypointTraffics
      .Where(w => existingWaypointTraffics.Any(ew => ew.Id == w.Id))
      .ToList();
    var deleteWaypointTraffics = databaseWaypointTraffics
      .Where(w => !existingWaypointTraffics.Any(ew => ew.Id == w.Id))
      .ToList();

    await _context.WaypointTraffics.AddRangeAsync(newWaypointTraffics.Select(w => new WaypointTraffic
    {
      FeatureId = w.FeatureId,
      RealmId = realmId,
      WaypointFromId = w.WaypointFromId,
      WaypointToId = w.WaypointToId,
      Overridable = w.Overridable,
      Cost = w.Cost,
      SpeedLimit = w.SpeedLimit,
      AbsoluteSpeedLimit = w.AbsoluteSpeedLimit,
    }));
    foreach (var waypointTraffic in updateWaypointTraffics)
    {
      var w = updateWaypointTraffics.First(w => w.Id == waypointTraffic.Id);
      waypointTraffic.FeatureId = w.FeatureId;
      waypointTraffic.WaypointFromId = w.WaypointFromId;
      waypointTraffic.WaypointToId = w.WaypointToId;
      waypointTraffic.Overridable = w.Overridable;
      waypointTraffic.Cost = w.Cost;
      waypointTraffic.SpeedLimit = w.SpeedLimit;
      waypointTraffic.AbsoluteSpeedLimit = w.AbsoluteSpeedLimit;
    }
    _context.WaypointTraffics.RemoveRange(deleteWaypointTraffics);

    // Save changes
    await _context.SaveChangesAsync();

    // Update Activity Log
    foreach (var w in addingWaypoints)
    {
      await _activityLogService.CreateActivityLogAsync(new ActivityLogCreateBusinessModel
      {
        EntityName = nameof(Waypoint),
        EntityId = w.Id.ToString(),
        Action = ActivityAction.Create,
      });
    }
    foreach (var w in updateWaypoints)
    {
      await _activityLogService.CreateActivityLogAsync(new ActivityLogCreateBusinessModel
      {
        EntityName = nameof(Waypoint),
        EntityId = w.Id.ToString(),
        Action = ActivityAction.Update,
      });
    }
    foreach (var w in deleteWaypoints)
    {
      await _activityLogService.CreateActivityLogAsync(new ActivityLogCreateBusinessModel
      {
        EntityName = nameof(Waypoint),
        EntityId = w.Id.ToString(),
        Action = ActivityAction.Delete,
      });
    }
    await _activityLogService.CreateActivityLogAsync(new ActivityLogCreateBusinessModel
    {
      EntityName = nameof(Realm),
      EntityId = realmId.ToString(),
      Action = ActivityAction.RealmTrafficUpdated,
    });

    return true;
  }

  public async Task<WaypointsTraffic> GetWaypointTrafficAsync(int realmId)
  {
    if (_memoryCache.TryGetValue($"MapEditorService_InternalWaypointsTraffic_{realmId}", out WaypointsTraffic? t))
    {
      return t ?? new();
    }
    
    var waypoints = await _context.Waypoints.AsNoTracking()
      .Where(w => w.RealmId == realmId)
      .ToListAsync();
    var waypointTraffics = await _context.WaypointTraffics.AsNoTracking()
      .Where(w => w.RealmId == realmId)
      .ToListAsync();

    var waypointsDict = waypoints.ToDictionary(w => w.Id);
    var waypointTrafficsDict = new Dictionary<int, HashSet<int>>();
    foreach (var traffic in waypointTraffics)
    {
      if (waypointTrafficsDict.TryGetValue(traffic.WaypointFromId, out HashSet<int>? neighbors))
      {
        neighbors.Add(traffic.WaypointToId);
        waypointTrafficsDict[traffic.WaypointFromId] = neighbors;
      }
      else
      {
        waypointTrafficsDict[traffic.WaypointFromId] = [traffic.WaypointToId];
      }
    }

    var internalWaypointsTraffic = new WaypointsTraffic
    {
      Waypoints = waypointsDict,
      WaypointTraffics = waypointTrafficsDict,
    };
    _memoryCache.Set($"MapEditorService_InternalWaypointsTraffic_{realmId}", internalWaypointsTraffic);
    return internalWaypointsTraffic;
  }
}