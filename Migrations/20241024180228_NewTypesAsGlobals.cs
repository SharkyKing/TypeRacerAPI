using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewTypesAsGlobals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameLevelBase_GameLevelId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameTypeBase_GameTypeId",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameTypeBase",
                table: "GameTypeBase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameLevelBase",
                table: "GameLevelBase");

            migrationBuilder.RenameTable(
                name: "GameTypeBase",
                newName: "GameLevel");

            migrationBuilder.RenameTable(
                name: "GameLevelBase",
                newName: "GameType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameLevel",
                table: "GameLevel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameType",
                table: "GameType",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameLevel_GameTypeId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameType_GameLevelId",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameType",
                table: "GameType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameLevel",
                table: "GameLevel");

            migrationBuilder.RenameTable(
                name: "GameType",
                newName: "GameLevelBase");

            migrationBuilder.RenameTable(
                name: "GameLevel",
                newName: "GameTypeBase");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameLevelBase",
                table: "GameLevelBase",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameTypeBase",
                table: "GameTypeBase",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameLevelBase_GameLevelId",
                table: "Games",
                column: "GameLevelId",
                principalTable: "GameLevelBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameTypeBase_GameTypeId",
                table: "Games",
                column: "GameTypeId",
                principalTable: "GameTypeBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
