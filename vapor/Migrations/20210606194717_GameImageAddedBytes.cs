using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vapor.Migrations
{
    public partial class GameImageAddedBytes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imageBase64",
                table: "GameImage");

            migrationBuilder.RenameColumn(
                name: "imageType",
                table: "GameImage",
                newName: "name");

            migrationBuilder.AddColumn<byte[]>(
                name: "imgaeBytes",
                table: "GameImage",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imgaeBytes",
                table: "GameImage");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "GameImage",
                newName: "imageType");

            migrationBuilder.AddColumn<string>(
                name: "imageBase64",
                table: "GameImage",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
