using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.pgsql.Migrations
{
    public partial class cscore20180818 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cs_User");

            migrationBuilder.DropColumn(
                name: "Trusted",
                table: "cs_User");

            migrationBuilder.RenameColumn(
                name: "ReallyDeleteUsers",
                table: "cs_Site",
                newName: "RequireCookieConsent");

            migrationBuilder.AddColumn<string>(
                name: "CookiePolicySummary",
                table: "cs_Site",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedUtc",
                table: "cs_Site",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CookiePolicySummary",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "LastModifiedUtc",
                table: "cs_Site");

            migrationBuilder.RenameColumn(
                name: "RequireCookieConsent",
                table: "cs_Site",
                newName: "ReallyDeleteUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cs_User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Trusted",
                table: "cs_User",
                nullable: false,
                defaultValue: false);
        }
    }
}
