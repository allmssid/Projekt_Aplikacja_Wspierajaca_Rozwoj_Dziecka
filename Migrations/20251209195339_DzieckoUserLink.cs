using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aplikacja_wspierajaca_rozwoj_dziecka.Migrations
{
    /// <inheritdoc />
    public partial class DzieckoUserLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Dzieci",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dzieci_UserId",
                table: "Dzieci",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dzieci_AspNetUsers_UserId",
                table: "Dzieci",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dzieci_AspNetUsers_UserId",
                table: "Dzieci");

            migrationBuilder.DropIndex(
                name: "IX_Dzieci_UserId",
                table: "Dzieci");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Dzieci");
        }
    }
}
