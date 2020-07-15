using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContratoAdopcion_DetalleAdopcion",
                table: "ContratoAdopcion");

            migrationBuilder.DropForeignKey(
                name: "FK_Seguimiento_ReporteSeguimiento",
                table: "Seguimiento");

            migrationBuilder.DropTable(
                name: "DetalleAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_Seguimiento_IdReporteSeguimiento",
                table: "Seguimiento");

            migrationBuilder.DropIndex(
                name: "IX_ContratoAdopcion_IdDetalleAdopcion",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "IdReporteSeguimiento",
                table: "Seguimiento");

            migrationBuilder.DropColumn(
                name: "IdDetalleAdopcion",
                table: "ContratoAdopcion");

            migrationBuilder.AddColumn<int>(
                name: "IdSeguimiento",
                table: "ReporteSeguimiento",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdMascota",
                table: "ContratoAdopcion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "ContratoAdopcion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReporteSeguimiento_IdSeguimiento",
                table: "ReporteSeguimiento",
                column: "IdSeguimiento");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_IdMascota",
                table: "ContratoAdopcion",
                column: "IdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_IdUsuario",
                table: "ContratoAdopcion",
                column: "IdUsuario");

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
                name: "FK_ReporteSeguimiento_Seguimiento",
                table: "ReporteSeguimiento",
                column: "IdSeguimiento",
                principalTable: "Seguimiento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContratoAdopcion_Mascota",
                table: "ContratoAdopcion");

            migrationBuilder.DropForeignKey(
                name: "FK_ContratoAdopcion_Usuario",
                table: "ContratoAdopcion");

            migrationBuilder.DropForeignKey(
                name: "FK_ReporteSeguimiento_Seguimiento",
                table: "ReporteSeguimiento");

            migrationBuilder.DropIndex(
                name: "IX_ReporteSeguimiento_IdSeguimiento",
                table: "ReporteSeguimiento");

            migrationBuilder.DropIndex(
                name: "IX_ContratoAdopcion_IdMascota",
                table: "ContratoAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_ContratoAdopcion_IdUsuario",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "IdSeguimiento",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "IdMascota",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "ContratoAdopcion");

            migrationBuilder.AddColumn<int>(
                name: "IdReporteSeguimiento",
                table: "Seguimiento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdDetalleAdopcion",
                table: "ContratoAdopcion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DetalleAdopcion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdMascota = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleAdopcion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleAdopcion_Mascota",
                        column: x => x.IdMascota,
                        principalTable: "Mascota",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleAdopcion_User",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seguimiento_IdReporteSeguimiento",
                table: "Seguimiento",
                column: "IdReporteSeguimiento");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_IdDetalleAdopcion",
                table: "ContratoAdopcion",
                column: "IdDetalleAdopcion");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleAdopcion_IdMascota",
                table: "DetalleAdopcion",
                column: "IdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleAdopcion_IdUsuario",
                table: "DetalleAdopcion",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_ContratoAdopcion_DetalleAdopcion",
                table: "ContratoAdopcion",
                column: "IdDetalleAdopcion",
                principalTable: "DetalleAdopcion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seguimiento_ReporteSeguimiento",
                table: "Seguimiento",
                column: "IdReporteSeguimiento",
                principalTable: "ReporteSeguimiento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
