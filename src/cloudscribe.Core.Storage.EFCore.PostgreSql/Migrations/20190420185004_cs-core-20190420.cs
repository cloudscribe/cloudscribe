using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.PostgreSql.Migrations
{
    public partial class cscore20190420 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "browser_key",
                table: "cs_user",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "single_browser_sessions",
                table: "cs_site",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "browser_key",
                table: "cs_user");

            migrationBuilder.DropColumn(
                name: "single_browser_sessions",
                table: "cs_site");
        }
    }
}
