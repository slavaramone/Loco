using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Management.Db.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreationDateTimeUtc = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    MapItemId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shunters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreationDateTimeUtc = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    MapItemId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shunters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationDateTimeUtc = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    FirstName = table.Column<string>(maxLength: 256, nullable: false),
                    LastName = table.Column<string>(maxLength: 256, nullable: true),
                    Login = table.Column<string>(maxLength: 512, nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 256, nullable: false),
                    PasswordSalt = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cameras",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreationDateTimeUtc = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    Position = table.Column<int>(nullable: false),
                    NucNumber = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    LocoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cameras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cameras_Locos_LocoId",
                        column: x => x.LocoId,
                        principalTable: "Locos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocoApiKeys",
                columns: table => new
                {
                    LocoId = table.Column<Guid>(nullable: false),
                    ApiKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocoApiKeys", x => x.LocoId);
                    table.ForeignKey(
                        name: "FK_LocoApiKeys_Locos_LocoId",
                        column: x => x.LocoId,
                        principalTable: "Locos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreationDateTimeUtc = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    IsTakeAverageValue = table.Column<bool>(nullable: false),
                    LocoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorGroups_Locos_LocoId",
                        column: x => x.LocoId,
                        principalTable: "Locos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    UserRole = table.Column<int>(nullable: false),
                    CreationDateTimeUtc = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToRoles", x => new { x.UserId, x.UserRole });
                    table.ForeignKey(
                        name: "FK_UserToRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreationDateTimeUtc = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    TrackerId = table.Column<string>(nullable: true),
                    SensorGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_SensorGroups_SensorGroupId",
                        column: x => x.SensorGroupId,
                        principalTable: "SensorGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FuelLevelCalibrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RawValue = table.Column<double>(nullable: false),
                    CalibratedValue = table.Column<double>(nullable: false),
                    FuelLevelSensorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelLevelCalibrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelLevelCalibrations_Sensors_FuelLevelSensorId",
                        column: x => x.FuelLevelSensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_LocoId",
                table: "Cameras",
                column: "LocoId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelLevelCalibrations_FuelLevelSensorId",
                table: "FuelLevelCalibrations",
                column: "FuelLevelSensorId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorGroups_LocoId",
                table: "SensorGroups",
                column: "LocoId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_SensorGroupId",
                table: "Sensors",
                column: "SensorGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cameras");

            migrationBuilder.DropTable(
                name: "FuelLevelCalibrations");

            migrationBuilder.DropTable(
                name: "LocoApiKeys");

            migrationBuilder.DropTable(
                name: "Shunters");

            migrationBuilder.DropTable(
                name: "UserToRoles");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SensorGroups");

            migrationBuilder.DropTable(
                name: "Locos");
        }
    }
}
