using Microsoft.EntityFrameworkCore.Migrations;

namespace vapor.Migrations
{
    public partial class Cascadedeletegameimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameImage_Game_gameid",
                table: "GameImage");

            migrationBuilder.RenameColumn(
                name: "gameid",
                table: "GameImage",
                newName: "gameID");

            migrationBuilder.RenameIndex(
                name: "IX_GameImage_gameid",
                table: "GameImage",
                newName: "IX_GameImage_gameID");

            migrationBuilder.AlterColumn<string>(
                name: "gameID",
                table: "GameImage",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GameImage_Game_gameID",
                table: "GameImage",
                column: "gameID",
                principalTable: "Game",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameImage_Game_gameID",
                table: "GameImage");

            migrationBuilder.RenameColumn(
                name: "gameID",
                table: "GameImage",
                newName: "gameid");

            migrationBuilder.RenameIndex(
                name: "IX_GameImage_gameID",
                table: "GameImage",
                newName: "IX_GameImage_gameid");

            migrationBuilder.AlterColumn<string>(
                name: "gameid",
                table: "GameImage",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
