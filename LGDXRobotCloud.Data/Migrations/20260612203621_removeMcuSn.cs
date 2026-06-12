using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LGDXRobotCloud.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeMcuSn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "McuSerialNumber",
                table: "Navigation.RobotSystemInfos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "McuSerialNumber",
                table: "Navigation.RobotSystemInfos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
