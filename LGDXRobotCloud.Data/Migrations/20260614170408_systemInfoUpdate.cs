using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LGDXRobotCloud.Data.Migrations
{
    /// <inheritdoc />
    public partial class systemInfoUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is32Bit",
                table: "RobotSystemInfos");

            migrationBuilder.AddColumn<string>(
                name: "CpuArchitecture",
                table: "RobotSystemInfos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CpuCores",
                table: "RobotSystemInfos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CpuArchitecture",
                table: "RobotSystemInfos");

            migrationBuilder.DropColumn(
                name: "CpuCores",
                table: "RobotSystemInfos");

            migrationBuilder.AddColumn<bool>(
                name: "Is32Bit",
                table: "RobotSystemInfos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
