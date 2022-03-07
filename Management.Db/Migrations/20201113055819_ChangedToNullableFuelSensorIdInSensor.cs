using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Management.Db.Migrations
{
    public partial class ChangedToNullableFuelSensorIdInSensor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "FuelSensorId",
                table: "Sensors",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "FuelSensorId",
                table: "Sensors",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
