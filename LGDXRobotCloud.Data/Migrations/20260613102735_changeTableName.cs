using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LGDXRobotCloud.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Automation.AutoTaskDetails_Automation.AutoTasks_AutoTaskId",
                table: "Automation.AutoTaskDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Automation.AutoTaskDetails_Navigation.Waypoints_WaypointId",
                table: "Automation.AutoTaskDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Automation.AutoTaskJourney_Automation.AutoTasks_AutoTaskId",
                table: "Automation.AutoTaskJourney");

            migrationBuilder.DropForeignKey(
                name: "FK_Automation.AutoTaskJourney_Automation.Progresses_CurrentPro~",
                table: "Automation.AutoTaskJourney");

            migrationBuilder.DropForeignKey(
                name: "FK_Automation.AutoTasks_Automation.Flows_FlowId",
                table: "Automation.AutoTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Automation.AutoTasks_Automation.Progresses_CurrentProgressId",
                table: "Automation.AutoTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Automation.AutoTasks_Navigation.Realms_RealmId",
                table: "Automation.AutoTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Automation.AutoTasks_Navigation.Robots_AssignedRobotId",
                table: "Automation.AutoTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Automation.FlowDetails_Automation.Flows_FlowId",
                table: "Automation.FlowDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Automation.FlowDetails_Automation.Progresses_ProgressId",
                table: "Automation.FlowDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Automation.FlowDetails_Automation.Triggers_TriggerId",
                table: "Automation.FlowDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Automation.TriggerRetries_Automation.AutoTasks_AutoTaskId",
                table: "Automation.TriggerRetries");

            migrationBuilder.DropForeignKey(
                name: "FK_Automation.TriggerRetries_Automation.Triggers_TriggerId",
                table: "Automation.TriggerRetries");

            migrationBuilder.DropForeignKey(
                name: "FK_Automation.Triggers_Administration.ApiKeys_ApiKeyId",
                table: "Automation.Triggers");

            migrationBuilder.DropForeignKey(
                name: "FK_Navigation.RobotCertificates_Navigation.Robots_RobotId",
                table: "Navigation.RobotCertificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Navigation.Robots_Navigation.Realms_RealmId",
                table: "Navigation.Robots");

            migrationBuilder.DropForeignKey(
                name: "FK_Navigation.RobotSystemInfos_Navigation.Robots_RobotId",
                table: "Navigation.RobotSystemInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_Navigation.Waypoints_Navigation.Realms_RealmId",
                table: "Navigation.Waypoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Navigation.WaypointTraffics_Navigation.Realms_RealmId",
                table: "Navigation.WaypointTraffics");

            migrationBuilder.DropForeignKey(
                name: "FK_Navigation.WaypointTraffics_Navigation.Waypoints_WaypointFr~",
                table: "Navigation.WaypointTraffics");

            migrationBuilder.DropForeignKey(
                name: "FK_Navigation.WaypointTraffics_Navigation.Waypoints_WaypointTo~",
                table: "Navigation.WaypointTraffics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Navigation.WaypointTraffics",
                table: "Navigation.WaypointTraffics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Navigation.Waypoints",
                table: "Navigation.Waypoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Navigation.RobotSystemInfos",
                table: "Navigation.RobotSystemInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Navigation.Robots",
                table: "Navigation.Robots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Navigation.RobotCertificates",
                table: "Navigation.RobotCertificates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Navigation.Realms",
                table: "Navigation.Realms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Automation.Triggers",
                table: "Automation.Triggers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Automation.TriggerRetries",
                table: "Automation.TriggerRetries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Automation.Progresses",
                table: "Automation.Progresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Automation.Flows",
                table: "Automation.Flows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Automation.FlowDetails",
                table: "Automation.FlowDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Automation.AutoTasks",
                table: "Automation.AutoTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Automation.AutoTaskJourney",
                table: "Automation.AutoTaskJourney");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Automation.AutoTaskDetails",
                table: "Automation.AutoTaskDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Administration.ApiKeys",
                table: "Administration.ApiKeys");

            migrationBuilder.RenameTable(
                name: "Navigation.WaypointTraffics",
                newName: "WaypointTraffics");

            migrationBuilder.RenameTable(
                name: "Navigation.Waypoints",
                newName: "Waypoints");

            migrationBuilder.RenameTable(
                name: "Navigation.RobotSystemInfos",
                newName: "RobotSystemInfos");

            migrationBuilder.RenameTable(
                name: "Navigation.Robots",
                newName: "Robots");

            migrationBuilder.RenameTable(
                name: "Navigation.RobotCertificates",
                newName: "RobotCertificates");

            migrationBuilder.RenameTable(
                name: "Navigation.Realms",
                newName: "Realms");

            migrationBuilder.RenameTable(
                name: "Automation.Triggers",
                newName: "Triggers");

            migrationBuilder.RenameTable(
                name: "Automation.TriggerRetries",
                newName: "TriggerRetries");

            migrationBuilder.RenameTable(
                name: "Automation.Progresses",
                newName: "Progresses");

            migrationBuilder.RenameTable(
                name: "Automation.Flows",
                newName: "Flows");

            migrationBuilder.RenameTable(
                name: "Automation.FlowDetails",
                newName: "FlowDetails");

            migrationBuilder.RenameTable(
                name: "Automation.AutoTasks",
                newName: "AutoTasks");

            migrationBuilder.RenameTable(
                name: "Automation.AutoTaskJourney",
                newName: "AutoTasksJourney");

            migrationBuilder.RenameTable(
                name: "Automation.AutoTaskDetails",
                newName: "AutoTasksDetail");

            migrationBuilder.RenameTable(
                name: "Administration.ApiKeys",
                newName: "ApiKeys");

            migrationBuilder.RenameIndex(
                name: "IX_Navigation.WaypointTraffics_WaypointToId",
                table: "WaypointTraffics",
                newName: "IX_WaypointTraffics_WaypointToId");

            migrationBuilder.RenameIndex(
                name: "IX_Navigation.WaypointTraffics_WaypointFromId",
                table: "WaypointTraffics",
                newName: "IX_WaypointTraffics_WaypointFromId");

            migrationBuilder.RenameIndex(
                name: "IX_Navigation.WaypointTraffics_RealmId",
                table: "WaypointTraffics",
                newName: "IX_WaypointTraffics_RealmId");

            migrationBuilder.RenameIndex(
                name: "IX_Navigation.Waypoints_RealmId",
                table: "Waypoints",
                newName: "IX_Waypoints_RealmId");

            migrationBuilder.RenameIndex(
                name: "IX_Navigation.RobotSystemInfos_RobotId",
                table: "RobotSystemInfos",
                newName: "IX_RobotSystemInfos_RobotId");

            migrationBuilder.RenameIndex(
                name: "IX_Navigation.Robots_RealmId",
                table: "Robots",
                newName: "IX_Robots_RealmId");

            migrationBuilder.RenameIndex(
                name: "IX_Navigation.RobotCertificates_RobotId",
                table: "RobotCertificates",
                newName: "IX_RobotCertificates_RobotId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.Triggers_ApiKeyInsertLocationId",
                table: "Triggers",
                newName: "IX_Triggers_ApiKeyInsertLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.Triggers_ApiKeyId",
                table: "Triggers",
                newName: "IX_Triggers_ApiKeyId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.TriggerRetries_TriggerId",
                table: "TriggerRetries",
                newName: "IX_TriggerRetries_TriggerId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.TriggerRetries_AutoTaskId",
                table: "TriggerRetries",
                newName: "IX_TriggerRetries_AutoTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.FlowDetails_TriggerId",
                table: "FlowDetails",
                newName: "IX_FlowDetails_TriggerId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.FlowDetails_ProgressId",
                table: "FlowDetails",
                newName: "IX_FlowDetails_ProgressId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.FlowDetails_Order",
                table: "FlowDetails",
                newName: "IX_FlowDetails_Order");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.FlowDetails_FlowId",
                table: "FlowDetails",
                newName: "IX_FlowDetails_FlowId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.AutoTasks_RealmId_AssignedRobotId",
                table: "AutoTasks",
                newName: "IX_AutoTasks_RealmId_AssignedRobotId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.AutoTasks_FlowId",
                table: "AutoTasks",
                newName: "IX_AutoTasks_FlowId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.AutoTasks_CurrentProgressId",
                table: "AutoTasks",
                newName: "IX_AutoTasks_CurrentProgressId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.AutoTasks_AssignedRobotId",
                table: "AutoTasks",
                newName: "IX_AutoTasks_AssignedRobotId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.AutoTaskJourney_CurrentProgressId",
                table: "AutoTasksJourney",
                newName: "IX_AutoTasksJourney_CurrentProgressId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.AutoTaskJourney_AutoTaskId",
                table: "AutoTasksJourney",
                newName: "IX_AutoTasksJourney_AutoTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.AutoTaskDetails_WaypointId",
                table: "AutoTasksDetail",
                newName: "IX_AutoTasksDetail_WaypointId");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.AutoTaskDetails_Order",
                table: "AutoTasksDetail",
                newName: "IX_AutoTasksDetail_Order");

            migrationBuilder.RenameIndex(
                name: "IX_Automation.AutoTaskDetails_AutoTaskId",
                table: "AutoTasksDetail",
                newName: "IX_AutoTasksDetail_AutoTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaypointTraffics",
                table: "WaypointTraffics",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Waypoints",
                table: "Waypoints",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RobotSystemInfos",
                table: "RobotSystemInfos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Robots",
                table: "Robots",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RobotCertificates",
                table: "RobotCertificates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Realms",
                table: "Realms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Triggers",
                table: "Triggers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TriggerRetries",
                table: "TriggerRetries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Progresses",
                table: "Progresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flows",
                table: "Flows",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlowDetails",
                table: "FlowDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutoTasks",
                table: "AutoTasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutoTasksJourney",
                table: "AutoTasksJourney",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutoTasksDetail",
                table: "AutoTasksDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiKeys",
                table: "ApiKeys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AutoTasks_Flows_FlowId",
                table: "AutoTasks",
                column: "FlowId",
                principalTable: "Flows",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoTasks_Progresses_CurrentProgressId",
                table: "AutoTasks",
                column: "CurrentProgressId",
                principalTable: "Progresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoTasks_Realms_RealmId",
                table: "AutoTasks",
                column: "RealmId",
                principalTable: "Realms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoTasks_Robots_AssignedRobotId",
                table: "AutoTasks",
                column: "AssignedRobotId",
                principalTable: "Robots",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoTasksDetail_AutoTasks_AutoTaskId",
                table: "AutoTasksDetail",
                column: "AutoTaskId",
                principalTable: "AutoTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoTasksDetail_Waypoints_WaypointId",
                table: "AutoTasksDetail",
                column: "WaypointId",
                principalTable: "Waypoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoTasksJourney_AutoTasks_AutoTaskId",
                table: "AutoTasksJourney",
                column: "AutoTaskId",
                principalTable: "AutoTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoTasksJourney_Progresses_CurrentProgressId",
                table: "AutoTasksJourney",
                column: "CurrentProgressId",
                principalTable: "Progresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowDetails_Flows_FlowId",
                table: "FlowDetails",
                column: "FlowId",
                principalTable: "Flows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowDetails_Progresses_ProgressId",
                table: "FlowDetails",
                column: "ProgressId",
                principalTable: "Progresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowDetails_Triggers_TriggerId",
                table: "FlowDetails",
                column: "TriggerId",
                principalTable: "Triggers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RobotCertificates_Robots_RobotId",
                table: "RobotCertificates",
                column: "RobotId",
                principalTable: "Robots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Robots_Realms_RealmId",
                table: "Robots",
                column: "RealmId",
                principalTable: "Realms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RobotSystemInfos_Robots_RobotId",
                table: "RobotSystemInfos",
                column: "RobotId",
                principalTable: "Robots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TriggerRetries_AutoTasks_AutoTaskId",
                table: "TriggerRetries",
                column: "AutoTaskId",
                principalTable: "AutoTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TriggerRetries_Triggers_TriggerId",
                table: "TriggerRetries",
                column: "TriggerId",
                principalTable: "Triggers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Triggers_ApiKeys_ApiKeyId",
                table: "Triggers",
                column: "ApiKeyId",
                principalTable: "ApiKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Waypoints_Realms_RealmId",
                table: "Waypoints",
                column: "RealmId",
                principalTable: "Realms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WaypointTraffics_Realms_RealmId",
                table: "WaypointTraffics",
                column: "RealmId",
                principalTable: "Realms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WaypointTraffics_Waypoints_WaypointFromId",
                table: "WaypointTraffics",
                column: "WaypointFromId",
                principalTable: "Waypoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WaypointTraffics_Waypoints_WaypointToId",
                table: "WaypointTraffics",
                column: "WaypointToId",
                principalTable: "Waypoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutoTasks_Flows_FlowId",
                table: "AutoTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoTasks_Progresses_CurrentProgressId",
                table: "AutoTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoTasks_Realms_RealmId",
                table: "AutoTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoTasks_Robots_AssignedRobotId",
                table: "AutoTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoTasksDetail_AutoTasks_AutoTaskId",
                table: "AutoTasksDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoTasksDetail_Waypoints_WaypointId",
                table: "AutoTasksDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoTasksJourney_AutoTasks_AutoTaskId",
                table: "AutoTasksJourney");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoTasksJourney_Progresses_CurrentProgressId",
                table: "AutoTasksJourney");

            migrationBuilder.DropForeignKey(
                name: "FK_FlowDetails_Flows_FlowId",
                table: "FlowDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FlowDetails_Progresses_ProgressId",
                table: "FlowDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FlowDetails_Triggers_TriggerId",
                table: "FlowDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RobotCertificates_Robots_RobotId",
                table: "RobotCertificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Robots_Realms_RealmId",
                table: "Robots");

            migrationBuilder.DropForeignKey(
                name: "FK_RobotSystemInfos_Robots_RobotId",
                table: "RobotSystemInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_TriggerRetries_AutoTasks_AutoTaskId",
                table: "TriggerRetries");

            migrationBuilder.DropForeignKey(
                name: "FK_TriggerRetries_Triggers_TriggerId",
                table: "TriggerRetries");

            migrationBuilder.DropForeignKey(
                name: "FK_Triggers_ApiKeys_ApiKeyId",
                table: "Triggers");

            migrationBuilder.DropForeignKey(
                name: "FK_Waypoints_Realms_RealmId",
                table: "Waypoints");

            migrationBuilder.DropForeignKey(
                name: "FK_WaypointTraffics_Realms_RealmId",
                table: "WaypointTraffics");

            migrationBuilder.DropForeignKey(
                name: "FK_WaypointTraffics_Waypoints_WaypointFromId",
                table: "WaypointTraffics");

            migrationBuilder.DropForeignKey(
                name: "FK_WaypointTraffics_Waypoints_WaypointToId",
                table: "WaypointTraffics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WaypointTraffics",
                table: "WaypointTraffics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Waypoints",
                table: "Waypoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Triggers",
                table: "Triggers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TriggerRetries",
                table: "TriggerRetries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RobotSystemInfos",
                table: "RobotSystemInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Robots",
                table: "Robots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RobotCertificates",
                table: "RobotCertificates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Realms",
                table: "Realms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Progresses",
                table: "Progresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flows",
                table: "Flows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlowDetails",
                table: "FlowDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AutoTasksJourney",
                table: "AutoTasksJourney");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AutoTasksDetail",
                table: "AutoTasksDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AutoTasks",
                table: "AutoTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiKeys",
                table: "ApiKeys");

            migrationBuilder.RenameTable(
                name: "WaypointTraffics",
                newName: "Navigation.WaypointTraffics");

            migrationBuilder.RenameTable(
                name: "Waypoints",
                newName: "Navigation.Waypoints");

            migrationBuilder.RenameTable(
                name: "Triggers",
                newName: "Automation.Triggers");

            migrationBuilder.RenameTable(
                name: "TriggerRetries",
                newName: "Automation.TriggerRetries");

            migrationBuilder.RenameTable(
                name: "RobotSystemInfos",
                newName: "Navigation.RobotSystemInfos");

            migrationBuilder.RenameTable(
                name: "Robots",
                newName: "Navigation.Robots");

            migrationBuilder.RenameTable(
                name: "RobotCertificates",
                newName: "Navigation.RobotCertificates");

            migrationBuilder.RenameTable(
                name: "Realms",
                newName: "Navigation.Realms");

            migrationBuilder.RenameTable(
                name: "Progresses",
                newName: "Automation.Progresses");

            migrationBuilder.RenameTable(
                name: "Flows",
                newName: "Automation.Flows");

            migrationBuilder.RenameTable(
                name: "FlowDetails",
                newName: "Automation.FlowDetails");

            migrationBuilder.RenameTable(
                name: "AutoTasksJourney",
                newName: "Automation.AutoTaskJourney");

            migrationBuilder.RenameTable(
                name: "AutoTasksDetail",
                newName: "Automation.AutoTaskDetails");

            migrationBuilder.RenameTable(
                name: "AutoTasks",
                newName: "Automation.AutoTasks");

            migrationBuilder.RenameTable(
                name: "ApiKeys",
                newName: "Administration.ApiKeys");

            migrationBuilder.RenameIndex(
                name: "IX_WaypointTraffics_WaypointToId",
                table: "Navigation.WaypointTraffics",
                newName: "IX_Navigation.WaypointTraffics_WaypointToId");

            migrationBuilder.RenameIndex(
                name: "IX_WaypointTraffics_WaypointFromId",
                table: "Navigation.WaypointTraffics",
                newName: "IX_Navigation.WaypointTraffics_WaypointFromId");

            migrationBuilder.RenameIndex(
                name: "IX_WaypointTraffics_RealmId",
                table: "Navigation.WaypointTraffics",
                newName: "IX_Navigation.WaypointTraffics_RealmId");

            migrationBuilder.RenameIndex(
                name: "IX_Waypoints_RealmId",
                table: "Navigation.Waypoints",
                newName: "IX_Navigation.Waypoints_RealmId");

            migrationBuilder.RenameIndex(
                name: "IX_Triggers_ApiKeyInsertLocationId",
                table: "Automation.Triggers",
                newName: "IX_Automation.Triggers_ApiKeyInsertLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Triggers_ApiKeyId",
                table: "Automation.Triggers",
                newName: "IX_Automation.Triggers_ApiKeyId");

            migrationBuilder.RenameIndex(
                name: "IX_TriggerRetries_TriggerId",
                table: "Automation.TriggerRetries",
                newName: "IX_Automation.TriggerRetries_TriggerId");

            migrationBuilder.RenameIndex(
                name: "IX_TriggerRetries_AutoTaskId",
                table: "Automation.TriggerRetries",
                newName: "IX_Automation.TriggerRetries_AutoTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_RobotSystemInfos_RobotId",
                table: "Navigation.RobotSystemInfos",
                newName: "IX_Navigation.RobotSystemInfos_RobotId");

            migrationBuilder.RenameIndex(
                name: "IX_Robots_RealmId",
                table: "Navigation.Robots",
                newName: "IX_Navigation.Robots_RealmId");

            migrationBuilder.RenameIndex(
                name: "IX_RobotCertificates_RobotId",
                table: "Navigation.RobotCertificates",
                newName: "IX_Navigation.RobotCertificates_RobotId");

            migrationBuilder.RenameIndex(
                name: "IX_FlowDetails_TriggerId",
                table: "Automation.FlowDetails",
                newName: "IX_Automation.FlowDetails_TriggerId");

            migrationBuilder.RenameIndex(
                name: "IX_FlowDetails_ProgressId",
                table: "Automation.FlowDetails",
                newName: "IX_Automation.FlowDetails_ProgressId");

            migrationBuilder.RenameIndex(
                name: "IX_FlowDetails_Order",
                table: "Automation.FlowDetails",
                newName: "IX_Automation.FlowDetails_Order");

            migrationBuilder.RenameIndex(
                name: "IX_FlowDetails_FlowId",
                table: "Automation.FlowDetails",
                newName: "IX_Automation.FlowDetails_FlowId");

            migrationBuilder.RenameIndex(
                name: "IX_AutoTasksJourney_CurrentProgressId",
                table: "Automation.AutoTaskJourney",
                newName: "IX_Automation.AutoTaskJourney_CurrentProgressId");

            migrationBuilder.RenameIndex(
                name: "IX_AutoTasksJourney_AutoTaskId",
                table: "Automation.AutoTaskJourney",
                newName: "IX_Automation.AutoTaskJourney_AutoTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_AutoTasksDetail_WaypointId",
                table: "Automation.AutoTaskDetails",
                newName: "IX_Automation.AutoTaskDetails_WaypointId");

            migrationBuilder.RenameIndex(
                name: "IX_AutoTasksDetail_Order",
                table: "Automation.AutoTaskDetails",
                newName: "IX_Automation.AutoTaskDetails_Order");

            migrationBuilder.RenameIndex(
                name: "IX_AutoTasksDetail_AutoTaskId",
                table: "Automation.AutoTaskDetails",
                newName: "IX_Automation.AutoTaskDetails_AutoTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_AutoTasks_RealmId_AssignedRobotId",
                table: "Automation.AutoTasks",
                newName: "IX_Automation.AutoTasks_RealmId_AssignedRobotId");

            migrationBuilder.RenameIndex(
                name: "IX_AutoTasks_FlowId",
                table: "Automation.AutoTasks",
                newName: "IX_Automation.AutoTasks_FlowId");

            migrationBuilder.RenameIndex(
                name: "IX_AutoTasks_CurrentProgressId",
                table: "Automation.AutoTasks",
                newName: "IX_Automation.AutoTasks_CurrentProgressId");

            migrationBuilder.RenameIndex(
                name: "IX_AutoTasks_AssignedRobotId",
                table: "Automation.AutoTasks",
                newName: "IX_Automation.AutoTasks_AssignedRobotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Navigation.WaypointTraffics",
                table: "Navigation.WaypointTraffics",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Navigation.Waypoints",
                table: "Navigation.Waypoints",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Automation.Triggers",
                table: "Automation.Triggers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Automation.TriggerRetries",
                table: "Automation.TriggerRetries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Navigation.RobotSystemInfos",
                table: "Navigation.RobotSystemInfos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Navigation.Robots",
                table: "Navigation.Robots",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Navigation.RobotCertificates",
                table: "Navigation.RobotCertificates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Navigation.Realms",
                table: "Navigation.Realms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Automation.Progresses",
                table: "Automation.Progresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Automation.Flows",
                table: "Automation.Flows",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Automation.FlowDetails",
                table: "Automation.FlowDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Automation.AutoTaskJourney",
                table: "Automation.AutoTaskJourney",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Automation.AutoTaskDetails",
                table: "Automation.AutoTaskDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Automation.AutoTasks",
                table: "Automation.AutoTasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Administration.ApiKeys",
                table: "Administration.ApiKeys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Automation.AutoTaskDetails_Automation.AutoTasks_AutoTaskId",
                table: "Automation.AutoTaskDetails",
                column: "AutoTaskId",
                principalTable: "Automation.AutoTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Automation.AutoTaskDetails_Navigation.Waypoints_WaypointId",
                table: "Automation.AutoTaskDetails",
                column: "WaypointId",
                principalTable: "Navigation.Waypoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Automation.AutoTaskJourney_Automation.AutoTasks_AutoTaskId",
                table: "Automation.AutoTaskJourney",
                column: "AutoTaskId",
                principalTable: "Automation.AutoTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Automation.AutoTaskJourney_Automation.Progresses_CurrentPro~",
                table: "Automation.AutoTaskJourney",
                column: "CurrentProgressId",
                principalTable: "Automation.Progresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Automation.AutoTasks_Automation.Flows_FlowId",
                table: "Automation.AutoTasks",
                column: "FlowId",
                principalTable: "Automation.Flows",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Automation.AutoTasks_Automation.Progresses_CurrentProgressId",
                table: "Automation.AutoTasks",
                column: "CurrentProgressId",
                principalTable: "Automation.Progresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Automation.AutoTasks_Navigation.Realms_RealmId",
                table: "Automation.AutoTasks",
                column: "RealmId",
                principalTable: "Navigation.Realms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Automation.AutoTasks_Navigation.Robots_AssignedRobotId",
                table: "Automation.AutoTasks",
                column: "AssignedRobotId",
                principalTable: "Navigation.Robots",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Automation.FlowDetails_Automation.Flows_FlowId",
                table: "Automation.FlowDetails",
                column: "FlowId",
                principalTable: "Automation.Flows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Automation.FlowDetails_Automation.Progresses_ProgressId",
                table: "Automation.FlowDetails",
                column: "ProgressId",
                principalTable: "Automation.Progresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Automation.FlowDetails_Automation.Triggers_TriggerId",
                table: "Automation.FlowDetails",
                column: "TriggerId",
                principalTable: "Automation.Triggers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Automation.TriggerRetries_Automation.AutoTasks_AutoTaskId",
                table: "Automation.TriggerRetries",
                column: "AutoTaskId",
                principalTable: "Automation.AutoTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Automation.TriggerRetries_Automation.Triggers_TriggerId",
                table: "Automation.TriggerRetries",
                column: "TriggerId",
                principalTable: "Automation.Triggers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Automation.Triggers_Administration.ApiKeys_ApiKeyId",
                table: "Automation.Triggers",
                column: "ApiKeyId",
                principalTable: "Administration.ApiKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Navigation.RobotCertificates_Navigation.Robots_RobotId",
                table: "Navigation.RobotCertificates",
                column: "RobotId",
                principalTable: "Navigation.Robots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Navigation.Robots_Navigation.Realms_RealmId",
                table: "Navigation.Robots",
                column: "RealmId",
                principalTable: "Navigation.Realms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Navigation.RobotSystemInfos_Navigation.Robots_RobotId",
                table: "Navigation.RobotSystemInfos",
                column: "RobotId",
                principalTable: "Navigation.Robots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Navigation.Waypoints_Navigation.Realms_RealmId",
                table: "Navigation.Waypoints",
                column: "RealmId",
                principalTable: "Navigation.Realms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Navigation.WaypointTraffics_Navigation.Realms_RealmId",
                table: "Navigation.WaypointTraffics",
                column: "RealmId",
                principalTable: "Navigation.Realms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Navigation.WaypointTraffics_Navigation.Waypoints_WaypointFr~",
                table: "Navigation.WaypointTraffics",
                column: "WaypointFromId",
                principalTable: "Navigation.Waypoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Navigation.WaypointTraffics_Navigation.Waypoints_WaypointTo~",
                table: "Navigation.WaypointTraffics",
                column: "WaypointToId",
                principalTable: "Navigation.Waypoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
