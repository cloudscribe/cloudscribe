using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EF.Migrations
{
    public partial class cleanup201606172 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "cs_User");

            migrationBuilder.DropColumn(
                name: "State",
                table: "cs_User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "cs_User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "cs_User",
                nullable: true);
        }
    }
}
