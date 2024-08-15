using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruckLoadingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTRoutesTableForReal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "TRoutes",
            columns: table => new
            {
                RouteId = table.Column<Guid>(nullable: false),
                Origin = table.Column<string>(maxLength: 100, nullable: false),
                Destination = table.Column<string>(maxLength: 100, nullable: false),
                AvailableDate = table.Column<DateTime>(nullable: false),
                DriverId = table.Column<string>(nullable: true),
                Description = table.Column<string>(maxLength: 200, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TRoutes", x => x.RouteId);
                table.ForeignKey(
                    name: "FK_TRoutes_AspNetUsers_DriverId",
                    column: x => x.DriverId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

            migrationBuilder.CreateIndex(
                name: "IX_TRoutes_DriverId",
                table: "TRoutes",
                column: "DriverId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "TRoutes");
        }
    }
}
