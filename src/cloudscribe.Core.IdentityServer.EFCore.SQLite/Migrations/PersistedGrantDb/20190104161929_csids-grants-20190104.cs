using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.IdentityServer.EFCore.SQLite.Migrations.PersistedGrantDb
{
    public partial class csidsgrants20190104 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Expiration",
            //    table: "csids_PersistedGrants",
            //    nullable: true,
            //    oldClrType: typeof(DateTime));

            migrationBuilder.CreateTable(
                name: "csids_DeviceFlowCodes",
                columns: table => new
                {
                    UserCode = table.Column<string>(maxLength: 200, nullable: false),
                    DeviceCode = table.Column<string>(maxLength: 200, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_DeviceFlowCodes", x => x.UserCode);
                });

            migrationBuilder.CreateIndex(
                name: "IX_csids_DeviceFlowCodes_DeviceCode",
                table: "csids_DeviceFlowCodes",
                column: "DeviceCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "csids_DeviceFlowCodes");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Expiration",
            //    table: "csids_PersistedGrants",
            //    nullable: false,
            //    oldClrType: typeof(DateTime),
            //    oldNullable: true);
        }
    }
}
