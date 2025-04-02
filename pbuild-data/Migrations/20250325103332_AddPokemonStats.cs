using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pbuild_data.Migrations
{
    /// <inheritdoc />
    public partial class AddPokemonStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Attack",
                table: "Pokemons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Defense",
                table: "Pokemons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HP",
                table: "Pokemons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SP_Attack",
                table: "Pokemons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SP_Defense",
                table: "Pokemons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Speed",
                table: "Pokemons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attack",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "Defense",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "HP",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "SP_Attack",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "SP_Defense",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Pokemons");
        }
    }
}
