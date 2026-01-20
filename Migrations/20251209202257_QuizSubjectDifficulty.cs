using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aplikacja_wspierajaca_rozwoj_dziecka.Migrations
{
    /// <inheritdoc />
    public partial class QuizSubjectDifficulty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Poziom",
                table: "MathQuizProby",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Przedmiot",
                table: "MathQuizProby",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Poziom",
                table: "MathQuizProby");

            migrationBuilder.DropColumn(
                name: "Przedmiot",
                table: "MathQuizProby");
        }
    }
}
