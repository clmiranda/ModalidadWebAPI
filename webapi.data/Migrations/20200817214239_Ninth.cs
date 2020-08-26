using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.data.Migrations
{
    public partial class Ninth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReporteSeguimiento_AspNetUsers_UserId",
                table: "ReporteSeguimiento");

            migrationBuilder.DropIndex(
                name: "IX_ReporteSeguimiento_UserId",
                table: "ReporteSeguimiento");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReporteSeguimiento");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Seguimiento",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seguimiento_UserId",
                table: "Seguimiento",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seguimiento_AspNetUsers_UserId",
                table: "Seguimiento",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seguimiento_AspNetUsers_UserId",
                table: "Seguimiento");

            migrationBuilder.DropIndex(
                name: "IX_Seguimiento_UserId",
                table: "Seguimiento");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Seguimiento");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ReporteSeguimiento",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReporteSeguimiento_UserId",
                table: "ReporteSeguimiento",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReporteSeguimiento_AspNetUsers_UserId",
                table: "ReporteSeguimiento",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
