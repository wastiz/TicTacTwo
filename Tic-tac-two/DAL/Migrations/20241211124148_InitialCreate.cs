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
                    ChipsCount = table.Column<string>(type: "TEXT", nullable: false),
                    WinCondition = table.Column<int>(type: "INTEGER", nullable: false),
                    OptionsAfterNMoves = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameStates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    GameConfigId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Board = table.Column<string>(type: "TEXT", nullable: false),
                    ChipsLeft = table.Column<string>(type: "TEXT", nullable: false),
                    PlayersMoves = table.Column<string>(type: "TEXT", nullable: false),
                    GridX = table.Column<int>(type: "INTEGER", nullable: false),
                    GridY = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Player1Options = table.Column<bool>(type: "INTEGER", nullable: false),
                    Player2Options = table.Column<bool>(type: "INTEGER", nullable: false),
                    Win = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStates", x => x.Id);
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
                    GameStateId = table.Column<string>(type: "TEXT", nullable: true),
                    Player1Id = table.Column<string>(type: "TEXT", nullable: true),
                    Player2Id = table.Column<string>(type: "TEXT", nullable: true),
                    GameMode = table.Column<string>(type: "TEXT", nullable: false),
                    GamePassword = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.Id);
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
                name: "GameConfigurations");

            migrationBuilder.DropTable(
                name: "GameSessions");

            migrationBuilder.DropTable(
                name: "GameStates");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
