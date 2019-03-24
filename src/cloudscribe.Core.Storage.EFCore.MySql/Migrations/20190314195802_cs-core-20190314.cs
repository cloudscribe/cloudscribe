using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.MySql.Migrations
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

            migrationBuilder.RenameColumn(
                name: "UseLdapAuth",
                table: "cs_Site",
                newName: "LdapUseSsl");

            migrationBuilder.AddColumn<string>(
                name: "LdapUserDNFormat",
                table: "cs_Site",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LdapUserDNFormat",
                table: "cs_Site");

            migrationBuilder.RenameColumn(
                name: "LdapUseSsl",
                table: "cs_Site",
                newName: "UseLdapAuth");

            migrationBuilder.AddColumn<bool>(
                name: "AllowDbFallbackWithLdap",
                table: "cs_Site",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutoCreateLdapUserOnFirstLogin",
                table: "cs_Site",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EmailLdapDbFallback",
                table: "cs_Site",
                nullable: false,
                defaultValue: false);
        }
    }
}
