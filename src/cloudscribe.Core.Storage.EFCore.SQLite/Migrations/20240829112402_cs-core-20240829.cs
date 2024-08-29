using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.Core.Storage.EFCore.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class cscore20240829 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaximumInactivityInMinutes",
                table: "cs_Site",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaximumInactivityInMinutes",
                table: "cs_Site");
        }
    }
}
