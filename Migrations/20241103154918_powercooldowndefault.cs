using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class powercooldowndefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlayerPower",
                keyColumn: "Id",
                keyValue: 1,
                column: "CooldownTime",
                value: 10);

            migrationBuilder.UpdateData(
                table: "PlayerPower",
                keyColumn: "Id",
                keyValue: 2,
                column: "CooldownTime",
                value: 5);

            migrationBuilder.UpdateData(
                table: "PlayerPower",
                keyColumn: "Id",
                keyValue: 3,
                column: "CooldownTime",
                value: 15);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlayerPower",
                keyColumn: "Id",
                keyValue: 1,
                column: "CooldownTime",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PlayerPower",
                keyColumn: "Id",
                keyValue: 2,
                column: "CooldownTime",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PlayerPower",
                keyColumn: "Id",
                keyValue: 3,
                column: "CooldownTime",
                value: 0);
        }
    }
}
