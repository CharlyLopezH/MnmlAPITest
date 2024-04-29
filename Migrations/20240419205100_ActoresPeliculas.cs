using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalAPIPeliculas.Migrations
{
    /// <inheritdoc />
    public partial class ActoresPeliculas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameTable(
                name: "GenerosPeliculas",
                newName: "GenerosPeliculas");


            migrationBuilder.CreateTable(
                name: "ActoresPeliculas",
                columns: table => new
                {
                    ActorId = table.Column<int>(type: "int", nullable: false),
                    PeliculaId = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Personaje = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActoresPeliculas", x => new { x.PeliculaId, x.ActorId });
                    table.ForeignKey(
                        name: "FK_ActoresPeliculas_Actores_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActoresPeliculas_Peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActoresPeliculas_ActorId",
                table: "ActoresPeliculas",
                column: "ActorId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenerosPeliculas_Generos_GeneroId",
                table: "GenerosPeliculas");

            migrationBuilder.DropForeignKey(
                name: "FK_GenerosPeliculas_Peliculas_PeliculaId",
                table: "GenerosPeliculas");

            migrationBuilder.DropTable(
                name: "ActoresPeliculas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenerosPeliculas",
                table: "GenerosPeliculas");

            migrationBuilder.RenameTable(
                name: "GenerosPeliculas",
                newName: "GenerosPeliculas");

            migrationBuilder.RenameIndex(
                name: "IX_GenerosPeliculas_PeliculaId",
                table: "GenerosPeliculas",
                newName: "IX_GeneroPeliculas_PeliculaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenerosPeliculas",
                table: "GenerosPeliculas",
                columns: new[] { "GeneroId", "PeliculaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GenerosPeliculas_Generos_GeneroId",
                table: "GenerosPeliculas",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GenerosPeliculas_Peliculas_PeliculaId",
                table: "GenerosPeliculas",
                column: "PeliculaId",
                principalTable: "Peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
