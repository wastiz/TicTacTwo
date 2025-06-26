using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
