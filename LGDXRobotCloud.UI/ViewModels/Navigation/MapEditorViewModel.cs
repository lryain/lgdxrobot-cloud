using LGDXRobotCloud.UI.Client.Models;
using LGDXRobotCloud.UI.ViewModels.Shared;

namespace LGDXRobotCloud.UI.ViewModels.Navigation;

public record WaypointTrafficDisplay
{
  public Guid WaypointFromId { get; set; }
  public Guid WaypointToId { get; set; }
  public bool IsBothWaysTraffic { get; set; }
}

public class MapEditorViewModel : FormViewModelBase
{
  public List<WaypointDetailsViewModel> Waypoints { get; set; } = [];

  public List<WaypointTrafficViewModel> WaypointTraffics { get; set; } = [];

  public List<WaypointTrafficDisplay> WaypointTrafficsDisplay { get; set; } = [];
}

public static class MapEditorViewModelExtensions
{
  public static void FromDto(this MapEditorViewModel mapEditorViewModel, MapEditorDto mapEditorDto)
  {
    foreach (var waypoint in mapEditorDto.Waypoints!)
    {
      WaypointDetailsViewModel w = new()
      {
        MapEditorObjectId = Guid.NewGuid()
      };
      w.FromDto(waypoint);
      mapEditorViewModel.Waypoints.Add(w);
    }

    // Key: MapEditorObjectId, Value: Id
    Dictionary<int, Guid> waypointObjectId = [];
    foreach (var waypoint in mapEditorViewModel.Waypoints)
    {
      waypointObjectId.Add((int)waypoint.Id!, (Guid)waypoint.MapEditorObjectId!);
    }

    foreach (var traffic in mapEditorDto.WaypointTraffics!)
    {
      WaypointTrafficViewModel t = new();
      t.FromDto(traffic);
      mapEditorViewModel.WaypointTraffics.Add(t);
    }

    foreach (var traffic in mapEditorViewModel.WaypointTraffics)
    {
      if (traffic.WaypointFromId != null)
      {
        traffic.AlternativeWaypointFromId = waypointObjectId[(int)traffic.WaypointFromId!];
      }
      if (traffic.WaypointToId != null)
      {
        traffic.AlternativeWaypointToId = waypointObjectId[(int)traffic.WaypointToId!];
      }
    }
    
    // Key: (WaypointFromId, WaypointToId), Value: IsBothWaysTraffic
    Dictionary<(Guid, Guid), bool> waypointTrafficsDisplayTemp = [];
    foreach (var traffic in mapEditorDto.WaypointTraffics!)
    {
      // Display
      // If other way around is exists, then it is both ways traffic
      if (waypointTrafficsDisplayTemp.ContainsKey((waypointObjectId[(int)traffic.WaypointToId!], waypointObjectId[(int)traffic.WaypointFromId!])))
      {
        waypointTrafficsDisplayTemp[(waypointObjectId[(int)traffic.WaypointToId!], waypointObjectId[(int)traffic.WaypointFromId!])] = true;
      }
      else
      {
        waypointTrafficsDisplayTemp.Add((waypointObjectId[(int)traffic.WaypointFromId!], waypointObjectId[(int)traffic.WaypointToId!]), false);
      }
    }
    // waypointTrafficsDisplayTemp to waypointTrafficsDisplay
    mapEditorViewModel.WaypointTrafficsDisplay = waypointTrafficsDisplayTemp.Select(x => new WaypointTrafficDisplay
    {
      WaypointFromId = x.Key.Item1,
      WaypointToId = x.Key.Item2,
      IsBothWaysTraffic = x.Value
    }).ToList();
  }

  public static MapEditorUpdateDto ToUpdateDto(this MapEditorViewModel mapEditorViewModel)
  {
    return new MapEditorUpdateDto
    {
      Waypoints = mapEditorViewModel.Waypoints.Select(x => new WaypointUpsertDto
      {
        Id = x.Id,
        Name = x.Name,
        FeatureId = x.FeatureId,
        ClassName = x.ClassName,
        X = x.X,
        Y = x.Y,
        Rotation = x.Rotation,
        IsDocking = x.IsDocking,
      }).ToList(),
      WaypointTraffics = mapEditorViewModel.WaypointTraffics.Select(x => new WaypointTrafficUpdateDto
      {
        Id = x.Id,
        FeatureId = x.FeatureId,
        WaypointFromId = x.WaypointFromId,
        WaypointToId = x.WaypointToId,
        Overridable = x.Overridable,
        Cost = x.Cost,
        SpeedLimit = x.SpeedLimit,
        AbsoluteSpeedLimit = x.AbsoluteSpeedLimit,
      }).ToList()
    };
  }
}

internal class HashMap<T1, T2>
{
}