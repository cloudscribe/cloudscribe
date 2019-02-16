using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.MSSQL.Migrations
{
    public partial class cscore20190215 : Migration
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
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FooterContent",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "HeaderContent",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "Require2FA",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "ShowSiteNameLink",
                table: "cs_Site");
        }
    }
}
