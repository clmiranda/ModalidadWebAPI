using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class Change_ReporteSeguimiento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Foto_ReporteSeguimientoId",
                table: "Foto");

            migrationBuilder.DropColumn(
                name: "EstadoHogarMascota",
                table: "ReporteSeguimiento");

            migrationBuilder.CreateIndex(
                name: "IX_Foto_ReporteSeguimientoId",
                table: "Foto",
                column: "ReporteSeguimientoId",
                unique: true,
                filter: "[ReporteSeguimientoId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Foto_ReporteSeguimientoId",
                table: "Foto");

            migrationBuilder.AddColumn<string>(
                name: "EstadoHogarMascota",
                table: "ReporteSeguimiento",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Foto_ReporteSeguimientoId",
                table: "Foto",
                column: "ReporteSeguimientoId");
        }
    }
}
