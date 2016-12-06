using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace cloudscribe.Core.IdentityServer.EFCore.MSSQL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "csids_ApiResources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    SiteId = table.Column<string>(maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_ApiResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "csids_Clients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AbsoluteRefreshTokenLifetime = table.Column<int>(nullable: false),
                    AccessTokenLifetime = table.Column<int>(nullable: false),
                    AccessTokenType = table.Column<int>(nullable: false),
                    AllowAccessTokensViaBrowser = table.Column<bool>(nullable: false),
                    AllowOfflineAccess = table.Column<bool>(nullable: false),
                    AllowPlainTextPkce = table.Column<bool>(nullable: false),
                    AllowRememberConsent = table.Column<bool>(nullable: false),
                    AlwaysSendClientClaims = table.Column<bool>(nullable: false),
                    AuthorizationCodeLifetime = table.Column<int>(nullable: false),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    ClientName = table.Column<string>(maxLength: 200, nullable: true),
                    ClientUri = table.Column<string>(maxLength: 2000, nullable: true),
                    EnableLocalLogin = table.Column<bool>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    IdentityTokenLifetime = table.Column<int>(nullable: false),
                    IncludeJwtId = table.Column<bool>(nullable: false),
                    LogoUri = table.Column<string>(nullable: true),
                    LogoutSessionRequired = table.Column<bool>(nullable: false),
                    LogoutUri = table.Column<string>(nullable: true),
                    PrefixClientClaims = table.Column<bool>(nullable: false),
                    ProtocolType = table.Column<string>(maxLength: 200, nullable: false),
                    RefreshTokenExpiration = table.Column<int>(nullable: false),
                    RefreshTokenUsage = table.Column<int>(nullable: false),
                    RequireClientSecret = table.Column<bool>(nullable: false),
                    RequireConsent = table.Column<bool>(nullable: false),
                    RequirePkce = table.Column<bool>(nullable: false),
                    SiteId = table.Column<string>(maxLength: 36, nullable: false),
                    SlidingRefreshTokenLifetime = table.Column<int>(nullable: false),
                    UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "csids_IdentityResources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Emphasize = table.Column<bool>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(nullable: false),
                    SiteId = table.Column<string>(maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_IdentityResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "csids_ApiClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApiResourceId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_ApiClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_ApiClaims_csids_ApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "csids_ApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_ApiScopes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApiResourceId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Emphasize = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_ApiScopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_ApiScopes_csids_ApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "csids_ApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_ApiSecrets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApiResourceId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Type = table.Column<string>(maxLength: 250, nullable: true),
                    Value = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_ApiSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_ApiSecrets_csids_ApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "csids_ApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_ClientClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_ClientClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_ClientClaims_csids_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "csids_Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_ClientCorsOrigins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    Origin = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_ClientCorsOrigins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_ClientCorsOrigins_csids_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "csids_Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_ClientGrantTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    GrantType = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_ClientGrantTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_ClientGrantTypes_csids_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "csids_Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_ClientIdPRestrictions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    Provider = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_ClientIdPRestrictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_ClientIdPRestrictions_csids_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "csids_Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_ClientPostLogoutRedirectUris",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    PostLogoutRedirectUri = table.Column<string>(maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_ClientPostLogoutRedirectUris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_ClientPostLogoutRedirectUris_csids_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "csids_Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_ClientRedirectUris",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    RedirectUri = table.Column<string>(maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_ClientRedirectUris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_ClientRedirectUris_csids_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "csids_Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_ClientScopes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    Scope = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_ClientScopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_ClientScopes_csids_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "csids_Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_ClientSecrets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Type = table.Column<string>(maxLength: 250, nullable: true),
                    Value = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_ClientSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_ClientSecrets_csids_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "csids_Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_IdentityClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdentityResourceId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_IdentityClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_IdentityClaims_csids_IdentityResources_IdentityResourceId",
                        column: x => x.IdentityResourceId,
                        principalTable: "csids_IdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csids_ApiScopeClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApiScopeId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csids_ApiScopeClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csids_ApiScopeClaims_csids_ApiScopes_ApiScopeId",
                        column: x => x.ApiScopeId,
                        principalTable: "csids_ApiScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_csids_ApiResources_Name",
                table: "csids_ApiResources",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_csids_ApiResources_SiteId",
                table: "csids_ApiResources",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_ApiClaims_ApiResourceId",
                table: "csids_ApiClaims",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_ApiScopes_ApiResourceId",
                table: "csids_ApiScopes",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_ApiScopes_Name",
                table: "csids_ApiScopes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_csids_ApiScopeClaims_ApiScopeId",
                table: "csids_ApiScopeClaims",
                column: "ApiScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_ApiSecrets_ApiResourceId",
                table: "csids_ApiSecrets",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_Clients_ClientId",
                table: "csids_Clients",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_csids_Clients_SiteId",
                table: "csids_Clients",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_ClientClaims_ClientId",
                table: "csids_ClientClaims",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_ClientCorsOrigins_ClientId",
                table: "csids_ClientCorsOrigins",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_ClientGrantTypes_ClientId",
                table: "csids_ClientGrantTypes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_ClientIdPRestrictions_ClientId",
                table: "csids_ClientIdPRestrictions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_ClientPostLogoutRedirectUris_ClientId",
                table: "csids_ClientPostLogoutRedirectUris",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_ClientRedirectUris_ClientId",
                table: "csids_ClientRedirectUris",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_ClientScopes_ClientId",
                table: "csids_ClientScopes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_ClientSecrets_ClientId",
                table: "csids_ClientSecrets",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_IdentityClaims_IdentityResourceId",
                table: "csids_IdentityClaims",
                column: "IdentityResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_csids_IdentityResources_Name",
                table: "csids_IdentityResources",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_csids_IdentityResources_SiteId",
                table: "csids_IdentityResources",
                column: "SiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "csids_ApiClaims");

            migrationBuilder.DropTable(
                name: "csids_ApiScopeClaims");

            migrationBuilder.DropTable(
                name: "csids_ApiSecrets");

            migrationBuilder.DropTable(
                name: "csids_ClientClaims");

            migrationBuilder.DropTable(
                name: "csids_ClientCorsOrigins");

            migrationBuilder.DropTable(
                name: "csids_ClientGrantTypes");

            migrationBuilder.DropTable(
                name: "csids_ClientIdPRestrictions");

            migrationBuilder.DropTable(
                name: "csids_ClientPostLogoutRedirectUris");

            migrationBuilder.DropTable(
                name: "csids_ClientRedirectUris");

            migrationBuilder.DropTable(
                name: "csids_ClientScopes");

            migrationBuilder.DropTable(
                name: "csids_ClientSecrets");

            migrationBuilder.DropTable(
                name: "csids_IdentityClaims");

            migrationBuilder.DropTable(
                name: "csids_ApiScopes");

            migrationBuilder.DropTable(
                name: "csids_Clients");

            migrationBuilder.DropTable(
                name: "csids_IdentityResources");

            migrationBuilder.DropTable(
                name: "csids_ApiResources");
        }
    }
}
