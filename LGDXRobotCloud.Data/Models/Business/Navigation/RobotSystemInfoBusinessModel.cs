using LGDXRobotCloud.Data.Models.DTOs.V1.Responses;

namespace LGDXRobotCloud.Data.Models.Business.Navigation;

public record RobotSystemInfoBusinessModel
{
  public required int Id { get; set; }

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

public static class RobotSystemInfoBusinessModelExtensions
{
  public static RobotSystemInfoDto ToDto(this RobotSystemInfoBusinessModel robotSystemInfo)
  {
    return new RobotSystemInfoDto {
      Id = robotSystemInfo.Id,
      Cpu = robotSystemInfo.Cpu,
      CpuCores = robotSystemInfo.CpuCores,
      CpuArchitecture = robotSystemInfo.CpuArchitecture,
      IsLittleEndian = robotSystemInfo.IsLittleEndian,
      RamMiB = robotSystemInfo.RamMiB,
      Motherboard = robotSystemInfo.Motherboard,
      MotherboardSerialNumber = robotSystemInfo.MotherboardSerialNumber,
      Gpu = robotSystemInfo.Gpu,
      Os = robotSystemInfo.Os,
    };
  }

  public static RobotSystemInfoCreateBusinessModel ToCreateBusinessModel(this RobotSystemInfoBusinessModel model)
  {
    return new RobotSystemInfoCreateBusinessModel {
      Cpu = model.Cpu,
      CpuCores = model.CpuCores,
      CpuArchitecture = model.CpuArchitecture,
      IsLittleEndian = model.IsLittleEndian,
      RamMiB = model.RamMiB,
      Motherboard = model.Motherboard,
      MotherboardSerialNumber = model.MotherboardSerialNumber,
      Gpu = model.Gpu,
      Os = model.Os,
    };
  }

  public static RobotSystemInfoUpdateBusinessModel ToUpdateBusinessModel(this RobotSystemInfoBusinessModel model)
  {
    return new RobotSystemInfoUpdateBusinessModel {
      Cpu = model.Cpu,
      CpuCores = model.CpuCores,
      CpuArchitecture = model.CpuArchitecture,
      IsLittleEndian = model.IsLittleEndian,
      RamMiB = model.RamMiB,
      Motherboard = model.Motherboard,
      MotherboardSerialNumber = model.MotherboardSerialNumber,
      Gpu = model.Gpu,
      Os = model.Os,
    };
  }
}