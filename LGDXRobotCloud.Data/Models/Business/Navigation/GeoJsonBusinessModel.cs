using System.Text.Json;

namespace LGDXRobotCloud.Data.Models.Business.Navigation;

// CRS
public record GeoJsonCrsProperties
{
  public string Name { get; set; } = "urn:ogc:def:crs:EPSG::3857";
}

public record GeoJsonCrs
{
  public string Type { get; set; } = "name";

  public required GeoJsonCrsProperties Properties { get; set; } = null!;
}

// Features
public record GeoJsonFeaturePropertiesMetadata
{
  public double? SpeedLimit { get; set; }

  public double? AbsSpeedLimit { get; set; }
}

public record GeoJsonFeatureProperties
{
  // Feature ID
  public required int Id { get; set; }

  // For Traffic
  public int? Startid { get; set; }

  public int? Endid { get; set; }

  public bool? Overridable { get; set; }

  public double? Cost { get; set; }

  public GeoJsonFeaturePropertiesMetadata? Metadata { get; set; }
}

public record GeoJsonFeatureGeometry
{
  public required string Type { get; set; } = null!; // Point, LineString, MultiLineString

  public required object Coordinates { get; set; }
}

public record GeoJsonFeature
{
  public string Type { get; set; } = "Feature";

  public required GeoJsonFeatureProperties Properties { get; set; } = null!;

  public required GeoJsonFeatureGeometry Geometry { get; set; } = null!;
}

// Main
public record GeoJsonBusinessModel
{
  public string Type { get; set; } = "FeatureCollection";

  public required string Name { get; set; } = null!;

  public required GeoJsonCrs Crs { get; set; } = null!;

  public required GeoJsonFeature[] Features { get; set; } = null!;
}