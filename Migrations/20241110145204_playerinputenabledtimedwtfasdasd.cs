using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class playerinputenabledtimedwtfasdasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlayerPower",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsTimedPower",
                value: false);

            migrationBuilder.UpdateData(
                table: "PlayerPower",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsTimedPower",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlayerPower",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsTimedPower",
                value: true);

            migrationBuilder.UpdateData(
                table: "PlayerPower",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsTimedPower",
                value: false);
        }
    }
}
