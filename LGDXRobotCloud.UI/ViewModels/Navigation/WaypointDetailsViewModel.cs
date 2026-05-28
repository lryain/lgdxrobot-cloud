using System.ComponentModel.DataAnnotations;
using LGDXRobotCloud.UI.Client.Models;
using LGDXRobotCloud.UI.ViewModels.Shared;

namespace LGDXRobotCloud.UI.ViewModels.Navigation;

public class WaypointDetailsViewModel : FormViewModelBase
{
  public int Id { get; set; }

  [MaxLength(100)]
  [Required (ErrorMessage = "Please enter a name.")]
  public string Name { get; set; } = null!;

  [Required (ErrorMessage = "A realm is required.")]
  public int? RealmId { get; set; } = null;

  public int? FeatureId { get; set; }

  [MaxLength(100)]
  public string? ClassName { get; set; }

  public string? RealmName { get; set; }
  
  [Required (ErrorMessage = "Please enter a X coordinate.")]
  public double? X { get; set; } = null!;

  [Required (ErrorMessage = "Please enter a Y coordinate.")]
  public double? Y { get; set; } = null!;

  public double? Rotation { get; set; } = null!;

  public bool IsDocking { get; set; }
}

public static class WaypointDetailsViewModelExtensions
{
  public static void FromDto(this WaypointDetailsViewModel WaypointDetailsViewModel, WaypointDto waypointDto)
  {
    WaypointDetailsViewModel.Id = (int)waypointDto.Id!;
    WaypointDetailsViewModel.Name = waypointDto.Name!;
    WaypointDetailsViewModel.RealmId = waypointDto.Realm!.Id;
    WaypointDetailsViewModel.RealmName = waypointDto.Realm!.Name;
    WaypointDetailsViewModel.FeatureId = waypointDto.FeatureId;
    WaypointDetailsViewModel.ClassName = waypointDto.ClassName;
    WaypointDetailsViewModel.X = waypointDto.X;
    WaypointDetailsViewModel.Y = waypointDto.Y;
    WaypointDetailsViewModel.Rotation = waypointDto.Rotation;
    WaypointDetailsViewModel.IsDocking = (bool)waypointDto.IsDocking!;
  }

  public static WaypointUpdateDto ToUpdateDto(this WaypointDetailsViewModel WaypointDetailsViewModel)
  {
    return new WaypointUpdateDto {
      Name = WaypointDetailsViewModel.Name,
      FeatureId = WaypointDetailsViewModel.FeatureId,
      ClassName = WaypointDetailsViewModel.ClassName,
      X = WaypointDetailsViewModel.X,
      Y = WaypointDetailsViewModel.Y,
      Rotation = WaypointDetailsViewModel.Rotation,
      IsDocking = WaypointDetailsViewModel.IsDocking,
    };
  }

  public static WaypointCreateDto ToCreateDto(this WaypointDetailsViewModel WaypointDetailsViewModel)
  {
    return new WaypointCreateDto {
      Name = WaypointDetailsViewModel.Name,
      RealmId = WaypointDetailsViewModel.RealmId,
      FeatureId = WaypointDetailsViewModel.FeatureId,
      ClassName = WaypointDetailsViewModel.ClassName,
      X = WaypointDetailsViewModel.X,
      Y = WaypointDetailsViewModel.Y,
      Rotation = WaypointDetailsViewModel.Rotation,
      IsDocking = WaypointDetailsViewModel.IsDocking,
    };
  }
}