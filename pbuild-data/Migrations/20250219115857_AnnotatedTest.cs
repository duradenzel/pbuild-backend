using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pbuild_data.Migrations
{
    /// <inheritdoc />
    public partial class AnnotatedTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pokemons_Teams_TeamId",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "Type1",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "Type2",
                table: "Pokemons");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "Pokemons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pokemons_Teams_TeamId",
                table: "Pokemons",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pokemons_Teams_TeamId",
                table: "Pokemons");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "Pokemons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Type1",
                table: "Pokemons",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Type2",
                table: "Pokemons",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Pokemons_Teams_TeamId",
                table: "Pokemons",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
