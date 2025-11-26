using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAPI.Migrations
{
    /// <inheritdoc />
    public partial class hoteliapi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prezime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Hoteli",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KontaktBroj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KontaktEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlSlike = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MjestoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StatusHotelaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hoteli", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hoteli_Mjesta_MjestoId",
                        column: x => x.MjestoId,
                        principalTable: "Mjesta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Hoteli_StatusiHotela_StatusHotelaId",
                        column: x => x.StatusHotelaId,
                        principalTable: "StatusiHotela",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateTable(
                name: "Soba",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrojKreveta = table.Column<int>(type: "int", nullable: false),
                    CijenaNocenja = table.Column<int>(type: "int", nullable: false),
                    HotelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Soba", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Soba_Hoteli_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hoteli",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacija",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DatumKreiranja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumOd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumDo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Napomena = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    SobaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacija", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rezervacija_AspNetUsers_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezervacija_Soba_SobaId",
                        column: x => x.SobaId,
                        principalTable: "Soba",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SlikaSobe",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UrlSlike = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NaslovSlike = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpisSlike = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SobaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlikaSobe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SlikaSobe_Soba_SobaId",
                        column: x => x.SobaId,
                        principalTable: "Soba",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "StatusiRezervacija",
                keyColumn: "Id",
                keyValue: 1,
                column: "Naziv",
                value: "UObradi");

            migrationBuilder.CreateIndex(
                name: "IX_Hoteli_MjestoId",
                table: "Hoteli",
                column: "MjestoId");

            migrationBuilder.CreateIndex(
                name: "IX_Hoteli_StatusHotelaId",
                table: "Hoteli",
                column: "StatusHotelaId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelKorisnik_UpraviteljiId",
                table: "HotelKorisnik",
                column: "UpraviteljiId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_KorisnikId",
                table: "Rezervacija",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_SobaId",
                table: "Rezervacija",
                column: "SobaId");

            migrationBuilder.CreateIndex(
                name: "IX_SlikaSobe_SobaId",
                table: "SlikaSobe",
                column: "SobaId");

            migrationBuilder.CreateIndex(
                name: "IX_Soba_HotelId",
                table: "Soba",
                column: "HotelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelKorisnik");

            migrationBuilder.DropTable(
                name: "Rezervacija");

            migrationBuilder.DropTable(
                name: "SlikaSobe");

            migrationBuilder.DropTable(
                name: "Soba");

            migrationBuilder.DropTable(
                name: "Hoteli");

            migrationBuilder.DropColumn(
                name: "Ime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Prezime",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "StatusiRezervacija",
                keyColumn: "Id",
                keyValue: 1,
                column: "Naziv",
                value: "U_obradi");
        }
    }
}
