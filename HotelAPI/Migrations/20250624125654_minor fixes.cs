using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAPI.Migrations
{
    /// <inheritdoc />
    public partial class minorfixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_AspNetUsers_KorisnikId",
                table: "Rezervacija");

            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_Sobe_SobaId",
                table: "Rezervacija");

            migrationBuilder.DropForeignKey(
                name: "FK_SlikaSobe_Sobe_SobaId",
                table: "SlikaSobe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SlikaSobe",
                table: "SlikaSobe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rezervacija",
                table: "Rezervacija");

            migrationBuilder.RenameTable(
                name: "SlikaSobe",
                newName: "SlikeSoba");

            migrationBuilder.RenameTable(
                name: "Rezervacija",
                newName: "Rezervacije");

            migrationBuilder.RenameIndex(
                name: "IX_SlikaSobe_SobaId",
                table: "SlikeSoba",
                newName: "IX_SlikeSoba_SobaId");

            migrationBuilder.RenameIndex(
                name: "IX_Rezervacija_SobaId",
                table: "Rezervacije",
                newName: "IX_Rezervacije_SobaId");

            migrationBuilder.RenameIndex(
                name: "IX_Rezervacija_KorisnikId",
                table: "Rezervacije",
                newName: "IX_Rezervacije_KorisnikId");

            migrationBuilder.AddColumn<string>(
                name: "RezervacijeIds",
                table: "Sobe",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SlikeSobeIds",
                table: "Sobe",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_SlikeSoba",
                table: "SlikeSoba",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rezervacije",
                table: "Rezervacije",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacije_AspNetUsers_KorisnikId",
                table: "Rezervacije",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacije_Sobe_SobaId",
                table: "Rezervacije",
                column: "SobaId",
                principalTable: "Sobe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SlikeSoba_Sobe_SobaId",
                table: "SlikeSoba",
                column: "SobaId",
                principalTable: "Sobe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacije_AspNetUsers_KorisnikId",
                table: "Rezervacije");

            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacije_Sobe_SobaId",
                table: "Rezervacije");

            migrationBuilder.DropForeignKey(
                name: "FK_SlikeSoba_Sobe_SobaId",
                table: "SlikeSoba");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SlikeSoba",
                table: "SlikeSoba");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rezervacije",
                table: "Rezervacije");

            migrationBuilder.DropColumn(
                name: "RezervacijeIds",
                table: "Sobe");

            migrationBuilder.DropColumn(
                name: "SlikeSobeIds",
                table: "Sobe");

            migrationBuilder.DropColumn(
                name: "SobeIds",
                table: "Hoteli");

            migrationBuilder.DropColumn(
                name: "UpraviteljiIds",
                table: "Hoteli");

            migrationBuilder.RenameTable(
                name: "SlikeSoba",
                newName: "SlikaSobe");

            migrationBuilder.RenameTable(
                name: "Rezervacije",
                newName: "Rezervacija");

            migrationBuilder.RenameIndex(
                name: "IX_SlikeSoba_SobaId",
                table: "SlikaSobe",
                newName: "IX_SlikaSobe_SobaId");

            migrationBuilder.RenameIndex(
                name: "IX_Rezervacije_SobaId",
                table: "Rezervacija",
                newName: "IX_Rezervacija_SobaId");

            migrationBuilder.RenameIndex(
                name: "IX_Rezervacije_KorisnikId",
                table: "Rezervacija",
                newName: "IX_Rezervacija_KorisnikId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SlikaSobe",
                table: "SlikaSobe",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rezervacija",
                table: "Rezervacija",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_AspNetUsers_KorisnikId",
                table: "Rezervacija",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_Sobe_SobaId",
                table: "Rezervacija",
                column: "SobaId",
                principalTable: "Sobe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SlikaSobe_Sobe_SobaId",
                table: "SlikaSobe",
                column: "SobaId",
                principalTable: "Sobe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
