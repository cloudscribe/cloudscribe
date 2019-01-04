using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace cloudscribe.Core.IdentityServer.EFCore.PostgreSql.Migrations
{
    public partial class cloudscribeidserverconfig20190104 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created",
                table: "csids_identity_resources",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "non_editable",
                table: "csids_identity_resources",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated",
                table: "csids_identity_resources",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "logo_uri",
                table: "csids_clients",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created",
                table: "csids_clients",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "device_code_lifetime",
                table: "csids_clients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_accessed",
                table: "csids_clients",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "non_editable",
                table: "csids_clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated",
                table: "csids_clients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "user_code_type",
                table: "csids_clients",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "user_sso_lifetime",
                table: "csids_clients",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created",
                table: "csids_api_resources",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "last_accessed",
                table: "csids_api_resources",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "non_editable",
                table: "csids_api_resources",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated",
                table: "csids_api_resources",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "csids_api_resource_property",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    key = table.Column<string>(maxLength: 250, nullable: false),
                    value = table.Column<string>(maxLength: 2000, nullable: false),
                    api_resource_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_api_resource_property", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_api_resource_property_csids_api_resources_api_resourc~",
                        column: x => x.api_resource_id,
                        principalTable: "csids_api_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_identity_resource_property",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    key = table.Column<string>(maxLength: 250, nullable: false),
                    value = table.Column<string>(maxLength: 2000, nullable: false),
                    identity_resource_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_identity_resource_property", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_identity_resource_property_csids_identity_resources_i~",
                        column: x => x.identity_resource_id,
                        principalTable: "csids_identity_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_csids_api_resource_property_api_resource_id",
                table: "csids_api_resource_property",
                column: "api_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_identity_resource_property_identity_resource_id",
                table: "csids_identity_resource_property",
                column: "identity_resource_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "csids_api_resource_property");

            migrationBuilder.DropTable(
                name: "csids_identity_resource_property");

            migrationBuilder.DropColumn(
                name: "created",
                table: "csids_identity_resources");

            migrationBuilder.DropColumn(
                name: "non_editable",
                table: "csids_identity_resources");

            migrationBuilder.DropColumn(
                name: "updated",
                table: "csids_identity_resources");

            migrationBuilder.DropColumn(
                name: "created",
                table: "csids_clients");

            migrationBuilder.DropColumn(
                name: "device_code_lifetime",
                table: "csids_clients");

            migrationBuilder.DropColumn(
                name: "last_accessed",
                table: "csids_clients");

            migrationBuilder.DropColumn(
                name: "non_editable",
                table: "csids_clients");

            migrationBuilder.DropColumn(
                name: "updated",
                table: "csids_clients");

            migrationBuilder.DropColumn(
                name: "user_code_type",
                table: "csids_clients");

            migrationBuilder.DropColumn(
                name: "user_sso_lifetime",
                table: "csids_clients");

            migrationBuilder.DropColumn(
                name: "created",
                table: "csids_api_resources");

            migrationBuilder.DropColumn(
                name: "last_accessed",
                table: "csids_api_resources");

            migrationBuilder.DropColumn(
                name: "non_editable",
                table: "csids_api_resources");

            migrationBuilder.DropColumn(
                name: "updated",
                table: "csids_api_resources");

            migrationBuilder.AlterColumn<string>(
                name: "logo_uri",
                table: "csids_clients",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);
        }
    }
}
