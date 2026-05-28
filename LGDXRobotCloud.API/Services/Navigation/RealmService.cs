using LGDXRobotCloud.API.Exceptions;
using LGDXRobotCloud.API.Services.Administration;
using LGDXRobotCloud.Data.DbContexts;
using LGDXRobotCloud.Data.Entities;
using LGDXRobotCloud.Data.Models.Business.Administration;
using LGDXRobotCloud.Data.Models.Business.Navigation;
using LGDXRobotCloud.Utilities.Enums;
using LGDXRobotCloud.Utilities.Helpers;
using Microsoft.EntityFrameworkCore;

namespace LGDXRobotCloud.API.Services.Navigation;

public interface IRealmService
{
  Task<(IEnumerable<RealmListBusinessModel>, PaginationHelper)> GetRealmsAsync(string? name, int pageNumber, int pageSize);
  Task<RealmBusinessModel> GetRealmAsync(int id);
  Task<RealmBusinessModel> GetDefaultRealmAsync();
  Task<RealmBusinessModel> CreateRealmAsync(RealmCreateBusinessModel createModel);
  Task<bool> UpdateRealmAsync(int id, RealmUpdateBusinessModel updateModel);
  Task<bool> UpdateRealmMapAsync(int id, RealmMapUpdateBusinessModel updateModel);
  Task<bool> TestDeleteRealmAsync(int id);
  Task<bool> DeleteRealmAsync(int id);

  Task<IEnumerable<RealmSearchBusinessModel>> SearchRealmsAsync(string? name);
}

