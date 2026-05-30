using System.ComponentModel.DataAnnotations;
using LGDXRobotCloud.Data.Models.Business.Navigation;

namespace LGDXRobotCloud.Data.Models.DTOs.V1.Commands;

public record WaypointTrafficUpdateDto : IValidatableObject
{
  public int? Id { get; set; }

  [Required (ErrorMessage = "A feature ID is required.")]
  public int FeatureId { get; set; }

  public int? WaypointFromId { get; set; }

  public int? WaypointToId { get; set; }

  public Guid? AlternativeWaypointFromId { get; set; }

  public Guid? AlternativeWaypointToId { get; set; }

  [Required (ErrorMessage = "Overridable is required.")]
  public bool Overridable { get; set; }

  public double? Cost { get; set; }

  [Range(0.0, 100.0)]
  public double? SpeedLimit { get; set; }

  public double? AbsoluteSpeedLimit { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    if (WaypointFromId == null && AlternativeWaypointFromId == null)
    {
      yield return new ValidationResult("WaypointFromId or AlternativeWaypointFromId is required.", [nameof(WaypointFromId), nameof(AlternativeWaypointFromId)]);
    }
    if (WaypointToId == null && AlternativeWaypointToId == null)
    {
      yield return new ValidationResult("WaypointToId or AlternativeWaypointToId is required.", [nameof(WaypointToId), nameof(AlternativeWaypointToId)]);
    }
  }
}

public static class WaypointTrafficUpdateDtoExtensions
{
  public static WaypointTrafficUpdateBusinessModel ToBusinessModel(this WaypointTrafficUpdateDto model)
  {
    return new WaypointTrafficUpdateBusinessModel
    {
      Id = model.Id,
      FeatureId = model.FeatureId,
      WaypointFromId = model.WaypointFromId,
      WaypointToId = model.WaypointToId,
      AlternativeWaypointFromId = model.AlternativeWaypointFromId,
      AlternativeWaypointToId = model.AlternativeWaypointToId,
      Overridable = model.Overridable,
      Cost = model.Cost,
      SpeedLimit = model.SpeedLimit,
      AbsoluteSpeedLimit = model.AbsoluteSpeedLimit,
    };
  }
}