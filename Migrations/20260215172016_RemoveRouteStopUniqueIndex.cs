using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminDashboard.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRouteStopUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RouteStops_RouteId_StopId",
                table: "RouteStops");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStops_RouteId_StopOrder",
                table: "RouteStops",
                columns: new[] { "RouteId", "StopOrder" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RouteStops_RouteId_StopOrder",
                table: "RouteStops");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStops_RouteId_StopId",
                table: "RouteStops",
                columns: new[] { "RouteId", "StopId" },
                unique: true);
        }
    }
}
