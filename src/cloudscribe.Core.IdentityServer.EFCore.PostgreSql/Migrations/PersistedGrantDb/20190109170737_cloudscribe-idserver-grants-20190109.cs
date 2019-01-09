using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.IdentityServer.EFCore.PostgreSql.Migrations.PersistedGrantDb
{
    public partial class cloudscribeidservergrants20190109 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "site_id",
                table: "csids_device_flow_codes",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_csids_device_flow_codes_site_id",
                table: "csids_device_flow_codes",
                column: "site_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_csids_device_flow_codes_site_id",
                table: "csids_device_flow_codes");

            migrationBuilder.DropColumn(
                name: "site_id",
                table: "csids_device_flow_codes");
        }
    }
}
