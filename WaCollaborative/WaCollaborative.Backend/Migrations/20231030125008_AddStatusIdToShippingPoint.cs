using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaCollaborative.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusIdToShippingPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "ShippingPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingPoints_StatusId",
                table: "ShippingPoints",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingPoints_Status_StatusId",
                table: "ShippingPoints",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingPoints_Status_StatusId",
                table: "ShippingPoints");

            migrationBuilder.DropIndex(
                name: "IX_ShippingPoints_StatusId",
                table: "ShippingPoints");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "ShippingPoints");
        }
    }
}
