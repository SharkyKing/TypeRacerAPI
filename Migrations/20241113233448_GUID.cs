using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class GUID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SocketID",
                table: "Players",
                newName: "ConnectionGUID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConnectionGUID",
                table: "Players",
                newName: "SocketID");
        }
    }
}
