using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aplikacja_wspierajaca_rozwoj_dziecka.Migrations
{
    /// <inheritdoc />
    public partial class DodanieMathQuizAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MathQuizProby",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UzytkownikId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataPodejscia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LiczbaPytan = table.Column<int>(type: "int", nullable: false),
                    PoprawneOdpowiedzi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MathQuizProby", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MathQuizProby_AspNetUsers_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MathQuizProby_UzytkownikId",
                table: "MathQuizProby",
                column: "UzytkownikId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MathQuizProby");
        }
    }
}
