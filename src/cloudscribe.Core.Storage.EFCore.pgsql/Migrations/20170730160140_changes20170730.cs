using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Storage.EFCore.pgsql.Migrations
{
    public partial class changes20170730 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PwdRequireDigit",
                table: "cs_Site",
                type: "bool",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PwdRequireLowercase",
                table: "cs_Site",
                type: "bool",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PwdRequireNonAlpha",
                table: "cs_Site",
                type: "bool",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PwdRequireUppercase",
                table: "cs_Site",
                type: "bool",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PwdRequireDigit",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "PwdRequireLowercase",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "PwdRequireNonAlpha",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "PwdRequireUppercase",
                table: "cs_Site");
        }
    }
}
