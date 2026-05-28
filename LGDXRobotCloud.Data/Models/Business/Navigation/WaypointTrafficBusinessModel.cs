using LGDXRobotCloud.Data.Models.DTOs.V1.Responses;

namespace LGDXRobotCloud.Data.Models.Business.Navigation;

public record WaypointTrafficBusinessModel
{
  public required int Id { get; set; }

  public required int FeatureId { get; set; }

  public required int WaypointFromId { get; set; }

  public required int WaypointToId { get; set; }

  public required bool Overridable { get; set; }

  public double? Cost { get; set; }

  public double? SpeedLimit { get; set; }

  public double? AbsoluteSpeedLimit { get; set; }
}

public static class WaypointTrafficBusinessModelExtensions
{
  public static WaypointTrafficDto ToDto(this WaypointTrafficBusinessModel model)
  {
    return new WaypointTrafficDto
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