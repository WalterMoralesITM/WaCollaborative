using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaCollaborative.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCollaborationCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CollaborationCycleId",
                table: "CollaborationCalendars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CollaborationCycles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Period = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborationCycles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollaborationCycles_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationCalendars_CollaborationCycleId",
                table: "CollaborationCalendars",
                column: "CollaborationCycleId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationCycles_StatusId",
                table: "CollaborationCycles",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_CollaborationCalendars_CollaborationCycles_CollaborationCycleId",
                table: "CollaborationCalendars",
                column: "CollaborationCycleId",
                principalTable: "CollaborationCycles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollaborationCalendars_CollaborationCycles_CollaborationCycleId",
                table: "CollaborationCalendars");

            migrationBuilder.DropTable(
                name: "CollaborationCycles");

            migrationBuilder.DropIndex(
                name: "IX_CollaborationCalendars_CollaborationCycleId",
                table: "CollaborationCalendars");

            migrationBuilder.DropColumn(
                name: "CollaborationCycleId",
                table: "CollaborationCalendars");
        }
    }
}
