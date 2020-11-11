using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContratoAdopcion_MascotaId",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "FechaReporte",
                table: "ReporteSeguimiento");

            migrationBuilder.AddColumn<string>(
                name: "EstadoHogarMascota",
                table: "ReporteSeguimiento",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoMascota",
                table: "ReporteSeguimiento",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAsignada",
                table: "ReporteSeguimiento",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRealizada",
                table: "ReporteSeguimiento",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "ReporteSeguimiento",
                maxLength: 10000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CasaPropia",
                table: "ContratoAdopcion",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_MascotaId",
                table: "ContratoAdopcion",
                column: "MascotaId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContratoAdopcion_MascotaId",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "EstadoHogarMascota",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "EstadoMascota",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "FechaAsignada",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "FechaRealizada",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "CasaPropia",
                table: "ContratoAdopcion");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "ReporteSeguimiento",
                type: "nvarchar(max)",
                maxLength: 10000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaReporte",
                table: "ReporteSeguimiento",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_MascotaId",
                table: "ContratoAdopcion",
                column: "MascotaId");
        }
    }
}
