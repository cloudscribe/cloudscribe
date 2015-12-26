using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace cloudscribe.Logging.EF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "LogIds");
            migrationBuilder.CreateTable(
                name: "mp_SystemLog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR LogIds"),
                    Culture = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    LogDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getutcdate()"),
                    LogLevel = table.Column<string>(nullable: true),
                    Logger = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    ShortUrl = table.Column<string>(nullable: true),
                    Thread = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogItem", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence("LogIds");
            migrationBuilder.DropTable("mp_SystemLog");
        }
    }
}
