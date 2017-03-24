using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.IdentityServer.EFCore.pgsql.Migrations
{
    public partial class changes20170324 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_csids_IdentityResources_Name",
                table: "csids_IdentityResources");

            migrationBuilder.DropIndex(
                name: "IX_csids_Clients_ClientId",
                table: "csids_Clients");

            migrationBuilder.DropIndex(
                name: "IX_csids_ApiScopes_Name",
                table: "csids_ApiScopes");

            migrationBuilder.DropIndex(
                name: "IX_csids_ApiResources_Name",
                table: "csids_ApiResources");

            migrationBuilder.AddColumn<string>(
                name: "SiteId",
                table: "csids_ApiScopes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_csids_IdentityResources_SiteId_Name",
                table: "csids_IdentityResources",
                columns: new[] { "SiteId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_csids_Clients_SiteId_ClientId",
                table: "csids_Clients",
                columns: new[] { "SiteId", "ClientId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_csids_ApiScopes_SiteId_Name",
                table: "csids_ApiScopes",
                columns: new[] { "SiteId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_csids_ApiResources_SiteId_Name",
                table: "csids_ApiResources",
                columns: new[] { "SiteId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_csids_IdentityResources_SiteId_Name",
                table: "csids_IdentityResources");

            migrationBuilder.DropIndex(
                name: "IX_csids_Clients_SiteId_ClientId",
                table: "csids_Clients");

            migrationBuilder.DropIndex(
                name: "IX_csids_ApiScopes_SiteId_Name",
                table: "csids_ApiScopes");

            migrationBuilder.DropIndex(
                name: "IX_csids_ApiResources_SiteId_Name",
                table: "csids_ApiResources");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "csids_ApiScopes");

            migrationBuilder.CreateIndex(
                name: "IX_csids_IdentityResources_Name",
                table: "csids_IdentityResources",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_csids_Clients_ClientId",
                table: "csids_Clients",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_csids_ApiScopes_Name",
                table: "csids_ApiScopes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_csids_ApiResources_Name",
                table: "csids_ApiResources",
                column: "Name",
                unique: true);
        }
    }
}
