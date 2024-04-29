using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalAPIPeliculas.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Comentarios_ComentarioId",
                table: "Comentarios");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_ComentarioId",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "ComentarioId",
                table: "Comentarios");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_PeliculaId",
                table: "Comentarios",
                column: "PeliculaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Peliculas_PeliculaId",
                table: "Comentarios",
                column: "PeliculaId",
                principalTable: "Peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Peliculas_PeliculaId",
                table: "Comentarios");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_PeliculaId",
                table: "Comentarios");

            migrationBuilder.AddColumn<int>(
                name: "ComentarioId",
                table: "Comentarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_ComentarioId",
                table: "Comentarios",
                column: "ComentarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Comentarios_ComentarioId",
                table: "Comentarios",
                column: "ComentarioId",
                principalTable: "Comentarios",
                principalColumn: "Id");
        }
    }
}
