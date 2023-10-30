using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaCollaborative.Backend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEventTypeFromDemandTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemandTypes_EventTypes_EventTypeId",
                table: "DemandTypes");

            migrationBuilder.DropIndex(
                name: "IX_DemandTypes_EventTypeId",
                table: "DemandTypes");

            migrationBuilder.DropIndex(
                name: "IX_DemandTypes_Name_EventTypeId",
                table: "DemandTypes");

            migrationBuilder.DropColumn(
                name: "EventTypeId",
                table: "DemandTypes");

            migrationBuilder.CreateIndex(
                name: "IX_DemandTypes_Name",
                table: "DemandTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DemandTypes_Name",
                table: "DemandTypes");

            migrationBuilder.AddColumn<int>(
                name: "EventTypeId",
                table: "DemandTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DemandTypes_EventTypeId",
                table: "DemandTypes",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandTypes_Name_EventTypeId",
                table: "DemandTypes",
                columns: new[] { "Name", "EventTypeId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DemandTypes_EventTypes_EventTypeId",
                table: "DemandTypes",
                column: "EventTypeId",
                principalTable: "EventTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
