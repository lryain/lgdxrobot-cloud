using System.ComponentModel.DataAnnotations;
using LGDXRobotCloud.UI.Client.Models;
using LGDXRobotCloud.UI.ViewModels.Shared;

namespace LGDXRobotCloud.UI.ViewModels.Navigation;

public class WaypointTrafficViewModel : FormViewModelBase
{
  public int? Id { get; set; }

  [Required (ErrorMessage = "A feature ID is required.")]
  public int FeatureId { get; set; }

  public int? WaypointFromId { get; set; }

  public int? WaypointToId { get; set; }

  public Guid? AlternativeWaypointFromId { get; set; }

  public Guid? AlternativeWaypointToId { get; set; }

  public bool Overridable { get; set; }

  public double? Cost { get; set; }

  [Range(0.0, 100.0)]
  public double? SpeedLimit { get; set; }

  public double? AbsoluteSpeedLimit { get; set; }
}

public static class WaypointTrafficViewModelExtensions
{
  public static void FromDto(this WaypointTrafficViewModel WaypointDetailsViewModel, WaypointTrafficDto waypointTrafficDto)
  {
    WaypointDetailsViewModel.Id = waypointTrafficDto.Id; 
    WaypointDetailsViewModel.FeatureId = (int)waypointTrafficDto.FeatureId!;  
    WaypointDetailsViewModel.WaypointFromId = (int)waypointTrafficDto.WaypointFromId!; 
    WaypointDetailsViewModel.WaypointToId = (int)waypointTrafficDto.WaypointToId!; 
    WaypointDetailsViewModel.Overridable = (bool)waypointTrafficDto.Overridable!; 
    WaypointDetailsViewModel.Cost = waypointTrafficDto.Cost; 
    WaypointDetailsViewModel.SpeedLimit = waypointTrafficDto.SpeedLimit; 
    WaypointDetailsViewModel.AbsoluteSpeedLimit = waypointTrafficDto.AbsoluteSpeedLimit; 
  }
}