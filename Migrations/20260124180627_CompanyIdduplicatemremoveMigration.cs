using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminDashboard.Migrations
{
    /// <inheritdoc />
    public partial class CompanyIdduplicatemremoveMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Companies_CompanyId1",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Stops_CompanyId1",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                table: "Stops");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId1",
                table: "Stops",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stops_CompanyId1",
                table: "Stops",
                column: "CompanyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Companies_CompanyId1",
                table: "Stops",
                column: "CompanyId1",
                principalTable: "Companies",
                principalColumn: "CompanyId");
        }
    }
}
