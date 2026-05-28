using System.ComponentModel.DataAnnotations;
using LGDXRobotCloud.Data.Models.Business.Navigation;

namespace LGDXRobotCloud.Data.Models.DTOs.V1.Commands;

public record WaypointTrafficUpdateDto
{
  public int? Id { get; set; }

  [Required (ErrorMessage = "A feature ID is required.")]
  public int FeatureId { get; set; }

  [Required (ErrorMessage = "A waypoint from ID is required.")]
  public required int WaypointFromId { get; set; }

  [Required (ErrorMessage = "A waypoint to ID is required.")]
  public required int WaypointToId { get; set; }

  [Required (ErrorMessage = "Overridable is required.")]
  public bool Overridable { get; set; }

  public double? Cost { get; set; }

  [Range(0.0, 100.0)]
  public double? SpeedLimit { get; set; }

  public double? AbsoluteSpeedLimit { get; set; }
}

public static class WaypointTrafficUpdateDtoExtensions
{
  public static WaypointTrafficUpdateBusinessModel ToBusinessModel(this WaypointTrafficUpdateDto model)
  {
    return new WaypointTrafficUpdateBusinessModel
    {
      Id = model.Id,
      FeatureId = model.FeatureId,
      WaypointFromId = model.WaypointFromId,
      WaypointToId = model.WaypointToId,
      Overridable = model.Overridable,
      Cost = model.Cost,
      SpeedLimit = model.SpeedLimit,
      AbsoluteSpeedLimit = model.AbsoluteSpeedLimit,
    };
  }
}