using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class Third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "Denuncia",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "CasoMascota",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "Denuncia");

            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "CasoMascota");
        }
    }
}
