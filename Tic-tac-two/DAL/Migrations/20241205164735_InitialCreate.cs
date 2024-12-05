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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    BoardSizeWidth = table.Column<int>(type: "INTEGER", nullable: false),
                    BoardSizeHeight = table.Column<int>(type: "INTEGER", nullable: false),
                    MovableBoardWidth = table.Column<int>(type: "INTEGER", nullable: false),
                    MovableBoardHeight = table.Column<int>(type: "INTEGER", nullable: false),
                    ChipsCountSerialized = table.Column<string>(type: "TEXT", nullable: false),
                    WinCondition = table.Column<int>(type: "INTEGER", nullable: false),
                    MovePieceAfterNMoves = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StateName = table.Column<string>(type: "TEXT", nullable: false),
                    GameConfigJson = table.Column<string>(type: "TEXT", nullable: false),
                    BoardJson = table.Column<string>(type: "TEXT", nullable: false),
                    ChipsLeftJson = table.Column<string>(type: "TEXT", nullable: false),
                    PlayersMovesJson = table.Column<string>(type: "TEXT", nullable: false),
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameConfigurations");

            migrationBuilder.DropTable(
                name: "GameStates");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
