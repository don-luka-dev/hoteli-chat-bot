using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModifySobeSlikeSobaAndRezervacijaForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RezervacijeIds",
                table: "Sobe");

            migrationBuilder.DropColumn(
                name: "SlikeSobeIds",
                table: "Sobe");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
