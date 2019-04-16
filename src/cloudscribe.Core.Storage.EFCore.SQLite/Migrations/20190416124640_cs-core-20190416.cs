using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.SQLite.Migrations
{
    public partial class cscore20190416 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OidConnectScopesCsv",
                table: "cs_Site",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OidConnectScopesCsv",
                table: "cs_Site");
        }
    }
}
