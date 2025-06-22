using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameStates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    BoardJson = table.Column<string>(type: "text", nullable: false),
                    ChipsLeftJson = table.Column<string>(type: "text", nullable: false),
                    PlayersMovesJson = table.Column<string>(type: "text", nullable: false),
                    GridX = table.Column<int>(type: "integer", nullable: false),
                    GridY = table.Column<int>(type: "integer", nullable: false),
                    PlayerNumber = table.Column<int>(type: "integer", nullable: false),
                    Player1Options = table.Column<bool>(type: "boolean", nullable: false),
                    Player2Options = table.Column<bool>(type: "boolean", nullable: false),
                    Win = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    GamesPlayed = table.Column<int>(type: "integer", nullable: false),
                    GamesWon = table.Column<int>(type: "integer", nullable: false),
                    GamesLost = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameConfigurations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BoardSizeWidth = table.Column<int>(type: "integer", nullable: false),
                    BoardSizeHeight = table.Column<int>(type: "integer", nullable: false),
                    MovableBoardWidth = table.Column<int>(type: "integer", nullable: false),
                    MovableBoardHeight = table.Column<int>(type: "integer", nullable: false),
                    ChipsCountJson = table.Column<string>(type: "text", nullable: false),
                    WinCondition = table.Column<int>(type: "integer", nullable: false),
                    OptionsAfterNMoves = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameConfigurations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    GameConfigId = table.Column<string>(type: "text", nullable: true),
                    GameStateId = table.Column<string>(type: "text", nullable: true),
                    Player1Id = table.Column<string>(type: "text", nullable: true),
                    Player2Id = table.Column<string>(type: "text", nullable: true),
                    GameMode = table.Column<string>(type: "text", nullable: true),
                    GamePassword = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastSaveAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameSessions_GameConfigurations_GameConfigId",
                        column: x => x.GameConfigId,
                        principalTable: "GameConfigurations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameSessions_GameStates_GameStateId",
                        column: x => x.GameStateId,
                        principalTable: "GameStates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameSessions_Users_Player1Id",
                        column: x => x.Player1Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameSessions_Users_Player2Id",
                        column: x => x.Player2Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameConfigurations_CreatedBy",
                table: "GameConfigurations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_GameConfigId",
                table: "GameSessions",
                column: "GameConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_GameStateId",
                table: "GameSessions",
                column: "GameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_Player1Id",
                table: "GameSessions",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_Player2Id",
                table: "GameSessions",
                column: "Player2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameSessions");

            migrationBuilder.DropTable(
                name: "GameConfigurations");

            migrationBuilder.DropTable(
                name: "GameStates");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
