using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.QueryTool.EFCore.SQLite.Migrations
{
    public partial class AddEnableAsApi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableAsApi",
                table: "csqt_SavedQuery",
                type: "INTEGER",
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
