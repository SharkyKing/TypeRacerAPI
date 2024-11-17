using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class dadaaasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerGameResultType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GifUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGameResultType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PlayerGameResultType",
                columns: new[] { "Id", "GifUrl", "Text", "Title" },
                values: new object[,]
                {
                    { 1, "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExOGkweWlweTBuanJjeWN0d2xna3R2YzJ0YWVoZTRkNmZhMTV5MjZrayZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/xT0GqssRweIhlz209i/giphy.gif", "Congratulations!", "You WON!" },
                    { 2, "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExNnBpM3dvYjgyYWdhaXJ0dzk3M2NkY3U3NzVzdzExamd6N2VkYTYweiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/B4uP3h97Hi2UaqS0E3/giphy.gif", "Better luck next time", "You lost :(" },
                    { 3, "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExNnBpM3dvYjgyYWdhaXJ0dzk3M2NkY3U3NzVzdzExamd6N2VkYTYweiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/B4uP3h97Hi2UaqS0E3/giphy.gif", "Be faster next time!", "Nobody won this game" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerGameResultType");
        }
    }
}
