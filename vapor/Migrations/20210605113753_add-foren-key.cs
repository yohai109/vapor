using Microsoft.EntityFrameworkCore.Migrations;

namespace vapor.Migrations
{
    public partial class addforenkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Developer_developerid",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_GameImage_Game_gameid",
                table: "GameImage");

            migrationBuilder.RenameColumn(
                name: "gameid",
                table: "GameImage",
                newName: "gameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameImage_gameid",
                table: "GameImage",
                newName: "IX_GameImage_gameId");

            migrationBuilder.RenameColumn(
                name: "developerid",
                table: "Game",
                newName: "developerId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_developerid",
                table: "Game",
                newName: "IX_Game_developerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Developer_developerId",
                table: "Game",
                column: "developerId",
                principalTable: "Developer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameImage_Game_gameId",
                table: "GameImage",
                column: "gameId",
                principalTable: "Game",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Developer_developerId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_GameImage_Game_gameId",
                table: "GameImage");

            migrationBuilder.RenameColumn(
                name: "gameId",
                table: "GameImage",
                newName: "gameid");

            migrationBuilder.RenameIndex(
                name: "IX_GameImage_gameId",
                table: "GameImage",
                newName: "IX_GameImage_gameid");

            migrationBuilder.RenameColumn(
                name: "developerId",
                table: "Game",
                newName: "developerid");

            migrationBuilder.RenameIndex(
                name: "IX_Game_developerId",
                table: "Game",
                newName: "IX_Game_developerid");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Developer_developerid",
                table: "Game",
                column: "developerid",
                principalTable: "Developer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameImage_Game_gameid",
                table: "GameImage",
                column: "gameid",
                principalTable: "Game",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
