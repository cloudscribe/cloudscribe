using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.IdentityServer.EFCore.SQLite.Migrations.PersistedGrantDb
{
    public partial class csidsgrants20190109 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SiteId",
                table: "csids_DeviceFlowCodes",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_csids_DeviceFlowCodes_SiteId",
                table: "csids_DeviceFlowCodes",
                column: "SiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_csids_DeviceFlowCodes_SiteId",
                table: "csids_DeviceFlowCodes");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "csids_DeviceFlowCodes");
        }
    }
}
