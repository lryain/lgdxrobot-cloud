using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LGDXRobotCloud.Utilities.Constants;

namespace LGDXRobotCloud.Data.Entities;

public class Realm
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  [MaxLength(50)]
  [Required]
  public string Name { get; set; } = null!;

  [MaxLength(200)]
  public string? Description { get; set; }

  [Required]
  public bool HasRouteControl { get; set; }

  [MaxLength(LgdxApiConstants.ImageMaxSize)]
  public byte[] Map { get; set; } = [];

  public int MapWidth { get; set; } = 0;

  public int MapHeight { get; set; } = 0;

  [MaxLength(LgdxApiConstants.ImageMaxSize)]
  public byte[] KeepoutMask { get; set; } = [];

  [MaxLength(LgdxApiConstants.ImageMaxSize)]
  public byte[] SpeedMask { get; set; } = [];

  public double Resolution { get; set; } = 0;

  public double OriginX { get; set; } = 0;

  public double OriginY { get; set; } = 0;

  public double OriginRotation { get; set; } = 0;
}