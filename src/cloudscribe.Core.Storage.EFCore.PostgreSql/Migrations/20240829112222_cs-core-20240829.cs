using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.Core.Storage.EFCore.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class cscore20240829 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "maximum_inactivity_in_minutes",
                table: "cs_site",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "maximum_inactivity_in_minutes",
                table: "cs_site");
        }
    }
}
