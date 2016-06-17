using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EF.Migrations
{
    public partial class cleanup20160617 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TimeZoneId",
                table: "cs_User",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "cs_User",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NewEmail",
                table: "cs_User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TimeZoneId",
                table: "cs_User",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "cs_User",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NewEmail",
                table: "cs_User",
                nullable: true);
        }
    }
}
