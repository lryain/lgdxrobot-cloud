using LGDXRobotCloud.UI.Client.Models;
using LGDXRobotCloud.UI.ViewModels.Shared;

namespace LGDXRobotCloud.UI.ViewModels.Navigation;

public record WaypointTrafficDisplay
{
  public int WaypointFromId { get; set; }
  public int WaypointToId { get; set; }
  public bool IsBothWaysTraffic { get; set; }
}

public class MapEditorViewModel : FormViewModelBase
{
  public List<WaypointDetailsViewModel> Waypoints { get; set; } = [];

  public List<WaypointTrafficDto> WaypointTraffics { get; set; } = [];

  public List<WaypointTrafficDisplay> WaypointTrafficsDisplay { get; set; } = [];
}

public static class MapEditorViewModelExtensions
{
  public static void FromDto(this MapEditorViewModel mapEditorViewModel, MapEditorDto mapEditorDto)
  {
    // Waypoints
    mapEditorViewModel.Waypoints = mapEditorDto.Waypoints?.Select(w => new WaypointDetailsViewModel
    {
      Id = w.Id,
      Name = w.Name!,
      RealmId = w.Realm!.Id,
      RealmName = w.Realm.Name,
      FeatureId = w.FeatureId,
      ClassName = w.ClassName,
      X = w.X,
      Y = w.Y,
      Rotation = w.Rotation,
      IsDocking = (bool)w.IsDocking!,
      MapEditorObjectId = Guid.NewGuid(),
    }).ToList() ?? [];

    // Waypoint Traffics
    mapEditorViewModel.WaypointTraffics = mapEditorDto.WaypointTraffics ?? [];

    // Key: (WaypointFromId, WaypointToId), Value: IsBothWaysTraffic
    Dictionary<(int, int), bool> waypointTrafficsDisplayTemp = [];
    foreach (var Traffic in mapEditorDto.WaypointTraffics!)
    {
      // Display
      // If other way around is exists, then it is both ways traffic
      if (waypointTrafficsDisplayTemp.ContainsKey(((int)Traffic.WaypointToId!, (int)Traffic.WaypointFromId!)))
      {
        waypointTrafficsDisplayTemp[((int)Traffic.WaypointToId!, (int)Traffic.WaypointFromId!)] = true;
      }
      else
      {
        waypointTrafficsDisplayTemp.Add(((int)Traffic.WaypointFromId!, (int)Traffic.WaypointToId!), false);
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