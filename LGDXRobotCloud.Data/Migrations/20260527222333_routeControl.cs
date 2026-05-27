using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LGDXRobotCloud.Data.Migrations
{
    /// <inheritdoc />
    public partial class routeControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasWaypointsTrafficControl",
                table: "Navigation.Realms",
                newName: "HasRouteControl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasRouteControl",
                table: "Navigation.Realms",
                newName: "HasWaypointsTrafficControl");
        }
    }
}
