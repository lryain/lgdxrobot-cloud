namespace LGDXRobotCloud.Data.Models.Business.Navigation;

public record WaypointUpdateBusinessModel
{
  public required string Name { get; set; }

  public int? FeatureId { get; set; }

  public string? ClassName { get; set; }

  public required double X { get; set; }

  public required double Y { get; set; }

  public required double Rotation { get; set; }

  public required bool IsDocking { get; set; }
}