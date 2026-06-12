using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LGDXRobotCloud.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveChassisInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Navigation.RobotChassisInfos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Navigation.RobotChassisInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RobotId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatteryCount = table.Column<int>(type: "integer", nullable: false),
                    BatteryMaxVoltage = table.Column<double>(type: "double precision", nullable: false),
                    BatteryMinVoltage = table.Column<double>(type: "double precision", nullable: false),
                    ChassisLengthX = table.Column<double>(type: "double precision", nullable: false),
                    ChassisLengthY = table.Column<double>(type: "double precision", nullable: false),
                    ChassisWheelCount = table.Column<int>(type: "integer", nullable: false),
                    ChassisWheelRadius = table.Column<double>(type: "double precision", nullable: false),
                    RobotTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Navigation.RobotChassisInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Navigation.RobotChassisInfos_Navigation.Robots_RobotId",
                        column: x => x.RobotId,
                        principalTable: "Navigation.Robots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Navigation.RobotChassisInfos_RobotId",
                table: "Navigation.RobotChassisInfos",
                column: "RobotId",
                unique: true);
        }
    }
}
