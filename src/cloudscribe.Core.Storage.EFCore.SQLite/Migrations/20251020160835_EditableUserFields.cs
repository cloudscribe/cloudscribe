using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.Core.Storage.EFCore.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class EditableUserFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add only the new site settings flags in this migration
            migrationBuilder.AddColumn<bool>(
                name: "AllowUserToEditDisplayName",
                table: "cs_Site",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowUserToEditFirstAndLastName",
                table: "cs_Site",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove only the new site settings flags added by this migration
            migrationBuilder.DropColumn(
                name: "AllowUserToEditDisplayName",
                table: "cs_Site");

            migrationBuilder.DropColumn(
                name: "AllowUserToEditFirstAndLastName",
                table: "cs_Site");
        }
    }
}
