namespace LGDXRobotCloud.Data.Models.DTOs.V1.Responses;

public record WaypointTrafficDto
{
  public int Id { get; set; }

  public int FeatureId { get; set; }

  public int WaypointFromId { get; set; }

  public int WaypointToId { get; set; }

  public bool Overridable { get; set; }

  public double? Cost { get; set; }

  public double? SpeedLimit { get; set; }

  public double? AbsoluteSpeedLimit { get; set; }
}