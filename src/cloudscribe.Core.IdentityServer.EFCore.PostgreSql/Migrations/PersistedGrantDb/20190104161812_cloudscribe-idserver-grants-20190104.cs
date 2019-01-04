using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.IdentityServer.EFCore.PostgreSql.Migrations.PersistedGrantDb
{
    public partial class cloudscribeidservergrants20190104 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "expiration",
                table: "csids_persisted_grants",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateTable(
                name: "csids_device_flow_codes",
                columns: table => new
                {
                    user_code = table.Column<string>(maxLength: 200, nullable: false),
                    device_code = table.Column<string>(maxLength: 200, nullable: false),
                    subject_id = table.Column<string>(maxLength: 200, nullable: true),
                    client_id = table.Column<string>(maxLength: 200, nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    expiration = table.Column<DateTime>(nullable: false),
                    data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_device_flow_codes", x => x.user_code);
                });

            migrationBuilder.CreateIndex(
                name: "ix_csids_device_flow_codes_device_code",
                table: "csids_device_flow_codes",
                column: "device_code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "csids_device_flow_codes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "expiration",
                table: "csids_persisted_grants",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
