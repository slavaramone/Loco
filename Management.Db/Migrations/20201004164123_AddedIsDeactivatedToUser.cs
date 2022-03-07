using Microsoft.EntityFrameworkCore.Migrations;

namespace Management.Db.Migrations
{
    public partial class AddedIsDeactivatedToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactivated",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactivated",
                table: "Users");
        }
    }
}
