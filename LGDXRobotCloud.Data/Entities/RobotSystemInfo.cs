using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LGDXRobotCloud.Data.Entities;

public class RobotSystemInfo
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  // CPU
  [MaxLength(100)]
  public string Cpu { get; set; } = string.Empty;

  public int CpuCores { get; set; }

  [MaxLength(100)]
  public string CpuArchitecture { get; set; } = string.Empty;

  public bool IsLittleEndian { get; set; }

  // Storage (HDD, SSD) can be changed, so it is not here
  // Memory
  public int RamMiB { get; set; }

  // Motherboard
  [MaxLength(100)]
  public string Motherboard { get; set; } = null!;

  [MaxLength(100)]
  public string MotherboardSerialNumber { get; set; } = null!;

  // GPU
  [MaxLength(100)]
  public string? Gpu { get; set; }

  [MaxLength(100)]
  public string Os { get; set; } = null!;


  [ForeignKey("RobotId")]
  public Robot Robot { get; set; } = null!;

  public Guid RobotId { get; set; }
}
