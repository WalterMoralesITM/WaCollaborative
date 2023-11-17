using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaCollaborative.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPortfolioProductProductNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioProducts_Products_ProductId",
                table: "PortfolioProducts");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioProducts_PortfolioId_ProductId",
                table: "PortfolioProducts");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "PortfolioProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioProducts_PortfolioId_ProductId",
                table: "PortfolioProducts",
                columns: new[] { "PortfolioId", "ProductId" },
                unique: true,
                filter: "[PortfolioId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioProducts_Products_ProductId",
                table: "PortfolioProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioProducts_Products_ProductId",
                table: "PortfolioProducts");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioProducts_PortfolioId_ProductId",
                table: "PortfolioProducts");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "PortfolioProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioProducts_PortfolioId_ProductId",
                table: "PortfolioProducts",
                columns: new[] { "PortfolioId", "ProductId" },
                unique: true,
                filter: "[PortfolioId] IS NOT NULL AND [ProductId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioProducts_Products_ProductId",
                table: "PortfolioProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
