using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class Third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContratoAdopcion_AspNetUsers_UserId",
                table: "ContratoAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_ContratoAdopcion_UserId",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "CasaPropia",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "RazonAdopcion",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "TerminosCondiciones",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ContratoAdopcion");

            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "ContratoAdopcion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ci",
                table: "ContratoAdopcion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Domicilio",
                table: "ContratoAdopcion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombres",
                table: "ContratoAdopcion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroCelular",
                table: "ContratoAdopcion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pregunta1",
                table: "ContratoAdopcion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pregunta2",
                table: "ContratoAdopcion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pregunta3",
                table: "ContratoAdopcion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pregunta4",
                table: "ContratoAdopcion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pregunta5",
                table: "ContratoAdopcion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pregunta6",
                table: "ContratoAdopcion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pregunta7",
                table: "ContratoAdopcion",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "Ci",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "Domicilio",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "Nombres",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "NumeroCelular",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "Pregunta1",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "Pregunta2",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "Pregunta3",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "Pregunta4",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "Pregunta5",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "Pregunta6",
                table: "ContratoAdopcion");

            migrationBuilder.DropColumn(
                name: "Pregunta7",
                table: "ContratoAdopcion");

            migrationBuilder.AddColumn<string>(
                name: "CasaPropia",
                table: "ContratoAdopcion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RazonAdopcion",
                table: "ContratoAdopcion",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TerminosCondiciones",
                table: "ContratoAdopcion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ContratoAdopcion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_UserId",
                table: "ContratoAdopcion",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContratoAdopcion_AspNetUsers_UserId",
                table: "ContratoAdopcion",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
