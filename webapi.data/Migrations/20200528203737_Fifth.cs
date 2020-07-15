using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class Fifth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CasoMascota_Denuncia",
                table: "CasoMascota");

            migrationBuilder.DropForeignKey(
                name: "FK_ContratoAdopcion_EstadoAdopcion",
                table: "ContratoAdopcion");

            migrationBuilder.DropForeignKey(
                name: "FK_ContratoAdopcion_Mascota",
                table: "ContratoAdopcion");

            migrationBuilder.DropForeignKey(
                name: "FK_ContratoAdopcion_Usuario",
                table: "ContratoAdopcion");

            migrationBuilder.DropForeignKey(
                name: "FK_Mascota_CasoMascota",
                table: "Mascota");

            migrationBuilder.DropForeignKey(
                name: "FK_Mascota_TipoMascota",
                table: "Mascota");

            migrationBuilder.DropForeignKey(
                name: "FK_ReporteSeguimiento_Seguimiento",
                table: "ReporteSeguimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_ReporteSeguimiento_User",
                table: "ReporteSeguimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_Seguimiento_ContratoAdopcion",
                table: "Seguimiento");

            migrationBuilder.DropTable(
                name: "TipoMascota");

            migrationBuilder.DropIndex(
                name: "IX_Seguimiento_IdContratoAdopcion",
                table: "Seguimiento");

            migrationBuilder.DropIndex(
                name: "IX_ReporteSeguimiento_IdSeguimiento",
                table: "ReporteSeguimiento");

            migrationBuilder.DropIndex(
                name: "IX_ReporteSeguimiento_IdUsuario",
                table: "ReporteSeguimiento");

            migrationBuilder.DropIndex(
                name: "IX_Mascota_IdCasoMascota",
                table: "Mascota");

            migrationBuilder.DropIndex(
                name: "IX_Mascota_IdTipoMascota",
                table: "Mascota");

            migrationBuilder.DropIndex(
                name: "IX_ContratoAdopcion_IdEstadoAdopcion",
                table: "ContratoAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_ContratoAdopcion_IdMascota",
                table: "ContratoAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_ContratoAdopcion_IdUsuario",
                table: "ContratoAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_CasoMascota_IdDenuncia",
                table: "CasoMascota");

            migrationBuilder.DropColumn(
                name: "IdContratoAdopcion",
                table: "Seguimiento");

            migrationBuilder.DropColumn(
                name: "IdSeguimiento",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "IdCasoMascota",
                table: "Mascota");

            migrationBuilder.DropColumn(
                name: "IdTipoMascota",
                table: "Mascota");

            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "Mascota");

            migrationBuilder.DropColumn(
                name: "IdEstadoAdopcion",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "IdMascota",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "IdDenuncia",
                table: "CasoMascota");

            migrationBuilder.AddColumn<int>(
                name: "ContratoAdopcionId",
                table: "Seguimiento",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeguimientoId",
                table: "ReporteSeguimiento",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ReporteSeguimiento",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CasoMascotaId",
                table: "Mascota",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "EstadoAdopcion",
                maxLength: 10000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstadoAdopcionId",
                table: "ContratoAdopcion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MascotaId",
                table: "ContratoAdopcion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ContratoAdopcion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DenunciaId",
                table: "CasoMascota",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Seguimiento_ContratoAdopcionId",
                table: "Seguimiento",
                column: "ContratoAdopcionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReporteSeguimiento_SeguimientoId",
                table: "ReporteSeguimiento",
                column: "SeguimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReporteSeguimiento_UserId",
                table: "ReporteSeguimiento",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Mascota_CasoMascotaId",
                table: "Mascota",
                column: "CasoMascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_EstadoAdopcionId",
                table: "ContratoAdopcion",
                column: "EstadoAdopcionId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_MascotaId",
                table: "ContratoAdopcion",
                column: "MascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_UserId",
                table: "ContratoAdopcion",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CasoMascota_DenunciaId",
                table: "CasoMascota",
                column: "DenunciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CasoMascota_Denuncia_DenunciaId",
                table: "CasoMascota",
                column: "DenunciaId",
                principalTable: "Denuncia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContratoAdopcion_EstadoAdopcion_EstadoAdopcionId",
                table: "ContratoAdopcion",
                column: "EstadoAdopcionId",
                principalTable: "EstadoAdopcion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContratoAdopcion_Mascota_MascotaId",
                table: "ContratoAdopcion",
                column: "MascotaId",
                principalTable: "Mascota",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContratoAdopcion_AspNetUsers_UserId",
                table: "ContratoAdopcion",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mascota_CasoMascota_CasoMascotaId",
                table: "Mascota",
                column: "CasoMascotaId",
                principalTable: "CasoMascota",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReporteSeguimiento_Seguimiento_SeguimientoId",
                table: "ReporteSeguimiento",
                column: "SeguimientoId",
                principalTable: "Seguimiento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReporteSeguimiento_AspNetUsers_UserId",
                table: "ReporteSeguimiento",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seguimiento_ContratoAdopcion_ContratoAdopcionId",
                table: "Seguimiento",
                column: "ContratoAdopcionId",
                principalTable: "ContratoAdopcion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CasoMascota_Denuncia_DenunciaId",
                table: "CasoMascota");

            migrationBuilder.DropForeignKey(
                name: "FK_ContratoAdopcion_EstadoAdopcion_EstadoAdopcionId",
                table: "ContratoAdopcion");

            migrationBuilder.DropForeignKey(
                name: "FK_ContratoAdopcion_Mascota_MascotaId",
                table: "ContratoAdopcion");

            migrationBuilder.DropForeignKey(
                name: "FK_ContratoAdopcion_AspNetUsers_UserId",
                table: "ContratoAdopcion");

            migrationBuilder.DropForeignKey(
                name: "FK_Mascota_CasoMascota_CasoMascotaId",
                table: "Mascota");

            migrationBuilder.DropForeignKey(
                name: "FK_ReporteSeguimiento_Seguimiento_SeguimientoId",
                table: "ReporteSeguimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_ReporteSeguimiento_AspNetUsers_UserId",
                table: "ReporteSeguimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_Seguimiento_ContratoAdopcion_ContratoAdopcionId",
                table: "Seguimiento");

            migrationBuilder.DropIndex(
                name: "IX_Seguimiento_ContratoAdopcionId",
                table: "Seguimiento");

            migrationBuilder.DropIndex(
                name: "IX_ReporteSeguimiento_SeguimientoId",
                table: "ReporteSeguimiento");

            migrationBuilder.DropIndex(
                name: "IX_ReporteSeguimiento_UserId",
                table: "ReporteSeguimiento");

            migrationBuilder.DropIndex(
                name: "IX_Mascota_CasoMascotaId",
                table: "Mascota");

            migrationBuilder.DropIndex(
                name: "IX_ContratoAdopcion_EstadoAdopcionId",
                table: "ContratoAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_ContratoAdopcion_MascotaId",
                table: "ContratoAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_ContratoAdopcion_UserId",
                table: "ContratoAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_CasoMascota_DenunciaId",
                table: "CasoMascota");

            migrationBuilder.DropColumn(
                name: "ContratoAdopcionId",
                table: "Seguimiento");

            migrationBuilder.DropColumn(
                name: "SeguimientoId",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "CasoMascotaId",
                table: "Mascota");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "EstadoAdopcion");

            migrationBuilder.DropColumn(
                name: "EstadoAdopcionId",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "MascotaId",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "DenunciaId",
                table: "CasoMascota");

            migrationBuilder.AddColumn<int>(
                name: "IdContratoAdopcion",
                table: "Seguimiento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdSeguimiento",
                table: "ReporteSeguimiento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "ReporteSeguimiento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdCasoMascota",
                table: "Mascota",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdTipoMascota",
                table: "Mascota",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen",
                table: "Mascota",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdEstadoAdopcion",
                table: "ContratoAdopcion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdMascota",
                table: "ContratoAdopcion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "ContratoAdopcion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdDenuncia",
                table: "CasoMascota",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TipoMascota",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMascota", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seguimiento_IdContratoAdopcion",
                table: "Seguimiento",
                column: "IdContratoAdopcion");

            migrationBuilder.CreateIndex(
                name: "IX_ReporteSeguimiento_IdSeguimiento",
                table: "ReporteSeguimiento",
                column: "IdSeguimiento");

            migrationBuilder.CreateIndex(
                name: "IX_ReporteSeguimiento_IdUsuario",
                table: "ReporteSeguimiento",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Mascota_IdCasoMascota",
                table: "Mascota",
                column: "IdCasoMascota");

            migrationBuilder.CreateIndex(
                name: "IX_Mascota_IdTipoMascota",
                table: "Mascota",
                column: "IdTipoMascota");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_IdEstadoAdopcion",
                table: "ContratoAdopcion",
                column: "IdEstadoAdopcion");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_IdMascota",
                table: "ContratoAdopcion",
                column: "IdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_IdUsuario",
                table: "ContratoAdopcion",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_CasoMascota_IdDenuncia",
                table: "CasoMascota",
                column: "IdDenuncia");

            migrationBuilder.AddForeignKey(
                name: "FK_CasoMascota_Denuncia",
                table: "CasoMascota",
                column: "IdDenuncia",
                principalTable: "Denuncia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContratoAdopcion_EstadoAdopcion",
                table: "ContratoAdopcion",
                column: "IdEstadoAdopcion",
                principalTable: "EstadoAdopcion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContratoAdopcion_Mascota",
                table: "ContratoAdopcion",
                column: "IdMascota",
                principalTable: "Mascota",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContratoAdopcion_Usuario",
                table: "ContratoAdopcion",
                column: "IdUsuario",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mascota_CasoMascota",
                table: "Mascota",
                column: "IdCasoMascota",
                principalTable: "CasoMascota",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mascota_TipoMascota",
                table: "Mascota",
                column: "IdTipoMascota",
                principalTable: "TipoMascota",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReporteSeguimiento_Seguimiento",
                table: "ReporteSeguimiento",
                column: "IdSeguimiento",
                principalTable: "Seguimiento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReporteSeguimiento_User",
                table: "ReporteSeguimiento",
                column: "IdUsuario",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seguimiento_ContratoAdopcion",
                table: "Seguimiento",
                column: "IdContratoAdopcion",
                principalTable: "ContratoAdopcion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
