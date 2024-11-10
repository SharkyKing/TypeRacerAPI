using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class playerinputenabledtimedwtfasdasdasdasdasdasdasdasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WordsStyleId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_WordsStyleId",
                table: "Players",
                column: "WordsStyleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_WordsStyle_WordsStyleId",
                table: "Players",
                column: "WordsStyleId",
                principalTable: "WordsStyle",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_WordsStyle_WordsStyleId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_WordsStyleId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "WordsStyleId",
                table: "Players");
        }
    }
}
