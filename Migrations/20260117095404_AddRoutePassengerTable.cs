using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminDashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddRoutePassengerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoutePassengers",
                columns: table => new
                {
                    RoutePassengerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    StopId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutePassengers", x => x.RoutePassengerId);
                    table.ForeignKey(
                        name: "FK_RoutePassengers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoutePassengers_RouteStops_StopId",
                        column: x => x.StopId,
                        principalTable: "RouteStops",
                        principalColumn: "StopId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoutePassengers_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "RouteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoutePassengers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoutePassengers_CompanyId_UserId_RouteId",
                table: "RoutePassengers",
                columns: new[] { "CompanyId", "UserId", "RouteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoutePassengers_RouteId",
                table: "RoutePassengers",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutePassengers_StopId",
                table: "RoutePassengers",
                column: "StopId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutePassengers_UserId",
                table: "RoutePassengers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoutePassengers");
        }
    }
}
