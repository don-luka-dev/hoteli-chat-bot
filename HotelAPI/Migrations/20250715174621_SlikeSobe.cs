using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAPI.Migrations
{
    /// <inheritdoc />
    public partial class SlikeSobe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlSlike",
                table: "Sobe",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "NaslovSlike",
                table: "SlikeSoba",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlSlike",
                table: "Sobe");

            migrationBuilder.AlterColumn<string>(
                name: "NaslovSlike",
                table: "SlikeSoba",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
