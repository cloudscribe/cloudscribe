using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.PostgreSql.Migrations
{
    public partial class cscore20190217 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "logo_url",
                table: "cs_site",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "logo_url",
                table: "cs_site");
        }
    }
}
