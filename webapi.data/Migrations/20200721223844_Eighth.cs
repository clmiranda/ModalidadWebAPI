using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class Eighth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ContratoAdopcion_EstadoAdopcion_EstadoAdopcionId",
            //    table: "ContratoAdopcion");

            migrationBuilder.DropForeignKey(
                name: "FK_Foto_AspNetUsers_UserId",
                table: "Foto");

            migrationBuilder.DropForeignKey(
                name: "FK_ReporteSeguimiento_AspNetUsers_UserId",
                table: "ReporteSeguimiento");

            //migrationBuilder.DropTable(
            //    name: "EstadoAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_Foto_UserId",
                table: "Foto");

            migrationBuilder.DropIndex(
                name: "IX_ContratoAdopcion_Ci",
                table: "ContratoAdopcion");

            //migrationBuilder.DropIndex(
            //    name: "IX_ContratoAdopcion_EstadoAdopcionId",
            //    table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Foto");

            migrationBuilder.DropColumn(
                name: "Ci",
                table: "ContratoAdopcion");

            //migrationBuilder.DropColumn(
            //    name: "EstadoAdopcionId",
            //    table: "ContratoAdopcion");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ReporteSeguimiento",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Mascota",
                maxLength: 3000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Edad",
                table: "ContratoAdopcion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "ContratoAdopcion",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaSolicitudAdopcion",
                table: "ContratoAdopcion",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RazonAdopcion",
                table: "ContratoAdopcion",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TerminosCondiciones",
                table: "ContratoAdopcion",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ContratoRechazo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RazonRechazo = table.Column<string>(maxLength: 300, nullable: true),
                    ContratoAdopcionId = table.Column<int>(nullable: false)
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
                name: "IX_ContratoRechazo_ContratoAdopcionId",
                table: "ContratoRechazo",
                column: "ContratoAdopcionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReporteSeguimiento_AspNetUsers_UserId",
                table: "ReporteSeguimiento",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReporteSeguimiento_AspNetUsers_UserId",
                table: "ReporteSeguimiento");

            migrationBuilder.DropTable(
                name: "ContratoRechazo");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Mascota");

            migrationBuilder.DropColumn(
                name: "Edad",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "FechaSolicitudAdopcion",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "RazonAdopcion",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "TerminosCondiciones",
                table: "ContratoAdopcion");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ReporteSeguimiento",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Foto",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ci",
                table: "ContratoAdopcion",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "EstadoAdopcionId",
            //    table: "ContratoAdopcion",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateTable(
            //    name: "EstadoAdopcion",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Descripcion = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
            //        Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_EstadoAdopcion", x => x.Id);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_Foto_UserId",
                table: "Foto",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_Ci",
                table: "ContratoAdopcion",
                column: "Ci",
                unique: true,
                filter: "[Ci] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContratoAdopcion_EstadoAdopcionId",
            //    table: "ContratoAdopcion",
            //    column: "EstadoAdopcionId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ContratoAdopcion_EstadoAdopcion_EstadoAdopcionId",
            //    table: "ContratoAdopcion",
            //    column: "EstadoAdopcionId",
            //    principalTable: "EstadoAdopcion",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Foto_AspNetUsers_UserId",
                table: "Foto",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReporteSeguimiento_AspNetUsers_UserId",
                table: "ReporteSeguimiento",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
