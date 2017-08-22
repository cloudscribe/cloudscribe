using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.IdentityServer.EFCore.pgsql.Migrations
{
    public partial class changes20170820 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoutSessionRequired",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "LogoutUri",
                table: "csids_Clients");

            migrationBuilder.AddColumn<bool>(
                name: "AlwaysIncludeUserClaimsInIdToken",
                table: "csids_Clients",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BackChannelLogoutSessionRequired",
                table: "csids_Clients",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BackChannelLogoutUri",
                table: "csids_Clients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FrontChannelLogoutSessionRequired",
                table: "csids_Clients",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FrontChannelLogoutUri",
                table: "csids_Clients",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
