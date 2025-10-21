using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace cloudscribe.Core.Storage.EFCore.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class cscoreIpTableRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Create the new table
            migrationBuilder.CreateTable(
                name: "BlockedPermittedIpAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    Reason = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SiteId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsPermitted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRange = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockedPermittedIpAddresses", x => x.Id);
                });

            // 2) Copy data from legacy table (if it exists). This assumes the legacy table
            //    is present from earlier migrations in SQLite databases.
            migrationBuilder.Sql(
                "INSERT INTO BlockedPermittedIpAddresses (Id, IpAddress, Reason, CreatedDate, LastUpdated, SiteId, IsPermitted, IsRange) " +
                "SELECT Id, IpAddress, Reason, CreatedDate, LastUpdated, SiteId, IsWhitelisted, 0 FROM BlackWhiteListedIpAddresses;");

            // 3) Drop the legacy table
            migrationBuilder.DropTable(
                name: "BlackWhiteListedIpAddresses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1) Recreate legacy table
            migrationBuilder.CreateTable(
                name: "BlackWhiteListedIpAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    IsWhitelisted = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Reason = table.Column<string>(type: "TEXT", nullable: true),
                    SiteId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    migrationBuilder.PrimaryKey("PK_BlackWhiteListedIpAddresses", x => x.Id);
                });

            // 2) Copy data back from the new table
            migrationBuilder.Sql(
                "INSERT INTO BlackWhiteListedIpAddresses (Id, CreatedDate, IpAddress, IsWhitelisted, LastUpdated, Reason, SiteId) " +
                "SELECT Id, CreatedDate, IpAddress, IsPermitted, LastUpdated, Reason, SiteId FROM BlockedPermittedIpAddresses;");

            // 3) Drop the new table
            migrationBuilder.DropTable(
                name: "BlockedPermittedIpAddresses");
        }
    }
}
