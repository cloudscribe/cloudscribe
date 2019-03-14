using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.MSSQL.Migrations
{
    public partial class cscore20190314 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowDbFallbackWithLdap",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "AutoCreateLdapUserOnFirstLogin",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "EmailLdapDbFallback",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "UseLdapAuth",
                table: "cs_Site");

            migrationBuilder.AddColumn<bool>(
                name: "LdapUseSsl",
                table: "cs_Site",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LdapUserDNFormat",
                table: "cs_Site",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LdapUseSsl",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "LdapUserDNFormat",
                table: "cs_Site");

            migrationBuilder.AddColumn<bool>(
                name: "AllowDbFallbackWithLdap",
                table: "cs_Site",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutoCreateLdapUserOnFirstLogin",
                table: "cs_Site",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EmailLdapDbFallback",
                table: "cs_Site",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseLdapAuth",
                table: "cs_Site",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
