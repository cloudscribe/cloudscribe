using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.PostgreSql.Migrations
{
    public partial class cscore20190215 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "footer_content",
                table: "cs_site",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "header_content",
                table: "cs_site",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "require2_fa",
                table: "cs_site",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_site_name_link",
                table: "cs_site",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "footer_content",
                table: "cs_site");

            migrationBuilder.DropColumn(
                name: "header_content",
                table: "cs_site");

            migrationBuilder.DropColumn(
                name: "require2_fa",
                table: "cs_site");

            migrationBuilder.DropColumn(
                name: "show_site_name_link",
                table: "cs_site");
        }
    }
}
