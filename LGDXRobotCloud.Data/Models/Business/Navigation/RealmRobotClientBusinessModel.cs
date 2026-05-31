namespace LGDXRobotCloud.Data.Models.Business.Navigation;

public record RealmRobotClientBusinessModel
{
  public int MapWidth { get; set; } = 0;

  public int MapHeight { get; set; } = 0;

  public double Resolution { get; set; } = 0.05;

  public double OriginX { get; set; } = 0;

  public double OriginY { get; set; } = 0;

  public double OriginRotation { get; set; } = 0;

  public byte[] Map { get; set; } = [];

  public byte[] KeepoutMask { get; set; } = [];

  public byte[] SpeedMask { get; set; } = [];
}