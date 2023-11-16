using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaCollaborative.Backend.Migrations
{
    /// <inheritdoc />
    public partial class CountsInAddPortfolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioCustomer_Customers_CustomerId",
                table: "PortfolioCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioCustomer_Portfolios_PortfolioId",
                table: "PortfolioCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioCustomerProduct_PortfolioCustomer_PortfolioCustomerId",
                table: "PortfolioCustomerProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioCustomerProduct_Products_ProductId",
                table: "PortfolioCustomerProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PortfolioCustomerProduct",
                table: "PortfolioCustomerProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PortfolioCustomer",
                table: "PortfolioCustomer");

            migrationBuilder.RenameTable(
                name: "PortfolioCustomerProduct",
                newName: "PortfolioCustomerProducts");

            migrationBuilder.RenameTable(
                name: "PortfolioCustomer",
                newName: "PortfolioCustomers");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioCustomerProduct_ProductId",
                table: "PortfolioCustomerProducts",
                newName: "IX_PortfolioCustomerProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioCustomerProduct_PortfolioCustomerId",
                table: "PortfolioCustomerProducts",
                newName: "IX_PortfolioCustomerProducts_PortfolioCustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioCustomer_PortfolioId",
                table: "PortfolioCustomers",
                newName: "IX_PortfolioCustomers_PortfolioId");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioCustomer_CustomerId",
                table: "PortfolioCustomers",
                newName: "IX_PortfolioCustomers_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PortfolioCustomerProducts",
                table: "PortfolioCustomerProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PortfolioCustomers",
                table: "PortfolioCustomers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioCustomerProducts_PortfolioCustomers_PortfolioCustomerId",
                table: "PortfolioCustomerProducts",
                column: "PortfolioCustomerId",
                principalTable: "PortfolioCustomers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioCustomerProducts_Products_ProductId",
                table: "PortfolioCustomerProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioCustomers_Customers_CustomerId",
                table: "PortfolioCustomers",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioCustomers_Portfolios_PortfolioId",
                table: "PortfolioCustomers",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioCustomerProducts_PortfolioCustomers_PortfolioCustomerId",
                table: "PortfolioCustomerProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioCustomerProducts_Products_ProductId",
                table: "PortfolioCustomerProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioCustomers_Customers_CustomerId",
                table: "PortfolioCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioCustomers_Portfolios_PortfolioId",
                table: "PortfolioCustomers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PortfolioCustomers",
                table: "PortfolioCustomers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PortfolioCustomerProducts",
                table: "PortfolioCustomerProducts");

            migrationBuilder.RenameTable(
                name: "PortfolioCustomers",
                newName: "PortfolioCustomer");

            migrationBuilder.RenameTable(
                name: "PortfolioCustomerProducts",
                newName: "PortfolioCustomerProduct");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioCustomers_PortfolioId",
                table: "PortfolioCustomer",
                newName: "IX_PortfolioCustomer_PortfolioId");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioCustomers_CustomerId",
                table: "PortfolioCustomer",
                newName: "IX_PortfolioCustomer_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioCustomerProducts_ProductId",
                table: "PortfolioCustomerProduct",
                newName: "IX_PortfolioCustomerProduct_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioCustomerProducts_PortfolioCustomerId",
                table: "PortfolioCustomerProduct",
                newName: "IX_PortfolioCustomerProduct_PortfolioCustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PortfolioCustomer",
                table: "PortfolioCustomer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PortfolioCustomerProduct",
                table: "PortfolioCustomerProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioCustomer_Customers_CustomerId",
                table: "PortfolioCustomer",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioCustomer_Portfolios_PortfolioId",
                table: "PortfolioCustomer",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioCustomerProduct_PortfolioCustomer_PortfolioCustomerId",
                table: "PortfolioCustomerProduct",
                column: "PortfolioCustomerId",
                principalTable: "PortfolioCustomer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioCustomerProduct_Products_ProductId",
                table: "PortfolioCustomerProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
