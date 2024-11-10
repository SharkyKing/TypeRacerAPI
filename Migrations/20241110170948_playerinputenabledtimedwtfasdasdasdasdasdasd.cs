using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class playerinputenabledtimedwtfasdasdasdasdasdasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WordsStyle",
                keyColumn: "Id",
                keyValue: 1,
                column: "StyleName",
                value: "BoldDecorator");

            migrationBuilder.UpdateData(
                table: "WordsStyle",
                keyColumn: "Id",
                keyValue: 2,
                column: "StyleName",
                value: "ItalicDecorator");

            migrationBuilder.UpdateData(
                table: "WordsStyle",
                keyColumn: "Id",
                keyValue: 3,
                column: "StyleName",
                value: "FancyFontDecorator");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WordsStyle",
                keyColumn: "Id",
                keyValue: 1,
                column: "StyleName",
                value: "bold");

            migrationBuilder.UpdateData(
                table: "WordsStyle",
                keyColumn: "Id",
                keyValue: 2,
                column: "StyleName",
                value: "Rewind");

            migrationBuilder.UpdateData(
                table: "WordsStyle",
                keyColumn: "Id",
                keyValue: 3,
                column: "StyleName",
                value: "Invisible");
        }
    }
}
