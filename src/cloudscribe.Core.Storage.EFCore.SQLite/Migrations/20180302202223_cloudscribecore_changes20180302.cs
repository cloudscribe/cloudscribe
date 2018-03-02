using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Storage.EFCore.SQLite.Migrations
{
    public partial class cloudscribecore_changes20180302 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailApiEndpoint",
                table: "cs_Site",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailApiKey",
                table: "cs_Site",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailSenderName",
                table: "cs_Site",
                maxLength: 100,
                nullable: false,
                defaultValue: "SmtpMailSender");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailApiEndpoint",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "EmailApiKey",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "EmailSenderName",
                table: "cs_Site");
        }
    }
}
