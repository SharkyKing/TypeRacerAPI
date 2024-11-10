using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class playerinputenabledtimedwtfasdasdasdasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordsStyle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StyleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fontStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fontWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fontFamily = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordsStyle", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "WordsStyle",
                columns: new[] { "Id", "StyleName", "fontFamily", "fontStyle", "fontWeight" },
                values: new object[,]
                {
                    { 1, "bold", "Arial, sans-serif", null, "bold" },
                    { 2, "Rewind", "Georgia, serif", null, null },
                    { 3, "Invisible", "Courier New, monospace", "normal", "normal" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordsStyle");
        }
    }
}
