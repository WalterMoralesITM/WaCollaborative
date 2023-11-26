using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaCollaborative.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCollaborativeDemandUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollaborativeDemandUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CollaborativeDemandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborativeDemandUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollaborativeDemandUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaborativeDemandUsers_CollaborativeDemand_CollaborativeDemandId",
                        column: x => x.CollaborativeDemandId,
                        principalTable: "CollaborativeDemand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeDemandUsers_CollaborativeDemandId",
                table: "CollaborativeDemandUsers",
                column: "CollaborativeDemandId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeDemandUsers_UserId",
                table: "CollaborativeDemandUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollaborativeDemandUsers");
        }
    }
}
