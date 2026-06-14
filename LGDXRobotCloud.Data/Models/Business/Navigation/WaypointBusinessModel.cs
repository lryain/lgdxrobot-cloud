using LGDXRobotCloud.Data.Models.DTOs.V1.Responses;

namespace LGDXRobotCloud.Data.Models.Business.Navigation;

public record WaypointBusinessModel
{
  public required int Id { get; set; }

  public required string Name { get; set; }

  public required int RealmId { get; set; }

  public required string RealmName { get; set; }

  public int? FeatureId { get; set; }

  public string? ClassName { get; set; }

  public required double X { get; set; }

  public required double Y { get; set; }

  public double Rotation { get; set; } = 0;

  public required bool IsIntermediate { get; set; }

  public required bool IsDocking { get; set; }
}

public static class WaypointBusinessModelExtensions
{
  public static WaypointDto ToDto(this WaypointBusinessModel model)
  {
    return new WaypointDto {
      Id = model.Id,
      Name = model.Name,
      Realm = new RealmSearchDto {
        Id = model.RealmId,
        Name = model.RealmName,
      },
      FeatureId = model.FeatureId,
      ClassName = model.ClassName,
      X = model.X,
      Y = model.Y,
      Rotation = model.Rotation,
      IsIntermediate = model.IsIntermediate,
      IsDocking = model.IsDocking,
    };
  }
}