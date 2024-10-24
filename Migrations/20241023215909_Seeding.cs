using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GameLevelBase",
                columns: new[] { "Id", "GameLevelName" },
                values: new object[,]
                {
                    { 1, "Beginner" },
                    { 2, "Normal" },
                    { 3, "Advanced" }
                });

            migrationBuilder.InsertData(
                table: "GameTypeBase",
                columns: new[] { "Id", "GameTypeName" },
                values: new object[,]
                {
                    { 1, "TimeAttack" },
                    { 2, "Endurance" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameLevelBase",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GameLevelBase",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GameLevelBase",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GameTypeBase",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GameTypeBase",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
