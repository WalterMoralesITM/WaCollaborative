using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaCollaborative.Backend.Migrations
{
    /// <inheritdoc />
    public partial class RemovePortfolioCustomerProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortfolioCustomerProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PortfolioCustomerProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortfolioCustomerId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioCustomerProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortfolioCustomerProducts_PortfolioCustomers_PortfolioCustomerId",
                        column: x => x.PortfolioCustomerId,
                        principalTable: "PortfolioCustomers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PortfolioCustomerProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioCustomerProducts_PortfolioCustomerId_ProductId",
                table: "PortfolioCustomerProducts",
                columns: new[] { "PortfolioCustomerId", "ProductId" },
                unique: true,
                filter: "[PortfolioCustomerId] IS NOT NULL AND [ProductId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioCustomerProducts_ProductId",
                table: "PortfolioCustomerProducts",
                column: "ProductId");
        }
    }
}
