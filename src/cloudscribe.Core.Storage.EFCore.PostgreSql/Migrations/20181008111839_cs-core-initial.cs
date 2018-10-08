using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EFCore.PostgreSql.Migrations
{
    public partial class cscoreinitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", "'uuid-ossp', '', ''");

            migrationBuilder.CreateTable(
                name: "cs_geo_country",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(maxLength: 255, nullable: false),
                    iso_code2 = table.Column<string>(maxLength: 2, nullable: false),
                    iso_code3 = table.Column<string>(maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_geo_country", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cs_geo_zone",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    country_id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(maxLength: 255, nullable: false),
                    code = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_geo_zone", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cs_role",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    site_id = table.Column<Guid>(nullable: false),
                    normalized_role_name = table.Column<string>(maxLength: 50, nullable: false),
                    role_name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cs_site",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    alias_id = table.Column<string>(maxLength: 36, nullable: true),
                    site_name = table.Column<string>(maxLength: 255, nullable: false),
                    site_folder_name = table.Column<string>(maxLength: 50, nullable: true),
                    preferred_host_name = table.Column<string>(maxLength: 250, nullable: true),
                    is_server_admin_site = table.Column<bool>(nullable: false),
                    created_utc = table.Column<DateTime>(nullable: false),
                    last_modified_utc = table.Column<DateTime>(nullable: false),
                    require_cookie_consent = table.Column<bool>(nullable: false),
                    cookie_policy_summary = table.Column<string>(maxLength: 255, nullable: true),
                    concurrency_stamp = table.Column<string>(nullable: true),
                    theme = table.Column<string>(maxLength: 100, nullable: true),
                    allow_new_registration = table.Column<bool>(nullable: false),
                    require_confirmed_email = table.Column<bool>(nullable: false),
                    require_confirmed_phone = table.Column<bool>(nullable: false),
                    use_ldap_auth = table.Column<bool>(nullable: false),
                    allow_db_fallback_with_ldap = table.Column<bool>(nullable: false),
                    email_ldap_db_fallback = table.Column<bool>(nullable: false),
                    auto_create_ldap_user_on_first_login = table.Column<bool>(nullable: false),
                    ldap_server = table.Column<string>(maxLength: 255, nullable: true),
                    ldap_domain = table.Column<string>(maxLength: 255, nullable: true),
                    ldap_port = table.Column<int>(nullable: false),
                    ldap_root_dn = table.Column<string>(maxLength: 255, nullable: true),
                    ldap_user_dn_key = table.Column<string>(maxLength: 10, nullable: true),
                    use_email_for_login = table.Column<bool>(nullable: false),
                    disable_db_auth = table.Column<bool>(nullable: false),
                    requires_question_and_answer = table.Column<bool>(nullable: false),
                    require_approval_before_login = table.Column<bool>(nullable: false),
                    account_approval_email_csv = table.Column<string>(nullable: true),
                    max_invalid_password_attempts = table.Column<int>(nullable: false),
                    min_required_password_length = table.Column<int>(nullable: false),
                    pwd_require_non_alpha = table.Column<bool>(nullable: false),
                    pwd_require_lowercase = table.Column<bool>(nullable: false),
                    pwd_require_uppercase = table.Column<bool>(nullable: false),
                    pwd_require_digit = table.Column<bool>(nullable: false),
                    allow_persistent_login = table.Column<bool>(nullable: false),
                    captcha_on_registration = table.Column<bool>(nullable: false),
                    captcha_on_login = table.Column<bool>(nullable: false),
                    recaptcha_private_key = table.Column<string>(maxLength: 255, nullable: true),
                    recaptcha_public_key = table.Column<string>(maxLength: 255, nullable: true),
                    use_invisible_recaptcha = table.Column<bool>(nullable: false),
                    facebook_app_id = table.Column<string>(maxLength: 100, nullable: true),
                    facebook_app_secret = table.Column<string>(nullable: true),
                    microsoft_client_id = table.Column<string>(maxLength: 100, nullable: true),
                    microsoft_client_secret = table.Column<string>(nullable: true),
                    google_client_id = table.Column<string>(maxLength: 100, nullable: true),
                    google_client_secret = table.Column<string>(nullable: true),
                    twitter_consumer_key = table.Column<string>(maxLength: 100, nullable: true),
                    twitter_consumer_secret = table.Column<string>(nullable: true),
                    oid_connect_app_id = table.Column<string>(maxLength: 255, nullable: true),
                    oid_connect_app_secret = table.Column<string>(maxLength: 255, nullable: true),
                    oid_connect_authority = table.Column<string>(maxLength: 255, nullable: true),
                    oid_connect_display_name = table.Column<string>(maxLength: 150, nullable: true),
                    add_this_dot_com_username = table.Column<string>(maxLength: 50, nullable: true),
                    time_zone_id = table.Column<string>(maxLength: 50, nullable: true),
                    company_name = table.Column<string>(maxLength: 250, nullable: true),
                    company_street_address = table.Column<string>(maxLength: 250, nullable: true),
                    company_street_address2 = table.Column<string>(maxLength: 250, nullable: true),
                    company_locality = table.Column<string>(maxLength: 200, nullable: true),
                    company_region = table.Column<string>(maxLength: 200, nullable: true),
                    company_postal_code = table.Column<string>(maxLength: 20, nullable: true),
                    company_country = table.Column<string>(maxLength: 10, nullable: true),
                    company_phone = table.Column<string>(maxLength: 20, nullable: true),
                    company_fax = table.Column<string>(maxLength: 20, nullable: true),
                    company_public_email = table.Column<string>(maxLength: 100, nullable: true),
                    company_website = table.Column<string>(maxLength: 255, nullable: true),
                    default_email_from_address = table.Column<string>(maxLength: 100, nullable: true),
                    default_email_from_alias = table.Column<string>(maxLength: 100, nullable: true),
                    smtp_user = table.Column<string>(maxLength: 500, nullable: true),
                    smtp_password = table.Column<string>(nullable: true),
                    smtp_port = table.Column<int>(nullable: false),
                    smtp_preferred_encoding = table.Column<string>(maxLength: 20, nullable: true),
                    smtp_server = table.Column<string>(maxLength: 200, nullable: true),
                    smtp_requires_auth = table.Column<bool>(nullable: false),
                    smtp_use_ssl = table.Column<bool>(nullable: false),
                    dkim_public_key = table.Column<string>(nullable: true),
                    dkim_private_key = table.Column<string>(nullable: true),
                    dkim_domain = table.Column<string>(maxLength: 255, nullable: true),
                    dkim_selector = table.Column<string>(maxLength: 128, nullable: true),
                    sign_email_with_dkim = table.Column<bool>(nullable: false),
                    email_sender_name = table.Column<string>(maxLength: 100, nullable: false, defaultValue: "SmtpMailSender"),
                    email_api_key = table.Column<string>(nullable: true),
                    email_api_endpoint = table.Column<string>(nullable: true),
                    sms_client_id = table.Column<string>(maxLength: 255, nullable: true),
                    sms_secure_token = table.Column<string>(nullable: true),
                    sms_from = table.Column<string>(maxLength: 100, nullable: true),
                    google_analytics_profile_id = table.Column<string>(maxLength: 25, nullable: true),
                    registration_agreement = table.Column<string>(nullable: true),
                    registration_preamble = table.Column<string>(nullable: true),
                    login_info_top = table.Column<string>(nullable: true),
                    login_info_bottom = table.Column<string>(nullable: true),
                    site_is_closed = table.Column<bool>(nullable: false),
                    site_is_closed_message = table.Column<string>(nullable: true),
                    privacy_policy = table.Column<string>(nullable: true),
                    is_data_protected = table.Column<bool>(nullable: false),
                    terms_updated_utc = table.Column<DateTime>(nullable: false),
                    forced_culture = table.Column<string>(maxLength: 10, nullable: true),
                    forced_ui_culture = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_site", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cs_site_host",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    host_name = table.Column<string>(maxLength: 255, nullable: false),
                    site_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_site_host", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cs_user",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    site_id = table.Column<Guid>(nullable: false),
                    email = table.Column<string>(maxLength: 100, nullable: false),
                    user_name = table.Column<string>(maxLength: 50, nullable: false),
                    display_name = table.Column<string>(maxLength: 100, nullable: false),
                    first_name = table.Column<string>(maxLength: 100, nullable: true),
                    last_name = table.Column<string>(maxLength: 100, nullable: true),
                    avatar_url = table.Column<string>(maxLength: 255, nullable: true),
                    date_of_birth = table.Column<DateTime>(nullable: true),
                    created_utc = table.Column<DateTime>(nullable: false),
                    last_modified_utc = table.Column<DateTime>(nullable: false),
                    display_in_member_list = table.Column<bool>(nullable: false),
                    gender = table.Column<string>(nullable: true),
                    is_locked_out = table.Column<bool>(nullable: false),
                    last_login_utc = table.Column<DateTime>(nullable: true),
                    phone_number = table.Column<string>(maxLength: 50, nullable: true),
                    phone_number_confirmed = table.Column<bool>(nullable: false),
                    account_approved = table.Column<bool>(nullable: false),
                    time_zone_id = table.Column<string>(maxLength: 50, nullable: true),
                    web_site_url = table.Column<string>(maxLength: 100, nullable: true),
                    author_bio = table.Column<string>(nullable: true),
                    comment = table.Column<string>(nullable: true),
                    normalized_email = table.Column<string>(maxLength: 100, nullable: false),
                    normalized_user_name = table.Column<string>(maxLength: 50, nullable: false),
                    email_confirmed = table.Column<bool>(nullable: false),
                    email_confirm_sent_utc = table.Column<DateTime>(nullable: true),
                    agreement_accepted_utc = table.Column<DateTime>(nullable: true),
                    lockout_end_date_utc = table.Column<DateTime>(nullable: true),
                    new_email = table.Column<string>(maxLength: 100, nullable: true),
                    new_email_approved = table.Column<bool>(nullable: false),
                    last_password_change_utc = table.Column<DateTime>(nullable: true),
                    must_change_pwd = table.Column<bool>(nullable: false),
                    password_hash = table.Column<string>(nullable: true),
                    can_auto_lockout = table.Column<bool>(nullable: false),
                    access_failed_count = table.Column<int>(nullable: false),
                    roles_changed = table.Column<bool>(nullable: false),
                    security_stamp = table.Column<string>(maxLength: 50, nullable: true),
                    signature = table.Column<string>(nullable: true),
                    two_factor_enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cs_user_claim",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    site_id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    claim_type = table.Column<string>(maxLength: 255, nullable: true),
                    claim_value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_user_claim", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cs_user_location",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    site_id = table.Column<Guid>(nullable: false),
                    ip_address = table.Column<string>(maxLength: 50, nullable: true),
                    ip_address_long = table.Column<long>(nullable: false),
                    host_name = table.Column<string>(maxLength: 255, nullable: true),
                    longitude = table.Column<double>(nullable: false),
                    latitude = table.Column<double>(nullable: false),
                    isp = table.Column<string>(maxLength: 255, nullable: true),
                    continent = table.Column<string>(maxLength: 255, nullable: true),
                    country = table.Column<string>(maxLength: 255, nullable: true),
                    region = table.Column<string>(maxLength: 255, nullable: true),
                    city = table.Column<string>(maxLength: 255, nullable: true),
                    time_zone = table.Column<string>(maxLength: 255, nullable: true),
                    capture_count = table.Column<int>(nullable: false),
                    first_capture_utc = table.Column<DateTime>(nullable: false),
                    last_capture_utc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_user_location", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cs_user_login",
                columns: table => new
                {
                    site_id = table.Column<Guid>(nullable: false),
                    login_provider = table.Column<string>(maxLength: 128, nullable: false),
                    provider_key = table.Column<string>(maxLength: 128, nullable: false),
                    provider_display_name = table.Column<string>(maxLength: 100, nullable: true),
                    user_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_user_login", x => new { x.user_id, x.site_id, x.login_provider, x.provider_key });
                });

            migrationBuilder.CreateTable(
                name: "cs_user_role",
                columns: table => new
                {
                    role_id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_user_role", x => new { x.user_id, x.role_id });
                });

            migrationBuilder.CreateTable(
                name: "cs_user_token",
                columns: table => new
                {
                    site_id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    login_provider = table.Column<string>(maxLength: 450, nullable: false),
                    name = table.Column<string>(maxLength: 450, nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_user_token", x => new { x.user_id, x.site_id, x.login_provider, x.name });
                });

            migrationBuilder.CreateIndex(
                name: "ix_cs_geo_country_iso_code2",
                table: "cs_geo_country",
                column: "iso_code2");

            migrationBuilder.CreateIndex(
                name: "ix_cs_geo_zone_code",
                table: "cs_geo_zone",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "ix_cs_geo_zone_country_id",
                table: "cs_geo_zone",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_role_id",
                table: "cs_role",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_cs_role_normalized_role_name",
                table: "cs_role",
                column: "normalized_role_name");

            migrationBuilder.CreateIndex(
                name: "ix_cs_role_site_id",
                table: "cs_role",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_site_alias_id",
                table: "cs_site",
                column: "alias_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_site_site_folder_name",
                table: "cs_site",
                column: "site_folder_name");

            migrationBuilder.CreateIndex(
                name: "ix_cs_site_host_host_name",
                table: "cs_site_host",
                column: "host_name");

            migrationBuilder.CreateIndex(
                name: "ix_cs_site_host_site_id",
                table: "cs_site_host",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_display_name",
                table: "cs_user",
                column: "display_name");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_normalized_email",
                table: "cs_user",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_normalized_user_name",
                table: "cs_user",
                column: "normalized_user_name");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_site_id",
                table: "cs_user",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_claim_claim_type",
                table: "cs_user_claim",
                column: "claim_type");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_claim_site_id",
                table: "cs_user_claim",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_claim_user_id",
                table: "cs_user_claim",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_location_ip_address",
                table: "cs_user_location",
                column: "ip_address");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_location_user_id",
                table: "cs_user_location",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_login_site_id",
                table: "cs_user_login",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_login_user_id",
                table: "cs_user_login",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_role_role_id",
                table: "cs_user_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_role_user_id",
                table: "cs_user_role",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_token_site_id",
                table: "cs_user_token",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_user_token_user_id",
                table: "cs_user_token",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cs_geo_country");

            migrationBuilder.DropTable(
                name: "cs_geo_zone");

            migrationBuilder.DropTable(
                name: "cs_role");

            migrationBuilder.DropTable(
                name: "cs_site");

            migrationBuilder.DropTable(
                name: "cs_site_host");

            migrationBuilder.DropTable(
                name: "cs_user");

            migrationBuilder.DropTable(
                name: "cs_user_claim");

            migrationBuilder.DropTable(
                name: "cs_user_location");

            migrationBuilder.DropTable(
                name: "cs_user_login");

            migrationBuilder.DropTable(
                name: "cs_user_role");

            migrationBuilder.DropTable(
                name: "cs_user_token");
        }
    }
}
