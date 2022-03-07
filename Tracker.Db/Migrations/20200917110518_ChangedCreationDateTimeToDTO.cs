using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracker.Db.Migrations
{
    public partial class ChangedCreationDateTimeToDTO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreationDateTime",
                table: "FuelLevels",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "FuelLevels",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));
        }
    }
}
