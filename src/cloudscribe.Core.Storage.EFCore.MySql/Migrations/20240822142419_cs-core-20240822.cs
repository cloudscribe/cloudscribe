using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.Core.Storage.EFCore.MySql.Migrations
{
    /// <inheritdoc />
    public partial class cscore20240822 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaximumInactivityInMinutes",
                table: "cs_Site",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
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
