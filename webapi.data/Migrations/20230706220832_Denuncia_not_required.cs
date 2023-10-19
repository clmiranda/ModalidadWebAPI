using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class Denuncia_not_required : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mascota_Denuncia_DenunciaId",
                table: "Mascota");

            migrationBuilder.AlterColumn<int>(
                name: "DenunciaId",
                table: "Mascota",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Mascota_Denuncia_DenunciaId",
                table: "Mascota",
                column: "DenunciaId",
                principalTable: "Denuncia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mascota_Denuncia_DenunciaId",
                table: "Mascota");

            migrationBuilder.AlterColumn<int>(
                name: "DenunciaId",
                table: "Mascota",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mascota_Denuncia_DenunciaId",
                table: "Mascota",
                column: "DenunciaId",
                principalTable: "Denuncia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
