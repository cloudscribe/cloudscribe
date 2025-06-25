using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.Core.Storage.EFCore.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class cscoreblackwhitelistingipaddressesaddingflag20250623 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWhitelisted",
                table: "cs_BlackWhiteListIpAddresses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWhitelisted",
                table: "cs_BlackWhiteListIpAddresses");
        }
    }
}
