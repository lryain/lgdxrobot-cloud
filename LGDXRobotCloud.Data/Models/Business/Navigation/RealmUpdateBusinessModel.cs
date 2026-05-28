namespace LGDXRobotCloud.Data.Models.Business.Navigation;

public record RealmUpdateBusinessModel
{
  public required string Name { get; set; }

  public string? Description { get; set; }

  public required bool HasRouteControl { get; set; }

  public string? Map { get; set; }

  public string? KeepoutMask { get; set; }

  public string? SpeedMask { get; set; }

  public double Resolution { get; set; }

  public double OriginX { get; set; }

  public double OriginY { get; set; }

  public double OriginRotation { get; set; }
}