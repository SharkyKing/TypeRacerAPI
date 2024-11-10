using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class messageBridgeVanGameIDNULLplayeridk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "GameLog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LogTypeId",
                table: "GameLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LogType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "LogType",
                columns: new[] { "Id", "LogTypeName" },
                values: new object[,]
                {
                    { 1, "Error" },
                    { 2, "GameLog" },
                    { 3, "Information" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameLog_LogTypeId",
                table: "GameLog",
                column: "LogTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameLog_LogType_LogTypeId",
                table: "GameLog",
                column: "LogTypeId",
                principalTable: "LogType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameLog_LogType_LogTypeId",
                table: "GameLog");

            migrationBuilder.DropTable(
                name: "LogType");

            migrationBuilder.DropIndex(
                name: "IX_GameLog_LogTypeId",
                table: "GameLog");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "GameLog");

            migrationBuilder.DropColumn(
                name: "LogTypeId",
                table: "GameLog");
        }
    }
}
