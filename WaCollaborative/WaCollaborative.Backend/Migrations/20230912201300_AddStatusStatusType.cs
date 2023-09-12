using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaCollaborative.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusStatusType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatusType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StatusTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Status_StatusType_StatusTypeId",
                        column: x => x.StatusTypeId,
                        principalTable: "StatusType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementUnits_Name",
                table: "MeasurementUnits",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Status_Name_StatusTypeId",
                table: "Status",
                columns: new[] { "Name", "StatusTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Status_StatusTypeId",
                table: "Status",
                column: "StatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusType_Name",
                table: "StatusType",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "StatusType");

            migrationBuilder.DropIndex(
                name: "IX_MeasurementUnits_Name",
                table: "MeasurementUnits");
        }
    }
}
