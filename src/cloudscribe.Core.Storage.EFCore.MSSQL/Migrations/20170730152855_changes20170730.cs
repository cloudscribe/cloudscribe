using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Storage.EFCore.MSSQL.Migrations
{
    public partial class changes20170730 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "CanAutoLockout",
                table: "cs_User",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "SiteIsClosed",
                table: "cs_Site",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PwdRequireDigit",
                table: "cs_Site",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PwdRequireLowercase",
                table: "cs_Site",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PwdRequireNonAlpha",
                table: "cs_Site",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PwdRequireUppercase",
                table: "cs_Site",
                type: "bit",
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

            migrationBuilder.AlterColumn<bool>(
                name: "CanAutoLockout",
                table: "cs_User",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "SiteIsClosed",
                table: "cs_Site",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
