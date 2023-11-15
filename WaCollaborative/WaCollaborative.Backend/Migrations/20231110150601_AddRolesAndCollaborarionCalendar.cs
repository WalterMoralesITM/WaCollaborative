using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaCollaborative.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesAndCollaborarionCalendar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InternalRoleId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InternalRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CollaborationCalendars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InternalRoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborationCalendars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollaborationCalendars_InternalRoles_InternalRoleId",
                        column: x => x.InternalRoleId,
                        principalTable: "InternalRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_InternalRoleId",
                table: "AspNetUsers",
                column: "InternalRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationCalendars_InternalRoleId",
                table: "CollaborationCalendars",
                column: "InternalRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalRoles_Name",
                table: "InternalRoles",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_InternalRoles_InternalRoleId",
                table: "AspNetUsers",
                column: "InternalRoleId",
                principalTable: "InternalRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_InternalRoles_InternalRoleId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CollaborationCalendars");

            migrationBuilder.DropTable(
                name: "InternalRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_InternalRoleId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InternalRoleId",
                table: "AspNetUsers");
        }
    }
}
