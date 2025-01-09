using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameConfigurations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    BoardSizeWidth = table.Column<int>(type: "INTEGER", nullable: false),
                    BoardSizeHeight = table.Column<int>(type: "INTEGER", nullable: false),
                    MovableBoardWidth = table.Column<int>(type: "INTEGER", nullable: false),
                    MovableBoardHeight = table.Column<int>(type: "INTEGER", nullable: false),
                    ChipsCountJson = table.Column<string>(type: "TEXT", nullable: false),
                    WinCondition = table.Column<int>(type: "INTEGER", nullable: false),
                    OptionsAfterNMoves = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    GameConfigId = table.Column<string>(type: "TEXT", nullable: false),
                    BoardJson = table.Column<string>(type: "TEXT", nullable: false),
                    ChipsLeftJson = table.Column<string>(type: "TEXT", nullable: false),
                    PlayersMovesJson = table.Column<string>(type: "TEXT", nullable: false),
                    GameState_GridX = table.Column<int>(type: "INTEGER", nullable: false),
                    GameState_GridY = table.Column<int>(type: "INTEGER", nullable: false),
                    GameState_PlayerNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    GameState_Player1Options = table.Column<bool>(type: "INTEGER", nullable: false),
                    GameState_Player2Options = table.Column<bool>(type: "INTEGER", nullable: false),
                    GameState_Win = table.Column<int>(type: "INTEGER", nullable: false),
                    Player1Id = table.Column<string>(type: "TEXT", nullable: true),
                    Player2Id = table.Column<string>(type: "TEXT", nullable: true),
                    GameMode = table.Column<string>(type: "TEXT", nullable: true),
                    GamePassword = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSaveAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameSessions_GameConfigurations_GameConfigId",
                        column: x => x.GameConfigId,
                        principalTable: "GameConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameSessions_Users_Player1Id",
                        column: x => x.Player1Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameSessions_Users_Player2Id",
                        column: x => x.Player2Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_GameConfigId",
                table: "GameSessions",
                column: "GameConfigId");

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
                name: "Users");
        }
    }
}
