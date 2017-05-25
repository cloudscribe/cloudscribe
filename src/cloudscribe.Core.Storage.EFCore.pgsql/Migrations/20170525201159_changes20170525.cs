using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.pgsql.Migrations
{
    public partial class changes20170525 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AgreementAcceptedUtc",
                table: "cs_User",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailConfirmSentUtc",
                table: "cs_User",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OidConnectAppSecret",
                table: "cs_Site",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OidConnectAuthority",
                table: "cs_Site",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgreementAcceptedUtc",
                table: "cs_User");

            migrationBuilder.DropColumn(
                name: "EmailConfirmSentUtc",
                table: "cs_User");

            migrationBuilder.DropColumn(
                name: "OidConnectAuthority",
                table: "cs_Site");

            migrationBuilder.AlterColumn<string>(
                name: "OidConnectAppSecret",
                table: "cs_Site",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}
