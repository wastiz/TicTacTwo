using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameConfigurations_Users_CreatedBy",
                table: "GameConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_GameConfigurations_GameConfigId",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_GameSessions_GameConfigId",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_GameConfigurations_CreatedBy",
                table: "GameConfigurations");

            migrationBuilder.DropColumn(
                name: "ChipsCountJson",
                table: "GameConfigurations");

            migrationBuilder.AddColumn<string>(
                name: "GameConfigurationId",
                table: "GameSessions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GameStatus",
                table: "GameSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Player1Chips",
                table: "GameConfigurations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Player2Chips",
                table: "GameConfigurations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "GameConfigurations",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_GameConfigurationId",
                table: "GameSessions",
                column: "GameConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_GameConfigurations_UserId",
                table: "GameConfigurations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameConfigurations_Users_UserId",
                table: "GameConfigurations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_GameConfigurations_GameConfigurationId",
                table: "GameSessions",
                column: "GameConfigurationId",
                principalTable: "GameConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameConfigurations_Users_UserId",
                table: "GameConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_GameConfigurations_GameConfigurationId",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_GameSessions_GameConfigurationId",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_GameConfigurations_UserId",
                table: "GameConfigurations");

            migrationBuilder.DropColumn(
                name: "GameConfigurationId",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "GameStatus",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "Player1Chips",
                table: "GameConfigurations");

            migrationBuilder.DropColumn(
                name: "Player2Chips",
                table: "GameConfigurations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GameConfigurations");

            migrationBuilder.AddColumn<string>(
                name: "ChipsCountJson",
                table: "GameConfigurations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_GameConfigId",
                table: "GameSessions",
                column: "GameConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_GameConfigurations_CreatedBy",
                table: "GameConfigurations",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_GameConfigurations_Users_CreatedBy",
                table: "GameConfigurations",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_GameConfigurations_GameConfigId",
                table: "GameSessions",
                column: "GameConfigId",
                principalTable: "GameConfigurations",
                principalColumn: "Id");
        }
    }
}
