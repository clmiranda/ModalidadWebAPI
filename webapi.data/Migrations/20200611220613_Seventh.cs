using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class Seventh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Mascota");

            migrationBuilder.AddColumn<string>(
                name: "EstadoSituacion",
                table: "Mascota",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoSituacion",
                table: "Mascota");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Mascota",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
