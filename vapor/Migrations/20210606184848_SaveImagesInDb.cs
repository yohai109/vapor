using Microsoft.EntityFrameworkCore.Migrations;

namespace vapor.Migrations
{
    public partial class SaveImagesInDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "imageUrl",
                table: "GameImage",
                newName: "imageType");

            migrationBuilder.AddColumn<string>(
                name: "imageBase64",
                table: "GameImage",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imageBase64",
                table: "GameImage");

            migrationBuilder.RenameColumn(
                name: "imageType",
                table: "GameImage",
                newName: "imageUrl");
        }
    }
}
