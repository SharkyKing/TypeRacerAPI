using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class addingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Words",
                table: "Words",
                newName: "Text");

            migrationBuilder.AddColumn<bool>(
                name: "IsSpectator",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSpectator",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Words",
                newName: "Words");
        }
    }
}
