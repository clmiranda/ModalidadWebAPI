using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class ChangesSeguimiento_Reportes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mascota_Denuncia_DenunciaId",
                table: "Mascota");

            migrationBuilder.DropForeignKey(
                name: "FK_ReporteSeguimiento_Seguimiento_SeguimientoId",
                table: "ReporteSeguimiento");

            migrationBuilder.DropTable(
                name: "Notificacion");

            migrationBuilder.DropIndex(
                name: "IX_Mascota_DenunciaId",
                table: "Mascota");

            migrationBuilder.DropColumn(
                name: "CantidadVisitas",
                table: "Seguimiento");

            migrationBuilder.DropColumn(
                name: "FechaAsignada",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "FechaRealizada",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Mascota");

            migrationBuilder.AlterColumn<int>(
                name: "SeguimientoId",
                table: "ReporteSeguimiento",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "ReporteSeguimiento",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "DenunciaId",
                table: "Mascota",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Caracteristicas",
                table: "Mascota",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Especie",
                table: "Mascota",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RasgosParticulares",
                table: "Mascota",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReporteSeguimientoId",
                table: "Foto",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mascota_DenunciaId",
                table: "Mascota",
                column: "DenunciaId",
                unique: true,
                filter: "[DenunciaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Foto_ReporteSeguimientoId",
                table: "Foto",
                column: "ReporteSeguimientoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Foto_ReporteSeguimiento_ReporteSeguimientoId",
                table: "Foto",
                column: "ReporteSeguimientoId",
                principalTable: "ReporteSeguimiento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mascota_Denuncia_DenunciaId",
                table: "Mascota",
                column: "DenunciaId",
                principalTable: "Denuncia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReporteSeguimiento_Seguimiento_SeguimientoId",
                table: "ReporteSeguimiento",
                column: "SeguimientoId",
                principalTable: "Seguimiento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foto_ReporteSeguimiento_ReporteSeguimientoId",
                table: "Foto");

            migrationBuilder.DropForeignKey(
                name: "FK_Mascota_Denuncia_DenunciaId",
                table: "Mascota");

            migrationBuilder.DropForeignKey(
                name: "FK_ReporteSeguimiento_Seguimiento_SeguimientoId",
                table: "ReporteSeguimiento");

            migrationBuilder.DropIndex(
                name: "IX_Mascota_DenunciaId",
                table: "Mascota");

            migrationBuilder.DropIndex(
                name: "IX_Foto_ReporteSeguimientoId",
                table: "Foto");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "Caracteristicas",
                table: "Mascota");

            migrationBuilder.DropColumn(
                name: "Especie",
                table: "Mascota");

            migrationBuilder.DropColumn(
                name: "RasgosParticulares",
                table: "Mascota");

            migrationBuilder.DropColumn(
                name: "ReporteSeguimientoId",
                table: "Foto");

            migrationBuilder.AddColumn<int>(
                name: "CantidadVisitas",
                table: "Seguimiento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SeguimientoId",
                table: "ReporteSeguimiento",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAsignada",
                table: "ReporteSeguimiento",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRealizada",
                table: "ReporteSeguimiento",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "DenunciaId",
                table: "Mascota",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Mascota",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacion", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mascota_DenunciaId",
                table: "Mascota",
                column: "DenunciaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mascota_Denuncia_DenunciaId",
                table: "Mascota",
                column: "DenunciaId",
                principalTable: "Denuncia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReporteSeguimiento_Seguimiento_SeguimientoId",
                table: "ReporteSeguimiento",
                column: "SeguimientoId",
                principalTable: "Seguimiento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
