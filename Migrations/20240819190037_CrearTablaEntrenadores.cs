using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeAPI.Migrations
{
    public partial class CrearTablaEntrenadores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Entrenadores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PokemonId",
                table: "Entrenadores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RutaImagen",
                table: "Entrenadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TipoEspecialidad",
                table: "Entrenadores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Entrenadores_PokemonId",
                table: "Entrenadores",
                column: "PokemonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entrenadores_Pokemons_PokemonId",
                table: "Entrenadores",
                column: "PokemonId",
                principalTable: "Pokemons",
                principalColumn: "PokemonId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entrenadores_Pokemons_PokemonId",
                table: "Entrenadores");

            migrationBuilder.DropIndex(
                name: "IX_Entrenadores_PokemonId",
                table: "Entrenadores");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Entrenadores");

            migrationBuilder.DropColumn(
                name: "PokemonId",
                table: "Entrenadores");

            migrationBuilder.DropColumn(
                name: "RutaImagen",
                table: "Entrenadores");

            migrationBuilder.DropColumn(
                name: "TipoEspecialidad",
                table: "Entrenadores");
        }
    }
}
