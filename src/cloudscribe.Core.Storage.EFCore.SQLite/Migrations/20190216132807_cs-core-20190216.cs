using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.SQLite.Migrations
{
    public partial class cscore20190216 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FooterContent",
                table: "cs_Site",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderContent",
                table: "cs_Site",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Require2FA",
                table: "cs_Site",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowSiteNameLink",
                table: "cs_Site",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "FooterContent",
            //    table: "cs_Site");

            //migrationBuilder.DropColumn(
            //    name: "HeaderContent",
            //    table: "cs_Site");

            //migrationBuilder.DropColumn(
            //    name: "Require2FA",
            //    table: "cs_Site");

            //migrationBuilder.DropColumn(
            //    name: "ShowSiteNameLink",
            //    table: "cs_Site");
        }
    }
}
