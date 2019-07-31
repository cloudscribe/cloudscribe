using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.SQLite.Migrations
{
    public partial class cscore20190420 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrowserKey",
                table: "cs_User",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SingleBrowserSessions",
                table: "cs_Site",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
               name: "LdapUseSsl",
               table: "cs_Site",
               nullable: false,
               defaultValue: false);

            //migrationBuilder.RenameColumn(
            //    name: "UseLdapAuth",
            //    table: "cs_Site",
            //    newName: "LdapUseSsl");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "BrowserKey",
            //    table: "cs_User");

            //migrationBuilder.DropColumn(
            //    name: "SingleBrowserSessions",
            //    table: "cs_Site");
        }
    }
}
