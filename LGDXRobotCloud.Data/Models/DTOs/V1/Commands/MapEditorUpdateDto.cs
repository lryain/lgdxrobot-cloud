using LGDXRobotCloud.Data.Models.Business.Navigation;

namespace LGDXRobotCloud.Data.Models.DTOs.V1.Commands;

public record MapEditorUpdateDto
{
  public IEnumerable<WaypointUpsertDto> Waypoints { get; set; } = [];

  public IEnumerable<WaypointTrafficUpdateDto> WaypointTraffics { get; set; } = [];
}

public static class MapEditUpdateDtoExtensions
{
  public static MapEditorUpdateBusinessModel ToBusinessModel(this MapEditorUpdateDto model)
  {
    return new MapEditorUpdateBusinessModel
    {
      Waypoints = model.Waypoints.Select(x => x.ToBusinessModel()),
      
      WaypointTraffics = model.WaypointTraffics.Select(x => x.ToBusinessModel())
    };
  }
}