public class RealmService(
    IActivityLogService activityLogService,
    LgdxContext context
  ) : IRealmService
{
  private readonly IActivityLogService _activityLogService = activityLogService ?? throw new ArgumentNullException(nameof(activityLogService));
  private readonly LgdxContext _context = context ?? throw new ArgumentNullException(nameof(context));

  public async Task<(IEnumerable<RealmListBusinessModel>, PaginationHelper)> GetRealmsAsync(string? name, int pageNumber, int pageSize)
  {
    var query = _context.Realms as IQueryable<Realm>;
    if (!string.IsNullOrWhiteSpace(name))
    {
      name = name.Trim();
      query = query.Where(m => m.Name.ToLower().Contains(name.ToLower()));
    }
    var itemCount = await query.CountAsync();
    var PaginationHelper = new PaginationHelper(itemCount, pageNumber, pageSize);
    var realms = await query.AsNoTracking()
      .OrderBy(m => m.Id)
      .Skip(pageSize * (pageNumber - 1))
      .Take(pageSize)
      .Select(m => new RealmListBusinessModel {
        Id = m.Id,
        Name = m.Name,
        Description = m.Description,
        Resolution = m.Resolution,
      })
      .ToListAsync();
    return (realms, PaginationHelper);
  }

  public async Task<RealmBusinessModel> GetRealmAsync(int id)
  {
    return await _context.Realms.AsNoTracking()
      .Where(m => m.Id == id)
      .Select(m => new RealmBusinessModel {
        Id = m.Id,
        Name = m.Name,
        Description = m.Description,
        HasRouteControl = m.HasRouteControl,
        Map = Convert.ToBase64String(m.Map),
        MapWidth = m.MapWidth,
        MapHeight = m.MapHeight,
        KeepoutMask = Convert.ToBase64String(m.KeepoutMask),
        SpeedMask = Convert.ToBase64String(m.SpeedMask),
        Resolution = m.Resolution,
        OriginX = m.OriginX,
        OriginY = m.OriginY,
        OriginRotation = m.OriginRotation,
      })
      .FirstOrDefaultAsync()
        ?? throw new LgdxNotFound404Exception();
  }

  public async Task<RealmBusinessModel> GetDefaultRealmAsync()
  {
    return await _context.Realms.AsNoTracking()
      .OrderBy(m => m.Id)
      .Select(m => new RealmBusinessModel {
        Id = m.Id,
        Name = m.Name,
        Description = m.Description,
        HasRouteControl = m.HasRouteControl,
        Map = Convert.ToBase64String(m.Map),
        MapWidth = m.MapWidth,
        MapHeight = m.MapHeight,
        KeepoutMask = Convert.ToBase64String(m.KeepoutMask),
        SpeedMask = Convert.ToBase64String(m.SpeedMask),
        Resolution = m.Resolution,
        OriginX = m.OriginX,
        OriginY = m.OriginY,
        OriginRotation = m.OriginRotation,
      })
      .FirstOrDefaultAsync()
        ?? throw new LgdxNotFound404Exception();
  }

  public async Task<RealmBusinessModel> CreateRealmAsync(RealmCreateBusinessModel createModel)
  {
    var realm = new Realm {
      Name = createModel.Name,
      Description = createModel.Description,
      HasRouteControl = createModel.HasRouteControl,
      Map = Convert.FromBase64String(createModel.Map ?? string.Empty),
      MapWidth = createModel.MapWidth ?? 0,
      MapHeight = createModel.MapHeight ?? 0,
      KeepoutMask = Convert.FromBase64String(createModel.KeepoutMask ?? string.Empty),
      SpeedMask = Convert.FromBase64String(createModel.SpeedMask ?? string.Empty),
      Resolution = createModel.Resolution,
      OriginX = createModel.OriginX,
      OriginY = createModel.OriginY,
      OriginRotation = createModel.OriginRotation,
    };

    await _context.Realms.AddAsync(realm);
    await _context.SaveChangesAsync();
    
    await _activityLogService.CreateActivityLogAsync(new ActivityLogCreateBusinessModel
    {
      EntityName = nameof(Realm),
      EntityId = realm.Id.ToString(),
      Action = ActivityAction.Create,
    });

    return new RealmBusinessModel
    {
      Id = realm.Id,
      Name = realm.Name,
      Description = realm.Description,
      HasRouteControl = realm.HasRouteControl,
      Map = createModel.Map ?? string.Empty,
      MapWidth = realm.MapWidth,
      MapHeight = realm.MapHeight,
      KeepoutMask = createModel.KeepoutMask ?? string.Empty,
      SpeedMask = createModel.SpeedMask ?? string.Empty,
      Resolution = realm.Resolution,
      OriginX = realm.OriginX,
      OriginY = realm.OriginY,
      OriginRotation = realm.OriginRotation,
    };
  }

  public async Task<bool> UpdateRealmAsync(int id, RealmUpdateBusinessModel updateModel)
  {
    bool result = await _context.Realms
      .Where(m => m.Id == id)
      .ExecuteUpdateAsync(setters => setters
        .SetProperty(m => m.Name, updateModel.Name)
        .SetProperty(m => m.Description, updateModel.Description)
        .SetProperty(m => m.HasRouteControl, updateModel.HasRouteControl)
        .SetProperty(m => m.Map, Convert.FromBase64String(updateModel.Map ?? string.Empty))
        .SetProperty(m => m.MapWidth, updateModel.MapWidth ?? 0)
        .SetProperty(m => m.MapHeight, updateModel.MapHeight ?? 0)
        .SetProperty(m => m.KeepoutMask, Convert.FromBase64String(updateModel.KeepoutMask ?? string.Empty))
        .SetProperty(m => m.SpeedMask, Convert.FromBase64String(updateModel.SpeedMask ?? string.Empty))
        .SetProperty(m => m.Resolution, updateModel.Resolution)
        .SetProperty(m => m.OriginX, updateModel.OriginX)
        .SetProperty(m => m.OriginY, updateModel.OriginY)
        .SetProperty(m => m.OriginRotation, updateModel.OriginRotation)
      ) == 1;

    if (result)
    {
      await _activityLogService.CreateActivityLogAsync(new ActivityLogCreateBusinessModel
      {
        EntityName = nameof(Realm),
        EntityId = id.ToString(),
        Action = ActivityAction.Update,
      });
    }
    return result;
  }

  // For SLAM
  public async Task<bool> UpdateRealmMapAsync(int id, RealmMapUpdateBusinessModel updateModel)
  {
    bool result = await _context.Realms
      .Where(m => m.Id == id)
      .ExecuteUpdateAsync(setters => setters
        .SetProperty(m => m.Map, Convert.FromBase64String(updateModel.Map))
        .SetProperty(m => m.MapWidth, updateModel.MapWidth)
        .SetProperty(m => m.MapHeight, updateModel.MapHeight)
        .SetProperty(m => m.KeepoutMask, [])
        .SetProperty(m => m.SpeedMask, [])
        .SetProperty(m => m.Resolution, updateModel.Resolution)
        .SetProperty(m => m.OriginX, updateModel.OriginX)
        .SetProperty(m => m.OriginY, updateModel.OriginY)
        .SetProperty(m => m.OriginRotation, updateModel.OriginRotation)
      ) == 1;

    if (result)
    {
      await _activityLogService.CreateActivityLogAsync(new ActivityLogCreateBusinessModel
      {
        EntityName = nameof(Realm),
        EntityId = id.ToString(),
        Action = ActivityAction.Update,
      });
    }
    return result;
  }

  public async Task<bool> TestDeleteRealmAsync(int id)
  {
    var dependencies = await _context.Robots.Where(m => m.RealmId == id).CountAsync();
    if (dependencies > 0)
    {
      throw new LgdxValidation400Expection(nameof(id), $"This realm has been used by {dependencies} robots.");
    }
    dependencies = await _context.Waypoints.Where(m => m.RealmId == id).CountAsync();
    if (dependencies > 0)
    {
      throw new LgdxValidation400Expection(nameof(id), $"This realm has been used by {dependencies} waypoints.");
    }
    dependencies = await _context.AutoTasks
      .Where(m => m.RealmId == id)
      .Where(m => m.CurrentProgressId != (int)ProgressState.Completed && m.CurrentProgressId != (int)ProgressState.Aborted)
      .CountAsync();
    if (dependencies > 0)
    {
      throw new LgdxValidation400Expection(nameof(id), $"This realm has been used by {dependencies} running/waiting/template tasks.");
    }
    return true;
  }

  public async Task<bool> DeleteRealmAsync(int id)
  {
    bool result = await _context.Realms.Where(m => m.Id == id)
      .ExecuteDeleteAsync() == 1;

    if (result)
    {
      await _activityLogService.CreateActivityLogAsync(new ActivityLogCreateBusinessModel
      {
        EntityName = nameof(Realm),
        EntityId = id.ToString(),
        Action = ActivityAction.Delete,
      });
    }
    return result;
  }

  public async Task<IEnumerable<RealmSearchBusinessModel>> SearchRealmsAsync(string? name)
  {
    var n = name ?? string.Empty;
    return await _context.Realms.AsNoTracking()
      .Where(w => w.Name.ToLower().Contains(n.ToLower()))
      .Take(10)
      .Select(m => new RealmSearchBusinessModel {
        Id = m.Id,
        Name = m.Name,
      })
      .ToListAsync();
  }
}