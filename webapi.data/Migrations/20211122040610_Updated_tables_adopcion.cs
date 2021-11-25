using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class Updated_tables_adopcion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seguimiento_ContratoAdopcion_ContratoAdopcionId",
                table: "Seguimiento");

            migrationBuilder.DropTable(
                name: "ContratoRechazo");

            migrationBuilder.DropTable(
                name: "ContratoAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_Seguimiento_ContratoAdopcionId",
                table: "Seguimiento");

            migrationBuilder.DropIndex(
                name: "IX_Foto_ReporteSeguimientoId",
                table: "Foto");

            migrationBuilder.DropColumn(
                name: "ContratoAdopcionId",
                table: "Seguimiento");

            migrationBuilder.DropColumn(
                name: "FechaConclusion",
                table: "Seguimiento");

            migrationBuilder.DropColumn(
                name: "EstadoHogarMascota",
                table: "ReporteSeguimiento");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFin",
                table: "Seguimiento",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SolicitudAdopcionId",
                table: "Seguimiento",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SolicitudAdopcion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(nullable: true),
                    Apellidos = table.Column<string>(nullable: true),
                    Ci = table.Column<string>(nullable: true),
                    Domicilio = table.Column<string>(nullable: true),
                    NumeroCelular = table.Column<string>(nullable: true),
                    Respuesta1 = table.Column<string>(nullable: true),
                    Respuesta2 = table.Column<string>(nullable: true),
                    Respuesta3 = table.Column<string>(nullable: true),
                    Respuesta4 = table.Column<string>(nullable: true),
                    Respuesta5 = table.Column<string>(nullable: true),
                    Respuesta6 = table.Column<string>(nullable: true),
                    Respuesta7 = table.Column<string>(nullable: true),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaSolicitudAdopcion = table.Column<DateTime>(nullable: false),
                    FechaAdopcion = table.Column<DateTime>(nullable: false),
                    Estado = table.Column<string>(nullable: true),
                    MascotaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudAdopcion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudAdopcion_Mascota_MascotaId",
                        column: x => x.MascotaId,
                        principalTable: "Mascota",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdopcionCancelada",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Razon = table.Column<string>(nullable: true),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    SolicitudAdopcionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdopcionCancelada", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdopcionCancelada_SolicitudAdopcion_SolicitudAdopcionId",
                        column: x => x.SolicitudAdopcionId,
                        principalTable: "SolicitudAdopcion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdopcionRechazada",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Razon = table.Column<string>(nullable: true),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    SolicitudAdopcionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdopcionRechazada", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdopcionRechazada_SolicitudAdopcion_SolicitudAdopcionId",
                        column: x => x.SolicitudAdopcionId,
                        principalTable: "SolicitudAdopcion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seguimiento_SolicitudAdopcionId",
                table: "Seguimiento",
                column: "SolicitudAdopcionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Foto_ReporteSeguimientoId",
                table: "Foto",
                column: "ReporteSeguimientoId",
                unique: true,
                filter: "[ReporteSeguimientoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdopcionCancelada_SolicitudAdopcionId",
                table: "AdopcionCancelada",
                column: "SolicitudAdopcionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdopcionRechazada_SolicitudAdopcionId",
                table: "AdopcionRechazada",
                column: "SolicitudAdopcionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudAdopcion_MascotaId",
                table: "SolicitudAdopcion",
                column: "MascotaId",
                unique: true,
                filter: "[MascotaId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Seguimiento_SolicitudAdopcion_SolicitudAdopcionId",
                table: "Seguimiento",
                column: "SolicitudAdopcionId",
                principalTable: "SolicitudAdopcion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seguimiento_SolicitudAdopcion_SolicitudAdopcionId",
                table: "Seguimiento");

            migrationBuilder.DropTable(
                name: "AdopcionCancelada");

            migrationBuilder.DropTable(
                name: "AdopcionRechazada");

            migrationBuilder.DropTable(
                name: "SolicitudAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_Seguimiento_SolicitudAdopcionId",
                table: "Seguimiento");

            migrationBuilder.DropIndex(
                name: "IX_Foto_ReporteSeguimientoId",
                table: "Foto");

            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "Seguimiento");

            migrationBuilder.DropColumn(
                name: "SolicitudAdopcionId",
                table: "Seguimiento");

            migrationBuilder.AddColumn<int>(
                name: "ContratoAdopcionId",
                table: "Seguimiento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaConclusion",
                table: "Seguimiento",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EstadoHogarMascota",
                table: "ReporteSeguimiento",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContratoAdopcion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ci = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Domicilio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaAdopcion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaSolicitudAdopcion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MascotaId = table.Column<int>(type: "int", nullable: true),
                    Nombres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroCelular = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Respuesta1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Respuesta2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Respuesta3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Respuesta4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Respuesta5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Respuesta6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Respuesta7 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratoAdopcion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContratoAdopcion_Mascota_MascotaId",
                        column: x => x.MascotaId,
                        principalTable: "Mascota",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContratoRechazo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContratoAdopcionId = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RazonRechazo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratoRechazo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContratoRechazo_ContratoAdopcion_ContratoAdopcionId",
                        column: x => x.ContratoAdopcionId,
                        principalTable: "ContratoAdopcion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seguimiento_ContratoAdopcionId",
                table: "Seguimiento",
                column: "ContratoAdopcionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Foto_ReporteSeguimientoId",
                table: "Foto",
                column: "ReporteSeguimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_MascotaId",
                table: "ContratoAdopcion",
                column: "MascotaId",
                unique: true,
                filter: "[MascotaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoRechazo_ContratoAdopcionId",
                table: "ContratoRechazo",
                column: "ContratoAdopcionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Seguimiento_ContratoAdopcion_ContratoAdopcionId",
                table: "Seguimiento",
                column: "ContratoAdopcionId",
                principalTable: "ContratoAdopcion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
