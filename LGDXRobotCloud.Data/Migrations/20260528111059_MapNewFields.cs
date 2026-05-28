using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LGDXRobotCloud.Data.Migrations
{
    /// <inheritdoc />
    public partial class MapNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCharger",
                table: "Navigation.Waypoints");

            migrationBuilder.DropColumn(
                name: "IsParking",
                table: "Navigation.Waypoints");

            migrationBuilder.RenameColumn(
                name: "IsReserved",
                table: "Navigation.Waypoints",
                newName: "IsDocking");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Navigation.Realms",
                newName: "SpeedMask");

            migrationBuilder.AddColumn<double>(
                name: "AbsoluteSpeedLimit",
                table: "Navigation.WaypointTraffics",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Cost",
                table: "Navigation.WaypointTraffics",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FeatureId",
                table: "Navigation.WaypointTraffics",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Overridable",
                table: "Navigation.WaypointTraffics",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "SpeedLimit",
                table: "Navigation.WaypointTraffics",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "Navigation.Waypoints",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FeatureId",
                table: "Navigation.Waypoints",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "KeepoutMask",
                table: "Navigation.Realms",
                type: "bytea",
                maxLength: 16777216,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "Map",
                table: "Navigation.Realms",
                type: "bytea",
                maxLength: 16777216,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.UpdateData(
                table: "Navigation.Realms",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "KeepoutMask", "Map" },
                values: new object[] { new byte[0], new byte[0] });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AbsoluteSpeedLimit",
                table: "Navigation.WaypointTraffics");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Navigation.WaypointTraffics");

            migrationBuilder.DropColumn(
                name: "FeatureId",
                table: "Navigation.WaypointTraffics");

            migrationBuilder.DropColumn(
                name: "Overridable",
                table: "Navigation.WaypointTraffics");

            migrationBuilder.DropColumn(
                name: "SpeedLimit",
                table: "Navigation.WaypointTraffics");

            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "Navigation.Waypoints");

            migrationBuilder.DropColumn(
                name: "FeatureId",
                table: "Navigation.Waypoints");

            migrationBuilder.DropColumn(
                name: "KeepoutMask",
                table: "Navigation.Realms");

            migrationBuilder.DropColumn(
                name: "Map",
                table: "Navigation.Realms");

            migrationBuilder.RenameColumn(
                name: "IsDocking",
                table: "Navigation.Waypoints",
                newName: "IsReserved");

            migrationBuilder.RenameColumn(
                name: "SpeedMask",
                table: "Navigation.Realms",
                newName: "Image");

            migrationBuilder.AddColumn<bool>(
                name: "HasCharger",
                table: "Navigation.Waypoints",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsParking",
                table: "Navigation.Waypoints",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
