using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class Tables_ReporteTratamiento_SolicitudAdopcion_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Respuesta1",
                table: "SolicitudAdopcion");

            migrationBuilder.DropColumn(
                name: "Respuesta2",
                table: "SolicitudAdopcion");

            migrationBuilder.DropColumn(
                name: "Respuesta3",
                table: "SolicitudAdopcion");

            migrationBuilder.DropColumn(
                name: "Respuesta4",
                table: "SolicitudAdopcion");

            migrationBuilder.DropColumn(
                name: "Respuesta5",
                table: "SolicitudAdopcion");

            migrationBuilder.DropColumn(
                name: "Respuesta6",
                table: "SolicitudAdopcion");

            migrationBuilder.DropColumn(
                name: "Respuesta7",
                table: "SolicitudAdopcion");

            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "ReporteTratamiento");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Respuesta1",
                table: "SolicitudAdopcion",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Respuesta2",
                table: "SolicitudAdopcion",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Respuesta3",
                table: "SolicitudAdopcion",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Respuesta4",
                table: "SolicitudAdopcion",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Respuesta5",
                table: "SolicitudAdopcion",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Respuesta6",
                table: "SolicitudAdopcion",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Respuesta7",
                table: "SolicitudAdopcion",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "ReporteTratamiento",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
