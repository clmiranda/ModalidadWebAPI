using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class Tenth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Domicilio",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "Edad",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "NombreCompleto",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "NumeroCelular",
                table: "ContratoAdopcion");

            migrationBuilder.AddColumn<string>(
                name: "Domicilio",
                table: "AspNetUsers",
                maxLength: 3000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Edad",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroCelular",
                table: "AspNetUsers",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Domicilio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Edad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NumeroCelular",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Domicilio",
                table: "ContratoAdopcion",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Edad",
                table: "ContratoAdopcion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreCompleto",
                table: "ContratoAdopcion",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroCelular",
                table: "ContratoAdopcion",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
