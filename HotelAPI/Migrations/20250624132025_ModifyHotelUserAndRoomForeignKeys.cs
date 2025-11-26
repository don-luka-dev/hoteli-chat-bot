using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModifyHotelUserAndRoomForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelKorisnik");

            migrationBuilder.DropColumn(
                name: "SobeIds",
                table: "Hoteli");

            migrationBuilder.DropColumn(
                name: "UpraviteljiIds",
                table: "Hoteli");

            migrationBuilder.AddColumn<Guid>(
                name: "KorisnikoviHotelId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_KorisnikoviHotelId",
                table: "AspNetUsers",
                column: "KorisnikoviHotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Hoteli_KorisnikoviHotelId",
                table: "AspNetUsers",
                column: "KorisnikoviHotelId",
                principalTable: "Hoteli",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Hoteli_KorisnikoviHotelId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_KorisnikoviHotelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "KorisnikoviHotelId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "SobeIds",
                table: "Hoteli",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpraviteljiIds",
                table: "Hoteli",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HotelKorisnik",
                columns: table => new
                {
                    KorisnikoviHoteliId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpraviteljiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelKorisnik", x => new { x.KorisnikoviHoteliId, x.UpraviteljiId });
                    table.ForeignKey(
                        name: "FK_HotelKorisnik_AspNetUsers_UpraviteljiId",
                        column: x => x.UpraviteljiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelKorisnik_Hoteli_KorisnikoviHoteliId",
                        column: x => x.KorisnikoviHoteliId,
                        principalTable: "Hoteli",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelKorisnik_UpraviteljiId",
                table: "HotelKorisnik",
                column: "UpraviteljiId");
        }
    }
}
