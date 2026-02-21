using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminDashboard.Migrations
{
    /// <inheritdoc />
    public partial class PassengerAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Users_DriverId",
                table: "Trip");

            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Vehicles_VehicleId",
                table: "Trip");

            migrationBuilder.CreateTable(
                name: "TripAttendances",
                columns: table => new
                {
                    TripAttendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    PassengerId = table.Column<int>(type: "int", nullable: false),
                    StopId = table.Column<int>(type: "int", nullable: false),
                    IsBoarded = table.Column<bool>(type: "bit", nullable: false),
                    MarkedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MarkedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripAttendances", x => x.TripAttendanceId);
                    table.ForeignKey(
                        name: "FK_TripAttendances_Trip_TripId",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripAttendances_TripId",
                table: "TripAttendances",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Users_DriverId",
                table: "Trip",
                column: "DriverId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Vehicles_VehicleId",
                table: "Trip",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Users_DriverId",
                table: "Trip");

            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Vehicles_VehicleId",
                table: "Trip");

            migrationBuilder.DropTable(
                name: "TripAttendances");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Users_DriverId",
                table: "Trip",
                column: "DriverId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Vehicles_VehicleId",
                table: "Trip",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId");
        }
    }
}
