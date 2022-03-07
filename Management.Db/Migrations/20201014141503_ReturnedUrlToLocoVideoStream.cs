using Microsoft.EntityFrameworkCore.Migrations;

namespace Management.Db.Migrations
{
    public partial class ReturnedUrlToLocoVideoStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadUrl",
                table: "LocoVideoStreams");

            migrationBuilder.DropColumn(
                name: "ViewUrl",
                table: "LocoVideoStreams");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "LocoVideoStreams",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "LocoVideoStreams");

            migrationBuilder.AddColumn<string>(
                name: "DownloadUrl",
                table: "LocoVideoStreams",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ViewUrl",
                table: "LocoVideoStreams",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
