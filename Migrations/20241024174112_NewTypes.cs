using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GameTypeBase",
                keyColumn: "Id",
                keyValue: 2,
                column: "GameTypeName",
                value: "FluentType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GameTypeBase",
                keyColumn: "Id",
                keyValue: 2,
                column: "GameTypeName",
                value: "Endurance");
        }
    }
}
