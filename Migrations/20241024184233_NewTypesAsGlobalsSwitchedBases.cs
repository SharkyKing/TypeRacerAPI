using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewTypesAsGlobalsSwitchedBases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameLevel_GameTypeId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameType_GameLevelId",
                table: "Games");

            migrationBuilder.DeleteData(
                table: "GameType",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "GameLevelName",
                table: "GameType",
                newName: "GameTypeName");

            migrationBuilder.RenameColumn(
                name: "GameTypeName",
                table: "GameLevel",
                newName: "GameLevelName");

            migrationBuilder.UpdateData(
                table: "GameLevel",
                keyColumn: "Id",
                keyValue: 1,
                column: "GameLevelName",
                value: "Beginner");

            migrationBuilder.UpdateData(
                table: "GameLevel",
                keyColumn: "Id",
                keyValue: 2,
                column: "GameLevelName",
                value: "Normal");

            migrationBuilder.InsertData(
                table: "GameLevel",
                columns: new[] { "Id", "GameLevelName" },
                values: new object[] { 3, "Advanced" });

            migrationBuilder.UpdateData(
                table: "GameType",
                keyColumn: "Id",
                keyValue: 1,
                column: "GameTypeName",
                value: "TimeAttack");

            migrationBuilder.UpdateData(
                table: "GameType",
                keyColumn: "Id",
                keyValue: 2,
                column: "GameTypeName",
                value: "FluentType");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameLevel_GameLevelId",
                table: "Games",
                column: "GameLevelId",
                principalTable: "GameLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameType_GameTypeId",
                table: "Games",
                column: "GameTypeId",
                principalTable: "GameType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameLevel_GameLevelId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameType_GameTypeId",
                table: "Games");

            migrationBuilder.DeleteData(
                table: "GameLevel",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "GameTypeName",
                table: "GameType",
                newName: "GameLevelName");

            migrationBuilder.RenameColumn(
                name: "GameLevelName",
                table: "GameLevel",
                newName: "GameTypeName");

            migrationBuilder.UpdateData(
                table: "GameLevel",
                keyColumn: "Id",
                keyValue: 1,
                column: "GameTypeName",
                value: "TimeAttack");

            migrationBuilder.UpdateData(
                table: "GameLevel",
                keyColumn: "Id",
                keyValue: 2,
                column: "GameTypeName",
                value: "FluentType");

            migrationBuilder.UpdateData(
                table: "GameType",
                keyColumn: "Id",
                keyValue: 1,
                column: "GameLevelName",
                value: "Beginner");

            migrationBuilder.UpdateData(
                table: "GameType",
                keyColumn: "Id",
                keyValue: 2,
                column: "GameLevelName",
                value: "Normal");

            migrationBuilder.InsertData(
                table: "GameType",
                columns: new[] { "Id", "GameLevelName" },
                values: new object[] { 3, "Advanced" });

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameLevel_GameTypeId",
                table: "Games",
                column: "GameTypeId",
                principalTable: "GameLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameType_GameLevelId",
                table: "Games",
                column: "GameLevelId",
                principalTable: "GameType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
