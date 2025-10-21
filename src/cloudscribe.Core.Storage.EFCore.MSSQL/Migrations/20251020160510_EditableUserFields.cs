using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.Core.Storage.EFCore.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class EditableUserFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowUserToEditDisplayName",
                table: "cs_Site",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowUserToEditFirstAndLastName",
                table: "cs_Site",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowUserToEditDisplayName",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "AllowUserToEditFirstAndLastName",
                table: "cs_Site");
        }
    }
}
