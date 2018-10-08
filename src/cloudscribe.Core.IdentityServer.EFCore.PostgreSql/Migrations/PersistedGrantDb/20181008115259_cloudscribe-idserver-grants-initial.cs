using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.IdentityServer.EFCore.PostgreSql.Migrations.PersistedGrantDb
{
    public partial class cloudscribeidservergrantsinitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "csids_persisted_grants",
                columns: table => new
                {
                    site_id = table.Column<string>(maxLength: 36, nullable: false),
                    key = table.Column<string>(maxLength: 200, nullable: false),
                    type = table.Column<string>(maxLength: 50, nullable: false),
                    subject_id = table.Column<string>(maxLength: 200, nullable: true),
                    client_id = table.Column<string>(maxLength: 200, nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    expiration = table.Column<DateTime>(nullable: false),
                    data = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csids_persisted_grants", x => new { x.key, x.type });
                });

            migrationBuilder.CreateIndex(
                name: "ix_csids_persisted_grants_site_id",
                table: "csids_persisted_grants",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_persisted_grants_subject_id",
                table: "csids_persisted_grants",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "ix_csids_persisted_grants_subject_id_client_id",
                table: "csids_persisted_grants",
                columns: new[] { "subject_id", "client_id" });

            migrationBuilder.CreateIndex(
                name: "ix_csids_persisted_grants_subject_id_client_id_type",
                table: "csids_persisted_grants",
                columns: new[] { "subject_id", "client_id", "type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "csids_persisted_grants");
        }
    }
}
