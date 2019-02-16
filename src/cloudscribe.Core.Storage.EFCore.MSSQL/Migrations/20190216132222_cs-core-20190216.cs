using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.MSSQL.Migrations
{
    public partial class cscore20190216 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ShowSiteNameLink",
                table: "cs_Site",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ShowSiteNameLink",
                table: "cs_Site",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));
        }
    }
}
