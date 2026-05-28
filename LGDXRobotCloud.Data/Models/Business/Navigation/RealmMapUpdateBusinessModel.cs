namespace LGDXRobotCloud.Data.Models.Business.Navigation;

public record RealmMapUpdateBusinessModel
{
  // All fields are required in SLAM mode

  public required string Map { get; set; }

  public required double Resolution { get; set; }

  public required double OriginX { get; set; }

  public required double OriginY { get; set; }

  public required double OriginRotation { get; set; }
}