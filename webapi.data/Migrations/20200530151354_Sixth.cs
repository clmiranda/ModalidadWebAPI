using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class Sixth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Edad",
                table: "Mascota",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Esterilizado",
                table: "Mascota",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAgregado",
                table: "Mascota",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Tamaño",
                table: "Mascota",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Edad",
                table: "Mascota");

            migrationBuilder.DropColumn(
                name: "Esterilizado",
                table: "Mascota");

            migrationBuilder.DropColumn(
                name: "FechaAgregado",
                table: "Mascota");

            migrationBuilder.DropColumn(
                name: "Tamaño",
                table: "Mascota");
        }
    }
}
