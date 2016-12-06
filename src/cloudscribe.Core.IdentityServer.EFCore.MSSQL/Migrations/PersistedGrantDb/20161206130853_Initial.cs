using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.IdentityServer.EFCore.MSSQL.Migrations.PersistedGrantDb
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "csids_PersistedGrants",
                columns: table => new
                {
                    Key = table.Column<string>(maxLength: 200, nullable: false),
                    Type = table.Column<string>(maxLength: 50, nullable: false),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: false),
                    SiteId = table.Column<string>(maxLength: 36, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_PersistedGrants", x => new { x.Key, x.Type });
                });

            migrationBuilder.CreateIndex(
                name: "IX_csids_PersistedGrants_SiteId",
                table: "csids_PersistedGrants",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_PersistedGrants_SubjectId",
                table: "csids_PersistedGrants",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_PersistedGrants_SubjectId_ClientId",
                table: "csids_PersistedGrants",
                columns: new[] { "SubjectId", "ClientId" });

            migrationBuilder.CreateIndex(
                name: "IX_csids_PersistedGrants_SubjectId_ClientId_Type",
                table: "csids_PersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "csids_PersistedGrants");
        }
    }
}
