using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGameAndPlayerRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameLevelId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GameTypeId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GameLevelBase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameLevelName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLevelBase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameTypeBase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTypeBase", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameLevelId",
                table: "Games",
                column: "GameLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameTypeId",
                table: "Games",
                column: "GameTypeId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameLevelBase_GameLevelId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameTypeBase_GameTypeId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "GameLevelBase");

            migrationBuilder.DropTable(
                name: "GameTypeBase");

            migrationBuilder.DropIndex(
                name: "IX_Games_GameLevelId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_GameTypeId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameLevelId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameTypeId",
                table: "Games");
        }
    }
}
