namespace LGDXRobotCloud.Data.Models.Business.Navigation;

public record WaypointUpsertBusinessModel
{
  public int? Id { get; set; }

  public required string Name { get; set; }

  // For Map Editor No realmId is required

  public int? FeatureId { get; set; }

  public string? ClassName { get; set; }

  public required double X { get; set; }

  public required double Y { get; set; }

  public double Rotation { get; set; } = 0;

  public required bool IsIntermediate { get; set; }

  public required bool IsDocking { get; set; }

  public Guid? AlternateId { get; set; }
}