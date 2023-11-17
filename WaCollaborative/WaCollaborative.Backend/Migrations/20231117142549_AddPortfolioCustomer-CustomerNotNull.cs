using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaCollaborative.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPortfolioCustomerCustomerNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioCustomers_Customers_CustomerId",
                table: "PortfolioCustomers");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioCustomers_PortfolioId_CustomerId",
                table: "PortfolioCustomers");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "PortfolioCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioCustomers_PortfolioId_CustomerId",
                table: "PortfolioCustomers",
                columns: new[] { "PortfolioId", "CustomerId" },
                unique: true,
                filter: "[PortfolioId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioCustomers_Customers_CustomerId",
                table: "PortfolioCustomers",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioCustomers_Customers_CustomerId",
                table: "PortfolioCustomers");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioCustomers_PortfolioId_CustomerId",
                table: "PortfolioCustomers");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "PortfolioCustomers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioCustomers_PortfolioId_CustomerId",
                table: "PortfolioCustomers",
                columns: new[] { "PortfolioId", "CustomerId" },
                unique: true,
                filter: "[PortfolioId] IS NOT NULL AND [CustomerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioCustomers_Customers_CustomerId",
                table: "PortfolioCustomers",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
