using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.pgsql.Migrations
{
    public partial class changes20170627 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForcedCulture",
                table: "cs_Site",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ForcedUICulture",
                table: "cs_Site",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForcedCulture",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "ForcedUICulture",
                table: "cs_Site");
        }
    }
}
