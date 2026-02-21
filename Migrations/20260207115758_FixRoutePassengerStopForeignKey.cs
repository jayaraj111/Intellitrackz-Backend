using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminDashboard.Migrations
{
    /// <inheritdoc />
    public partial class FixRoutePassengerStopForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop WRONG foreign key (RouteStops)
            migrationBuilder.DropForeignKey(
                name: "FK_RoutePassengers_RouteStops_StopId",
                table: "RoutePassengers");

            // Create CORRECT foreign key (Stops)
            migrationBuilder.AddForeignKey(
                name: "FK_RoutePassengers_Stops_StopId",
                table: "RoutePassengers",
                column: "StopId",
                principalTable: "Stops",
                principalColumn: "StopId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback: remove correct FK
            migrationBuilder.DropForeignKey(
                name: "FK_RoutePassengers_Stops_StopId",
                table: "RoutePassengers");

            // Restore old (incorrect) FK
            migrationBuilder.AddForeignKey(
                name: "FK_RoutePassengers_RouteStops_StopId",
                table: "RoutePassengers",
                column: "StopId",
                principalTable: "RouteStops",
                principalColumn: "RouteStopId",
                onDelete: ReferentialAction.Restrict);
        }

    }
}
