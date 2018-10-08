using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace cloudscribe.Core.IdentityServer.EFCore.PostgreSql.Migrations
{
    public partial class cloudscribeidserverconfiginitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "csids_api_resources",
                columns: table => new
                {
                    site_id = table.Column<string>(maxLength: 36, nullable: false),
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    enabled = table.Column<bool>(nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    display_name = table.Column<string>(maxLength: 200, nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_api_resources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "csids_clients",
                columns: table => new
                {
                    site_id = table.Column<string>(maxLength: 36, nullable: false),
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    enabled = table.Column<bool>(nullable: false),
                    client_id = table.Column<string>(maxLength: 200, nullable: false),
                    protocol_type = table.Column<string>(maxLength: 200, nullable: false),
                    require_client_secret = table.Column<bool>(nullable: false),
                    client_name = table.Column<string>(maxLength: 200, nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    client_uri = table.Column<string>(maxLength: 2000, nullable: true),
                    logo_uri = table.Column<string>(nullable: true),
                    require_consent = table.Column<bool>(nullable: false),
                    allow_remember_consent = table.Column<bool>(nullable: false),
                    always_include_user_claims_in_id_token = table.Column<bool>(nullable: false),
                    require_pkce = table.Column<bool>(nullable: false),
                    allow_plain_text_pkce = table.Column<bool>(nullable: false),
                    allow_access_tokens_via_browser = table.Column<bool>(nullable: false),
                    front_channel_logout_uri = table.Column<string>(maxLength: 2000, nullable: true),
                    front_channel_logout_session_required = table.Column<bool>(nullable: false),
                    back_channel_logout_uri = table.Column<string>(maxLength: 2000, nullable: true),
                    back_channel_logout_session_required = table.Column<bool>(nullable: false),
                    allow_offline_access = table.Column<bool>(nullable: false),
                    identity_token_lifetime = table.Column<int>(nullable: false),
                    access_token_lifetime = table.Column<int>(nullable: false),
                    authorization_code_lifetime = table.Column<int>(nullable: false),
                    consent_lifetime = table.Column<int>(nullable: true),
                    absolute_refresh_token_lifetime = table.Column<int>(nullable: false),
                    sliding_refresh_token_lifetime = table.Column<int>(nullable: false),
                    refresh_token_usage = table.Column<int>(nullable: false),
                    update_access_token_claims_on_refresh = table.Column<bool>(nullable: false),
                    refresh_token_expiration = table.Column<int>(nullable: false),
                    access_token_type = table.Column<int>(nullable: false),
                    enable_local_login = table.Column<bool>(nullable: false),
                    include_jwt_id = table.Column<bool>(nullable: false),
                    always_send_client_claims = table.Column<bool>(nullable: false),
                    client_claims_prefix = table.Column<string>(maxLength: 200, nullable: true),
                    pair_wise_subject_salt = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "csids_identity_resources",
                columns: table => new
                {
                    site_id = table.Column<string>(maxLength: 36, nullable: false),
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    enabled = table.Column<bool>(nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    display_name = table.Column<string>(maxLength: 200, nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    required = table.Column<bool>(nullable: false),
                    emphasize = table.Column<bool>(nullable: false),
                    show_in_discovery_document = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_identity_resources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "csids_api_claims",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    type = table.Column<string>(maxLength: 200, nullable: false),
                    api_resource_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_api_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_api_claims_csids_api_resources_api_resource_id",
                        column: x => x.api_resource_id,
                        principalTable: "csids_api_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_api_scopes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    site_id = table.Column<string>(nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    display_name = table.Column<string>(maxLength: 200, nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    required = table.Column<bool>(nullable: false),
                    emphasize = table.Column<bool>(nullable: false),
                    show_in_discovery_document = table.Column<bool>(nullable: false),
                    api_resource_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_api_scopes", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_api_scopes_csids_api_resources_api_resource_id",
                        column: x => x.api_resource_id,
                        principalTable: "csids_api_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_api_secrets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    value = table.Column<string>(maxLength: 2000, nullable: true),
                    expiration = table.Column<DateTime>(nullable: true),
                    type = table.Column<string>(maxLength: 250, nullable: true),
                    api_resource_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_api_secrets", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_api_secrets_csids_api_resources_api_resource_id",
                        column: x => x.api_resource_id,
                        principalTable: "csids_api_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_client_claims",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    type = table.Column<string>(maxLength: 250, nullable: false),
                    value = table.Column<string>(maxLength: 250, nullable: false),
                    client_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_client_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_client_claims_csids_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "csids_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_client_cors_origins",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    origin = table.Column<string>(maxLength: 150, nullable: false),
                    client_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_client_cors_origins", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_client_cors_origins_csids_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "csids_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_client_grant_types",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    grant_type = table.Column<string>(maxLength: 250, nullable: false),
                    client_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_client_grant_types", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_client_grant_types_csids_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "csids_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_client_id_p_restrictions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    provider = table.Column<string>(maxLength: 200, nullable: false),
                    client_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_client_id_p_restrictions", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_client_id_p_restrictions_csids_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "csids_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_client_post_logout_redirect_uris",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    post_logout_redirect_uri = table.Column<string>(maxLength: 2000, nullable: false),
                    client_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_client_post_logout_redirect_uris", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_client_post_logout_redirect_uris_csids_clients_client~",
                        column: x => x.client_id,
                        principalTable: "csids_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_client_props",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    key = table.Column<string>(maxLength: 250, nullable: false),
                    value = table.Column<string>(maxLength: 2000, nullable: false),
                    client_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_client_props", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_client_props_csids_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "csids_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_client_redirect_uris",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    redirect_uri = table.Column<string>(maxLength: 2000, nullable: false),
                    client_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_client_redirect_uris", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_client_redirect_uris_csids_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "csids_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_client_scopes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    scope = table.Column<string>(maxLength: 200, nullable: false),
                    client_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_client_scopes", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_client_scopes_csids_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "csids_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_client_secrets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    description = table.Column<string>(maxLength: 2000, nullable: true),
                    value = table.Column<string>(maxLength: 250, nullable: false),
                    expiration = table.Column<DateTime>(nullable: true),
                    type = table.Column<string>(maxLength: 250, nullable: true),
                    client_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_client_secrets", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_client_secrets_csids_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "csids_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_identity_claims",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    type = table.Column<string>(maxLength: 200, nullable: false),
                    identity_resource_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_identity_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_identity_claims_csids_identity_resources_identity_reso~",
                        column: x => x.identity_resource_id,
                        principalTable: "csids_identity_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_api_scope_claims",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    type = table.Column<string>(maxLength: 200, nullable: false),
                    api_scope_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_api_scope_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_csids_api_scope_claims_csids_api_scopes_api_scope_id",
                        column: x => x.api_scope_id,
                        principalTable: "csids_api_scopes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_csids_api_claims_api_resource_id",
                table: "csids_api_claims",
                column: "api_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_api_resources_site_id",
                table: "csids_api_resources",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_api_resources_site_id_name",
                table: "csids_api_resources",
                columns: new[] { "site_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_csids_api_scope_claims_api_scope_id",
                table: "csids_api_scope_claims",
                column: "api_scope_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_api_scopes_api_resource_id",
                table: "csids_api_scopes",
                column: "api_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_api_scopes_site_id_name",
                table: "csids_api_scopes",
                columns: new[] { "site_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_csids_api_secrets_api_resource_id",
                table: "csids_api_secrets",
                column: "api_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_client_claims_client_id",
                table: "csids_client_claims",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_client_cors_origins_client_id",
                table: "csids_client_cors_origins",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_client_grant_types_client_id",
                table: "csids_client_grant_types",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_client_id_p_restrictions_client_id",
                table: "csids_client_id_p_restrictions",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_client_post_logout_redirect_uris_client_id",
                table: "csids_client_post_logout_redirect_uris",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_client_props_client_id",
                table: "csids_client_props",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_client_redirect_uris_client_id",
                table: "csids_client_redirect_uris",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_client_scopes_client_id",
                table: "csids_client_scopes",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_client_secrets_client_id",
                table: "csids_client_secrets",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_clients_site_id",
                table: "csids_clients",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_clients_site_id_client_id",
                table: "csids_clients",
                columns: new[] { "site_id", "client_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_csids_identity_claims_identity_resource_id",
                table: "csids_identity_claims",
                column: "identity_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_identity_resources_site_id",
                table: "csids_identity_resources",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_identity_resources_site_id_name",
                table: "csids_identity_resources",
                columns: new[] { "site_id", "name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "csids_api_claims");

            migrationBuilder.DropTable(
                name: "csids_api_scope_claims");

            migrationBuilder.DropTable(
                name: "csids_api_secrets");

            migrationBuilder.DropTable(
                name: "csids_client_claims");

            migrationBuilder.DropTable(
                name: "csids_client_cors_origins");

            migrationBuilder.DropTable(
                name: "csids_client_grant_types");

            migrationBuilder.DropTable(
                name: "csids_client_id_p_restrictions");

            migrationBuilder.DropTable(
                name: "csids_client_post_logout_redirect_uris");

            migrationBuilder.DropTable(
                name: "csids_client_props");

            migrationBuilder.DropTable(
                name: "csids_client_redirect_uris");

            migrationBuilder.DropTable(
                name: "csids_client_scopes");

            migrationBuilder.DropTable(
                name: "csids_client_secrets");

            migrationBuilder.DropTable(
                name: "csids_identity_claims");

            migrationBuilder.DropTable(
                name: "csids_api_scopes");

            migrationBuilder.DropTable(
                name: "csids_clients");

            migrationBuilder.DropTable(
                name: "csids_identity_resources");

            migrationBuilder.DropTable(
                name: "csids_api_resources");
        }
    }
}
