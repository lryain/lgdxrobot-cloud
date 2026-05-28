namespace LGDXRobotCloud.Data.Models.Business.Navigation;

public record MapEditorUpdateBusinessModel
{
  public IEnumerable<WaypointUpsertBusinessModel> Waypoints { get; set; } = [];

  public IEnumerable<WaypointTrafficUpdateBusinessModel> WaypointTraffics { get; set; } = [];
}