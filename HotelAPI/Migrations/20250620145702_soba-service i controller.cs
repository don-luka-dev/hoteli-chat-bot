using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAPI.Migrations
{
    /// <inheritdoc />
    public partial class sobaserviceicontroller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_Soba_SobaId",
                table: "Rezervacija");

            migrationBuilder.DropForeignKey(
                name: "FK_SlikaSobe_Soba_SobaId",
                table: "SlikaSobe");

            migrationBuilder.DropForeignKey(
                name: "FK_Soba_Hoteli_HotelId",
                table: "Soba");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Soba",
                table: "Soba");

            migrationBuilder.RenameTable(
                name: "Soba",
                newName: "Sobe");

            migrationBuilder.RenameIndex(
                name: "IX_Soba_HotelId",
                table: "Sobe",
                newName: "IX_Sobe_HotelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sobe",
                table: "Sobe",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Sobe_Hoteli_HotelId",
                table: "Sobe",
                column: "HotelId",
                principalTable: "Hoteli",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_Sobe_SobaId",
                table: "Rezervacija");

            migrationBuilder.DropForeignKey(
                name: "FK_SlikaSobe_Sobe_SobaId",
                table: "SlikaSobe");

            migrationBuilder.DropForeignKey(
                name: "FK_Sobe_Hoteli_HotelId",
                table: "Sobe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sobe",
                table: "Sobe");

            migrationBuilder.RenameTable(
                name: "Sobe",
                newName: "Soba");

            migrationBuilder.RenameIndex(
                name: "IX_Sobe_HotelId",
                table: "Soba",
                newName: "IX_Soba_HotelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Soba",
                table: "Soba",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_Soba_SobaId",
                table: "Rezervacija",
                column: "SobaId",
                principalTable: "Soba",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SlikaSobe_Soba_SobaId",
                table: "SlikaSobe",
                column: "SobaId",
                principalTable: "Soba",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Soba_Hoteli_HotelId",
                table: "Soba",
                column: "HotelId",
                principalTable: "Hoteli",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
