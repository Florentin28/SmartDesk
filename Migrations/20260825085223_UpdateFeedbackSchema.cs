using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartDesk.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFeedbackSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSatisfied",
                table: "Feedbacks",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSatisfied",
                table: "Feedbacks");
        }
    }
}
