namespace LGDXRobotCloud.Data.Models.Business.Navigation;

public record WaypointTrafficUpdateBusinessModel
{
  public int? Id { get; set; }

  public required int FeatureId { get; set; }

  public int? WaypointFromId { get; set; }
  
  public int? WaypointToId { get; set; }

  public Guid? AlternativeWaypointFromId { get; set; }

  public Guid? AlternativeWaypointToId { get; set; }

  public required bool Overridable { get; set; }

  public double? Cost { get; set; }

  public double? SpeedLimit { get; set; }

  public double? AbsoluteSpeedLimit { get; set; }
}