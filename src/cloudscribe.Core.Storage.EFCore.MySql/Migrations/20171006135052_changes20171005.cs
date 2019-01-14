using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Storage.EFCore.MySql.Migrations
{
    public partial class changes20171005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cs_UserToken",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SiteId = table.Column<Guid>(type: "char(36)", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_UserToken", x => new { x.UserId, x.SiteId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateIndex(
                name: "IX_cs_UserToken_SiteId",
                table: "cs_UserToken",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_UserToken_UserId",
                table: "cs_UserToken",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cs_UserToken");
        }
    }
}
