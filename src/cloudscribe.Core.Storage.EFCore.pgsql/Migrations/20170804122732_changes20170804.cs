using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Storage.EFCore.pgsql.Migrations
{
    public partial class changes20170804 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SiteFolderName",
                table: "cs_Site",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "PwdRequireUppercase",
                table: "cs_Site",
                type: "bool",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "PwdRequireNonAlpha",
                table: "cs_Site",
                type: "bool",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "PwdRequireLowercase",
                table: "cs_Site",
                type: "bool",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "PwdRequireDigit",
                table: "cs_Site",
                type: "bool",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SiteFolderName",
                table: "cs_Site",
                maxLength: 50,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "PwdRequireUppercase",
                table: "cs_Site",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bool");

            migrationBuilder.AlterColumn<bool>(
                name: "PwdRequireNonAlpha",
                table: "cs_Site",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bool");

            migrationBuilder.AlterColumn<bool>(
                name: "PwdRequireLowercase",
                table: "cs_Site",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bool");

            migrationBuilder.AlterColumn<bool>(
                name: "PwdRequireDigit",
                table: "cs_Site",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bool");
        }
    }
}
