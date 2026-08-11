using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartDesk.Migrations
{
    /// <inheritdoc />
    public partial class AddScreenshotPathToTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScreenshotPath",
                table: "Tickets",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScreenshotPath",
                table: "Tickets");
        }
    }
}
