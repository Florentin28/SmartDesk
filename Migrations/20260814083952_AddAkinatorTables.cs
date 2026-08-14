using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartDesk.Migrations
{
    /// <inheritdoc />
    public partial class AddAkinatorTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Procedures");

            migrationBuilder.DropColumn(
                name: "FailedCount",
                table: "Procedures");

            migrationBuilder.DropColumn(
                name: "SolutionSteps",
                table: "Procedures");

            migrationBuilder.RenameColumn(
                name: "SubCategory",
                table: "Procedures",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "HelpfulCount",
                table: "Procedures",
                newName: "SuccessCount");

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    NextQuestionId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProcedureId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Procedures_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "Procedures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answers_Questions_NextQuestionId",
                        column: x => x.NextQuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_NextQuestionId",
                table: "Answers",
                column: "NextQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_ProcedureId",
                table: "Answers",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.RenameColumn(
                name: "SuccessCount",
                table: "Procedures",
                newName: "HelpfulCount");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Procedures",
                newName: "SubCategory");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Procedures",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FailedCount",
                table: "Procedures",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SolutionSteps",
                table: "Procedures",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
