using System.ComponentModel.DataAnnotations;
using LGDXRobotCloud.Data.Models.Business.Navigation;

namespace LGDXRobotCloud.Data.Models.DTOs.V1.Commands;

public record WaypointUpdateDto
{
  [MaxLength(100)]
  [Required (ErrorMessage = "Please enter a name.")]
  public required string Name { get; set; }

  [Range(1, int.MaxValue)]
  public int? FeatureId { get; set; }

  [MaxLength(100)]
  public string? ClassName { get; set; }
  
  [Required (ErrorMessage = "Please enter a X coordinate.")]
  public required double X { get; set; }

  [Required (ErrorMessage = "Please enter a Y coordinate.")]
  public required double Y { get; set; }

  public double Rotation { get; set; } = 0;

  public bool IsIntermediate { get; set; } = false;

  public bool IsDocking { get; set; } = false;
}

public static class WaypointUpdateDtoExtensions
{
  public static WaypointUpdateBusinessModel ToBusinessModel(this WaypointUpdateDto model)
  {
    return new WaypointUpdateBusinessModel {
      Name = model.Name,
      X = model.X,
      Y = model.Y,
      FeatureId = model.FeatureId,
      ClassName = model.ClassName,
      Rotation = model.Rotation,
      IsIntermediate = model.IsIntermediate,
      IsDocking = model.IsDocking,
    };
  }
}