using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_GameConfigurations_GameConfigurationId",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_GameSessions_GameConfigurationId",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "GameConfigurationId",
                table: "GameSessions");

            migrationBuilder.AlterColumn<string>(
                name: "GameConfigId",
                table: "GameSessions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "GameConfigurations",
                columns: new[] { "Id", "BoardSizeHeight", "BoardSizeWidth", "CreatedBy", "MovableBoardHeight", "MovableBoardWidth", "Name", "OptionsAfterNMoves", "Player1Chips", "Player2Chips", "UserId", "WinCondition" },
                values: new object[,]
                {
                    { "big-game", 10, 10, null, 5, 5, "Big Game", 3, 6, 6, null, 3 },
                    { "classic", 5, 5, null, 3, 3, "Classical", 2, 4, 4, null, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_GameConfigId",
                table: "GameSessions",
                column: "GameConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_GameConfigurations_GameConfigId",
                table: "GameSessions",
                column: "GameConfigId",
                principalTable: "GameConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_GameConfigurations_GameConfigId",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_GameSessions_GameConfigId",
                table: "GameSessions");

            migrationBuilder.DeleteData(
                table: "GameConfigurations",
                keyColumn: "Id",
                keyValue: "big-game");

            migrationBuilder.DeleteData(
                table: "GameConfigurations",
                keyColumn: "Id",
                keyValue: "classic");

            migrationBuilder.AlterColumn<string>(
                name: "GameConfigId",
                table: "GameSessions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "GameConfigurationId",
                table: "GameSessions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_GameConfigurationId",
                table: "GameSessions",
                column: "GameConfigurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_GameConfigurations_GameConfigurationId",
                table: "GameSessions",
                column: "GameConfigurationId",
                principalTable: "GameConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
