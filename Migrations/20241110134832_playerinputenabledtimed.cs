using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class playerinputenabledtimed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsOneTimeUse",
                table: "PlayerPower",
                newName: "IsTimedPower");

            migrationBuilder.UpdateData(
                table: "PlayerPower",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsTimedPower",
                value: true);

            migrationBuilder.UpdateData(
                table: "PlayerPower",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsTimedPower",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsTimedPower",
                table: "PlayerPower",
                newName: "IsOneTimeUse");

            migrationBuilder.UpdateData(
                table: "PlayerPower",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsOneTimeUse",
                value: false);

            migrationBuilder.UpdateData(
                table: "PlayerPower",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsOneTimeUse",
                value: false);
        }
    }
}
