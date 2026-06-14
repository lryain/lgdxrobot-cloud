namespace LGDXRobotCloud.Data.Models.DTOs.V1.Responses;

public record WaypointDto
{
  public required int Id { get; set; }

  public required string Name { get; set; }

  public required RealmSearchDto Realm { get; set; }

  public int? FeatureId { get; set; }

  public string? ClassName { get; set; }

  public required double X { get; set; }

  public required double Y { get; set; }

  public double Rotation { get; set; } = 0;

  public required bool IsIntermediate { get; set; }

  public required bool IsDocking { get; set; }
}
