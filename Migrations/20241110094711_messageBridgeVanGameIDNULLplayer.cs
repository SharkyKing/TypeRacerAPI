using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class messageBridgeVanGameIDNULLplayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "GameLog",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameLog_PlayerId",
                table: "GameLog",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameLog_Players_PlayerId",
                table: "GameLog",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameLog_Players_PlayerId",
                table: "GameLog");

            migrationBuilder.DropIndex(
                name: "IX_GameLog_PlayerId",
                table: "GameLog");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "GameLog");
        }
    }
}
