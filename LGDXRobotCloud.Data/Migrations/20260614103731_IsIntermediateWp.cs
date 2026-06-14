using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LGDXRobotCloud.Data.Migrations
{
    /// <inheritdoc />
    public partial class IsIntermediateWp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIntermediate",
                table: "Waypoints",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIntermediate",
                table: "Waypoints");
        }
    }
}
