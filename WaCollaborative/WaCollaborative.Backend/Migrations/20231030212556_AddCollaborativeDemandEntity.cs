using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaCollaborative.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCollaborativeDemandEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollaborativeDemand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandTypeId = table.Column<int>(type: "int", nullable: false),
                    EventTypeId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ShippingPointId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborativeDemand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollaborativeDemand_DemandTypes_DemandTypeId",
                        column: x => x.DemandTypeId,
                        principalTable: "DemandTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaborativeDemand_EventTypes_EventTypeId",
                        column: x => x.EventTypeId,
                        principalTable: "EventTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaborativeDemand_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaborativeDemand_ShippingPoints_ShippingPointId",
                        column: x => x.ShippingPointId,
                        principalTable: "ShippingPoints",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CollaborativeDemand_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CollaborativeDemandComponentsDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YearMonth = table.Column<int>(type: "int", nullable: false),
                    CollaborativeDemandId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborativeDemandComponentsDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollaborativeDemandComponentsDetail_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CollaborativeDemandComponentsDetail_CollaborativeDemand_CollaborativeDemandId",
                        column: x => x.CollaborativeDemandId,
                        principalTable: "CollaborativeDemand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeDemand_DemandTypeId_EventTypeId_ProductId_ShippingPointId",
                table: "CollaborativeDemand",
                columns: new[] { "DemandTypeId", "EventTypeId", "ProductId", "ShippingPointId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeDemand_EventTypeId",
                table: "CollaborativeDemand",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeDemand_ProductId",
                table: "CollaborativeDemand",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeDemand_ShippingPointId",
                table: "CollaborativeDemand",
                column: "ShippingPointId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeDemand_StatusId",
                table: "CollaborativeDemand",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeDemandComponentsDetail_CollaborativeDemandId",
                table: "CollaborativeDemandComponentsDetail",
                column: "CollaborativeDemandId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeDemandComponentsDetail_UserId",
                table: "CollaborativeDemandComponentsDetail",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeDemandComponentsDetail_YearMonth_CollaborativeDemandId_UserId",
                table: "CollaborativeDemandComponentsDetail",
                columns: new[] { "YearMonth", "CollaborativeDemandId", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollaborativeDemandComponentsDetail");

            migrationBuilder.DropTable(
                name: "CollaborativeDemand");
        }
    }
}
