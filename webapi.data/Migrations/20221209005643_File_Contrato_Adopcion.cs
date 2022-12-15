using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace webapi.data.Migrations
{
    public partial class File_Contrato_Adopcion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContratoAdopcion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(type: "text", nullable: true),
                    IdPublico = table.Column<string>(type: "text", nullable: true),
                    SolicitudAdopcionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratoAdopcion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContratoAdopcion_SolicitudAdopcion_SolicitudAdopcionId",
                        column: x => x.SolicitudAdopcionId,
                        principalTable: "SolicitudAdopcion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAdopcion_SolicitudAdopcionId",
                table: "ContratoAdopcion",
                column: "SolicitudAdopcionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContratoAdopcion");
        }
    }
}
