using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.PostgreSql.Migrations
{
    public partial class cscore20190216 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "show_site_name_link",
                table: "cs_site",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "show_site_name_link",
                table: "cs_site",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));
        }
    }
}
