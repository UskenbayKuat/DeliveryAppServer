using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddSmsLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DicSmsLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MinuteLife = table.Column<int>(type: "integer", nullable: false),
                    TryCount = table.Column<int>(type: "integer", nullable: false),
                    InputCount = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    KazInfoErrorMessage = table.Column<string>(type: "text", nullable: true),
                    TryCountMessage = table.Column<string>(type: "text", nullable: true),
                    InputCountMessage = table.Column<string>(type: "text", nullable: true),
                    CodeErrorMessage = table.Column<string>(type: "text", nullable: true),
                    LifeTimeErrorMessage = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicSmsLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LifeTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActual = table.Column<bool>(type: "boolean", nullable: false),
                    TryCount = table.Column<int>(type: "integer", nullable: false),
                    InputCount = table.Column<int>(type: "integer", nullable: false),
                    SmsCode = table.Column<string>(type: "text", nullable: true),
                    ContentXml = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmsLogs_UserId",
                table: "SmsLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DicSmsLogs");

            migrationBuilder.DropTable(
                name: "SmsLogs");
        }
    }
}
