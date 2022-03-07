using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Management.Db.Migrations
{
    public partial class AddedLocoVideoStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocoVideoStreams",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LocoId = table.Column<Guid>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocoVideoStreams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocoVideoStreams_Locos_LocoId",
                        column: x => x.LocoId,
                        principalTable: "Locos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocoVideoStreams_LocoId",
                table: "LocoVideoStreams",
                column: "LocoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocoVideoStreams");
        }
    }
}
