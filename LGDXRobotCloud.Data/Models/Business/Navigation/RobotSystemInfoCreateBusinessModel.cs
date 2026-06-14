namespace LGDXRobotCloud.Data.Models.Business.Navigation;

public record RobotSystemInfoCreateBusinessModel
{  
  public required string Cpu { get; set; }

  public required int CpuCores { get; set; }

  public required string CpuArchitecture { get; set; } 

  public required bool IsLittleEndian { get; set; }

  public required int RamMiB { get; set; }

  public required string Motherboard { get; set; }

  public required string MotherboardSerialNumber { get; set; }

  public string? Gpu { get; set; }

  public required string Os { get; set; }

}