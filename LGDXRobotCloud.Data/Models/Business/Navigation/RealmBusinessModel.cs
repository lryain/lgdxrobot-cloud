using LGDXRobotCloud.Data.Models.DTOs.V1.Responses;

namespace LGDXRobotCloud.Data.Models.Business.Navigation;

public record RealmBusinessModel
{
  public required int Id { get; set; }

  public required string Name { get; set; }

  public string? Description { get; set; }

  public required bool HasRouteControl { get; set; }

  public required string Map { get; set; }

  public string? KeepoutMask { get; set; }

  public string? SpeedMask { get; set; }

  public required double Resolution { get; set; }

  public required double OriginX { get; set; }

  public required double OriginY { get; set; }

  public required double OriginRotation { get; set; }
}

public static class RealmBusinessModelExtensions
{
  public static RealmDto ToDto(this RealmBusinessModel model)
  {
    return new RealmDto {
      Id = model.Id,
      Name = model.Name,
      Description = model.Description,
      HasRouteControl = model.HasRouteControl,
      Map = model.Map,
      KeepoutMask = model.KeepoutMask,
      SpeedMask = model.SpeedMask,
      Resolution = model.Resolution,
      OriginX = model.OriginX,
      OriginY = model.OriginY,
      OriginRotation = model.OriginRotation,
    };
  }
}