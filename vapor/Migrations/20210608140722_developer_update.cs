using Microsoft.EntityFrameworkCore.Migrations;

namespace vapor.Migrations
{
    public partial class developer_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fileContentType",
                table: "Developer",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fileContentType",
                table: "Developer");
        }
    }
}
