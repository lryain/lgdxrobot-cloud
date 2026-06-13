using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LGDXRobotCloud.Data.Entities;

public class WaypointTraffic
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  [Required]
  public int FeatureId { get; set; }

  [ForeignKey("RealmId")]
  public Realm Realm { get; set; } = null!;

  [Required]
  public int RealmId { get; set; }

  [ForeignKey("WaypointFromId")]
  public Waypoint WaypointFrom { get; set; } = null!;

  [Required]
  public int WaypointFromId { get; set; }

  [ForeignKey("WaypointToId")]
  public Waypoint WaypointTo { get; set; } = null!;

  [Required]
  public int WaypointToId { get; set; }

  [Required]
  public bool Overridable { get; set; }

  public double? Cost { get; set; }

  [Range(0.0, 100.0)]
  public double? SpeedLimit { get; set; }

  public double? AbsoluteSpeedLimit { get; set; }
}
