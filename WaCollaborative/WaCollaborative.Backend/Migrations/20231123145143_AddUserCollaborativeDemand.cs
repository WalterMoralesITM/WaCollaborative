using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaCollaborative.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserCollaborativeDemand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCollaborativeDemands",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CollaborativeDemandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCollaborativeDemands", x => new { x.UserId, x.CollaborativeDemandId });
                    table.ForeignKey(
                        name: "FK_UserCollaborativeDemands_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCollaborativeDemands_CollaborativeDemand_CollaborativeDemandId",
                        column: x => x.CollaborativeDemandId,
                        principalTable: "CollaborativeDemand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCollaborativeDemands_CollaborativeDemandId",
                table: "UserCollaborativeDemands",
                column: "CollaborativeDemandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCollaborativeDemands");
        }
    }
}
