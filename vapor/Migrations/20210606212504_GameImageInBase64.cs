using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vapor.Migrations
{
    public partial class GameImageInBase64 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imgaeBytes",
                table: "GameImage");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "GameImage",
                newName: "fileContentType");

            migrationBuilder.AddColumn<string>(
                name: "fileBase64",
                table: "GameImage",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fileBase64",
                table: "GameImage");

            migrationBuilder.RenameColumn(
                name: "fileContentType",
                table: "GameImage",
                newName: "name");

            migrationBuilder.AddColumn<byte[]>(
                name: "imgaeBytes",
                table: "GameImage",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
