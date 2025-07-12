using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_17 : Migration
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
                name: "PlayersMovesJson",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "GameConfigurationId",
                table: "GameSessions");

            migrationBuilder.RenameColumn(
                name: "Player2Options",
                table: "GameStates",
                newName: "Player2Abilities");

            migrationBuilder.RenameColumn(
                name: "Player1Options",
                table: "GameStates",
                newName: "Player1Abilities");

            migrationBuilder.RenameColumn(
                name: "OptionsAfterNMoves",
                table: "GameConfigurations",
                newName: "AbilitiesAfterNMoves");

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
                columns: new[] { "Id", "AbilitiesAfterNMoves", "BoardSizeHeight", "BoardSizeWidth", "CreatedBy", "MovableBoardHeight", "MovableBoardWidth", "Name", "Player1Chips", "Player2Chips", "UserId", "WinCondition" },
                values: new object[,]
                {
                    { "big-game", 3, 10, 10, null, 5, 5, "Big Game", 6, 6, null, 3 },
                    { "classic", 2, 5, 5, null, 3, 3, "Classical", 4, 4, null, 3 }
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

            migrationBuilder.RenameColumn(
                name: "Player2Abilities",
                table: "GameStates",
                newName: "Player2Options");

            migrationBuilder.RenameColumn(
                name: "Player1Abilities",
                table: "GameStates",
                newName: "Player1Options");

            migrationBuilder.RenameColumn(
                name: "AbilitiesAfterNMoves",
                table: "GameConfigurations",
                newName: "OptionsAfterNMoves");

            migrationBuilder.AddColumn<string>(
                name: "PlayersMovesJson",
                table: "GameStates",
                type: "text",
                nullable: false,
                defaultValue: "");

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
