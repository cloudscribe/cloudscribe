using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.QueryTool.EFCore.MySql.Migrations
{
    public partial class AddEnableAsApi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableAsApi",
                table: "csqt_SavedQuery",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableAsApi",
                table: "csqt_SavedQuery");
        }
    }
}
