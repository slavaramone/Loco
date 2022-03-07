using Microsoft.EntityFrameworkCore.Migrations;

namespace Management.Db.Migrations
{
    public partial class AddedUrlHdToLocoVideoStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlHd",
                table: "LocoVideoStreams",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlHd",
                table: "LocoVideoStreams");
        }
    }
}
