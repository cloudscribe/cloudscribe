using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.Core.Storage.EFCore.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class EditableUserFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "allow_user_to_edit_display_name",
                table: "cs_site",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "allow_user_to_edit_first_and_last_name",
                table: "cs_site",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "allow_user_to_edit_display_name",
                table: "cs_site");

            migrationBuilder.DropColumn(
                name: "allow_user_to_edit_first_and_last_name",
                table: "cs_site");
        }
    }
}
