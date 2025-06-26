using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialGameConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GameConfigurations",
                columns: new[] { "Id", "BoardSizeHeight", "BoardSizeWidth", "CreatedBy", "MovableBoardHeight", "MovableBoardWidth", "Name", "OptionsAfterNMoves", "Player1Chips", "Player2Chips", "UserId", "WinCondition" },
                values: new object[,]
                {
                    { "big-game", 10, 10, null, 5, 5, "Big Game", 3, 6, 6, null, 3 },
                    { "classic", 5, 5, null, 3, 3, "Classical", 2, 4, 4, null, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameConfigurations",
                keyColumn: "Id",
                keyValue: "big-game");

            migrationBuilder.DeleteData(
                table: "GameConfigurations",
                keyColumn: "Id",
                keyValue: "classic");
        }
    }
}
