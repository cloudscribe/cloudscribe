using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.IdentityServer.EFCore.SQLite.Migrations
{
    public partial class csidsconfig20190104 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "csids_IdentityResources",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "NonEditable",
                table: "csids_IdentityResources",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "csids_IdentityResources",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "csids_Clients",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DeviceCodeLifetime",
                table: "csids_Clients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAccessed",
                table: "csids_Clients",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NonEditable",
                table: "csids_Clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "csids_Clients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserCodeType",
                table: "csids_Clients",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserSsoLifetime",
                table: "csids_Clients",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "csids_ApiResources",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAccessed",
                table: "csids_ApiResources",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NonEditable",
                table: "csids_ApiResources",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "csids_ApiResources",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "csids_ApiResourceProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false),
                    ApiResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_ApiResourceProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_ApiResourceProperty_csids_ApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "csids_ApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_IdentityResourceProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false),
                    IdentityResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_IdentityResourceProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_IdentityResourceProperty_csids_IdentityResources_IdentityResourceId",
                        column: x => x.IdentityResourceId,
                        principalTable: "csids_IdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_csids_ApiResourceProperty_ApiResourceId",
                table: "csids_ApiResourceProperty",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_IdentityResourceProperty_IdentityResourceId",
                table: "csids_IdentityResourceProperty",
                column: "IdentityResourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "csids_ApiResourceProperty");

            migrationBuilder.DropTable(
                name: "csids_IdentityResourceProperty");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "csids_IdentityResources");

            migrationBuilder.DropColumn(
                name: "NonEditable",
                table: "csids_IdentityResources");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "csids_IdentityResources");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "DeviceCodeLifetime",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "LastAccessed",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "NonEditable",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "UserCodeType",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "UserSsoLifetime",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "csids_ApiResources");

            migrationBuilder.DropColumn(
                name: "LastAccessed",
                table: "csids_ApiResources");

            migrationBuilder.DropColumn(
                name: "NonEditable",
                table: "csids_ApiResources");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "csids_ApiResources");
        }
    }
}
