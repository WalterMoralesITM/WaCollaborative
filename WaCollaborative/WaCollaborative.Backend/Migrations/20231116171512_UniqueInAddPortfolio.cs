using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaCollaborative.Backend.Migrations
{
    /// <inheritdoc />
    public partial class UniqueInAddPortfolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PortfolioCustomers_PortfolioId",
                table: "PortfolioCustomers");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioCustomerProducts_PortfolioCustomerId",
                table: "PortfolioCustomerProducts");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioCustomers_PortfolioId_CustomerId",
                table: "PortfolioCustomers",
                columns: new[] { "PortfolioId", "CustomerId" },
                unique: true,
                filter: "[PortfolioId] IS NOT NULL AND [CustomerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioCustomerProducts_PortfolioCustomerId_ProductId",
                table: "PortfolioCustomerProducts",
                columns: new[] { "PortfolioCustomerId", "ProductId" },
                unique: true,
                filter: "[PortfolioCustomerId] IS NOT NULL AND [ProductId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PortfolioCustomers_PortfolioId_CustomerId",
                table: "PortfolioCustomers");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioCustomerProducts_PortfolioCustomerId_ProductId",
                table: "PortfolioCustomerProducts");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioCustomers_PortfolioId",
                table: "PortfolioCustomers",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioCustomerProducts_PortfolioCustomerId",
                table: "PortfolioCustomerProducts",
                column: "PortfolioCustomerId");
        }
    }
}
