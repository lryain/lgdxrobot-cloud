using System.ComponentModel.DataAnnotations;
using LGDXRobotCloud.Data.Models.Business.Navigation;

namespace LGDXRobotCloud.Data.Models.DTOs.V1.Commands;

public record WaypointUpsertDto
{
  public int? Id { get; set; }

  [MaxLength(100)]
  [Required (ErrorMessage = "Please enter a name.")]
  public required string Name { get; set; }

  [Required (ErrorMessage = "A realm is required.")]
  public required int RealmId { get; set; }

  public int? FeatureId { get; set; }

  [MaxLength(100)]
  public string? ClassName { get; set; }
  
  [Required (ErrorMessage = "Please enter a X coordinate.")]
  public required double X { get; set; }

  [Required (ErrorMessage = "Please enter a Y coordinate.")]
  public required double Y { get; set; }

  public double Rotation { get; set; } = 0;

  public bool IsDocking { get; set; } = false;
}

public static class WaypointUpsertDtoExtensions
{
  public static WaypointUpsertBusinessModel ToBusinessModel(this WaypointUpsertDto model)
  {
    return new WaypointUpsertBusinessModel {
      Id = model.Id,
      Name = model.Name,
      RealmId = model.RealmId,
      FeatureId = model.FeatureId,
      ClassName = model.ClassName,
      X = model.X,
      Y = model.Y,
      Rotation = model.Rotation,
      IsDocking = model.IsDocking,
    };
  }
}