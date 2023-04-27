using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.Core.Storage.EFCore.PostgreSql.Migrations
{
    public partial class cscore20230426 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "smtp_oauth_authorize_endpoint",
                table: "cs_site",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "smtp_oauth_client_id",
                table: "cs_site",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "smtp_oauth_client_secret",
                table: "cs_site",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "smtp_oauth_scopes_csv",
                table: "cs_site",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "smtp_oauth_token_endpoint",
                table: "cs_site",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "cs_user_interactive_service_token",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    site_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cloudscribe_service_provider = table.Column<string>(type: "text", nullable: true),
                    user_principal_name = table.Column<string>(type: "text", nullable: true),
                    secure_token = table.Column<string>(type: "text", nullable: true),
                    token_expires_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    token_has_expired = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_user_interactive_service_token", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_interactive_service_token_cloudscribe_service_provi",
                table: "cs_user_interactive_service_token",
                column: "cloudscribe_service_provider");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_interactive_service_token_site_id",
                table: "cs_user_interactive_service_token",
                column: "site_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cs_user_interactive_service_token");

            migrationBuilder.DropColumn(
                name: "smtp_oauth_authorize_endpoint",
                table: "cs_site");

            migrationBuilder.DropColumn(
                name: "smtp_oauth_client_id",
                table: "cs_site");

            migrationBuilder.DropColumn(
                name: "smtp_oauth_client_secret",
                table: "cs_site");

            migrationBuilder.DropColumn(
                name: "smtp_oauth_scopes_csv",
                table: "cs_site");

            migrationBuilder.DropColumn(
                name: "smtp_oauth_token_endpoint",
                table: "cs_site");
        }
    }
}
