using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.IdentityServer.EFCore.MSSQL.Migrations
{
    public partial class changes20170820 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_csids_ApiScopes_SiteId_Name",
                table: "csids_ApiScopes");

            migrationBuilder.DropColumn(
                name: "LogoutSessionRequired",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "LogoutUri",
                table: "csids_Clients");

            migrationBuilder.AddColumn<bool>(
                name: "AlwaysIncludeUserClaimsInIdToken",
                table: "csids_Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BackChannelLogoutSessionRequired",
                table: "csids_Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BackChannelLogoutUri",
                table: "csids_Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FrontChannelLogoutSessionRequired",
                table: "csids_Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FrontChannelLogoutUri",
                table: "csids_Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_csids_ApiScopes_SiteId_Name",
                table: "csids_ApiScopes",
                columns: new[] { "SiteId", "Name" },
                unique: true,
                filter: "[SiteId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_csids_ApiScopes_SiteId_Name",
                table: "csids_ApiScopes");

            migrationBuilder.DropColumn(
                name: "AlwaysIncludeUserClaimsInIdToken",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "BackChannelLogoutSessionRequired",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "BackChannelLogoutUri",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "FrontChannelLogoutSessionRequired",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "FrontChannelLogoutUri",
                table: "csids_Clients");

            migrationBuilder.AddColumn<bool>(
                name: "LogoutSessionRequired",
                table: "csids_Clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LogoutUri",
                table: "csids_Clients",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_csids_ApiScopes_SiteId_Name",
                table: "csids_ApiScopes",
                columns: new[] { "SiteId", "Name" },
                unique: true);
        }
    }
}
