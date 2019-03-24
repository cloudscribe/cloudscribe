using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.PostgreSql.Migrations
{
    public partial class cscore20190314 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "allow_db_fallback_with_ldap",
                table: "cs_site");

            migrationBuilder.DropColumn(
                name: "auto_create_ldap_user_on_first_login",
                table: "cs_site");

            migrationBuilder.DropColumn(
                name: "email_ldap_db_fallback",
                table: "cs_site");

            migrationBuilder.RenameColumn(
                name: "use_ldap_auth",
                table: "cs_site",
                newName: "ldap_use_ssl");

            migrationBuilder.AddColumn<string>(
                name: "ldap_user_dn_format",
                table: "cs_site",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ldap_user_dn_format",
                table: "cs_site");

            migrationBuilder.RenameColumn(
                name: "ldap_use_ssl",
                table: "cs_site",
                newName: "use_ldap_auth");

            migrationBuilder.AddColumn<bool>(
                name: "allow_db_fallback_with_ldap",
                table: "cs_site",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "auto_create_ldap_user_on_first_login",
                table: "cs_site",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "email_ldap_db_fallback",
                table: "cs_site",
                nullable: false,
                defaultValue: false);
        }
    }
}
