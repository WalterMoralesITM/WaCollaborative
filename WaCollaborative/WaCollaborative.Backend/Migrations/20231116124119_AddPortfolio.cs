using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaCollaborative.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPortfolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PortfolioCustomer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    PortfolioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioCustomer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortfolioCustomer_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PortfolioCustomer_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PortfolioCustomerProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortfolioCustomerId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioCustomerProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortfolioCustomerProduct_PortfolioCustomer_PortfolioCustomerId",
                        column: x => x.PortfolioCustomerId,
                        principalTable: "PortfolioCustomer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PortfolioCustomerProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioCustomer_CustomerId",
                table: "PortfolioCustomer",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioCustomer_PortfolioId",
                table: "PortfolioCustomer",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioCustomerProduct_PortfolioCustomerId",
                table: "PortfolioCustomerProduct",
                column: "PortfolioCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioCustomerProduct_ProductId",
                table: "PortfolioCustomerProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_Name",
                table: "Portfolios",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortfolioCustomerProduct");

            migrationBuilder.DropTable(
                name: "PortfolioCustomer");

            migrationBuilder.DropTable(
                name: "Portfolios");
        }
    }
}
