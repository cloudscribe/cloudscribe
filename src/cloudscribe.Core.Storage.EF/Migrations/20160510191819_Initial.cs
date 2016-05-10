using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace cloudscribe.Core.Storage.EF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mp_Currency",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Code = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getutcdate()"),
                    DecimalPlaces = table.Column<string>(nullable: true),
                    DecimalPointChar = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getutcdate()"),
                    SymbolLeft = table.Column<string>(nullable: true),
                    SymbolRight = table.Column<string>(nullable: true),
                    ThousandsPointChar = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Guid);
                });
            migrationBuilder.CreateTable(
                name: "mp_GeoCountry",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    ISOCode2 = table.Column<string>(nullable: false),
                    ISOCode3 = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoCountry", x => x.Guid);
                });
            migrationBuilder.CreateTable(
                name: "mp_GeoZone",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Code = table.Column<string>(nullable: false),
                    CountryGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoZone", x => x.Guid);
                });
            migrationBuilder.CreateTable(
                name: "mp_Language",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Guid);
                });
            migrationBuilder.CreateTable(
                name: "mp_SiteHosts",
                columns: table => new
                {
                    HostGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    HostName = table.Column<string>(nullable: false),
                    SiteGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteHost", x => x.HostGuid);
                });
            migrationBuilder.CreateTable(
                name: "mp_Roles",
                columns: table => new
                {
                    RoleGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    DisplayName = table.Column<string>(nullable: false),
                    RoleName = table.Column<string>(nullable: false),
                    SiteGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteRole", x => x.RoleGuid);
                });
            migrationBuilder.CreateTable(
                name: "mp_Sites",
                columns: table => new
                {
                    SiteGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    AccountApprovalEmailCsv = table.Column<string>(nullable: true),
                    AddThisDotComUsername = table.Column<string>(nullable: true),
                    AliasId = table.Column<string>(nullable: true),
                    AllowDbFallbackWithLdap = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    AllowNewRegistration = table.Column<bool>(type: "bit", nullable: false, defaultValue: 1),
                    AllowPersistentLogin = table.Column<bool>(type: "bit", nullable: false, defaultValue: 1),
                    AutoCreateLdapUserOnFirstLogin = table.Column<bool>(type: "bit", nullable: false, defaultValue: 1),
                    CaptchaOnLogin = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    CaptchaOnRegistration = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    CompanyCountry = table.Column<string>(nullable: true),
                    CompanyFax = table.Column<string>(nullable: true),
                    CompanyLocality = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    CompanyPhone = table.Column<string>(nullable: true),
                    CompanyPostalCode = table.Column<string>(nullable: true),
                    CompanyPublicEmail = table.Column<string>(nullable: true),
                    CompanyRegion = table.Column<string>(nullable: true),
                    CompanyStreetAddress = table.Column<string>(nullable: true),
                    CompanyStreetAddress2 = table.Column<string>(nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getutcdate()"),
                    DefaultEmailFromAddress = table.Column<string>(nullable: true),
                    DefaultEmailFromAlias = table.Column<string>(nullable: true),
                    DisableDbAuth = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    DkimDomain = table.Column<string>(nullable: true),
                    DkimPrivateKey = table.Column<string>(nullable: true),
                    DkimPublicKey = table.Column<string>(nullable: true),
                    DkimSelector = table.Column<string>(nullable: true),
                    EmailLdapDbFallback = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    FacebookAppId = table.Column<string>(nullable: true),
                    FacebookAppSecret = table.Column<string>(nullable: true),
                    GoogleAnalyticsProfileId = table.Column<string>(nullable: true),
                    GoogleClientId = table.Column<string>(nullable: true),
                    GoogleClientSecret = table.Column<string>(nullable: true),
                    IsDataProtected = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    IsServerAdminSite = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    LdapDomain = table.Column<string>(nullable: true),
                    LdapPort = table.Column<int>(type: "int", nullable: false, defaultValue: 389),
                    LdapRootDN = table.Column<string>(nullable: true),
                    LdapServer = table.Column<string>(nullable: true),
                    LdapUserDNKey = table.Column<string>(nullable: true),
                    LoginInfoBottom = table.Column<string>(nullable: true),
                    LoginInfoTop = table.Column<string>(nullable: true),
                    MaxInvalidPasswordAttempts = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    MicrosoftClientId = table.Column<string>(nullable: true),
                    MicrosoftClientSecret = table.Column<string>(nullable: true),
                    MinRequiredPasswordLength = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    OidConnectAppId = table.Column<string>(nullable: true),
                    OidConnectAppSecret = table.Column<string>(nullable: true),
                    PreferredHostName = table.Column<string>(nullable: true),
                    PrivacyPolicy = table.Column<string>(nullable: true),
                    ReallyDeleteUsers = table.Column<bool>(type: "bit", nullable: false, defaultValue: 1),
                    RecaptchaPrivateKey = table.Column<string>(nullable: true),
                    RecaptchaPublicKey = table.Column<string>(nullable: true),
                    RegistrationAgreement = table.Column<string>(nullable: true),
                    RegistrationPreamble = table.Column<string>(nullable: true),
                    RequireApprovalBeforeLogin = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    RequireConfirmedEmail = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    RequireConfirmedPhone = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    RequiresQuestionAndAnswer = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    SignEmailWithDkim = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    SiteFolderName = table.Column<string>(nullable: true, defaultValue: ""),
                    SiteIsClosed = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    SiteIsClosedMessage = table.Column<string>(nullable: true),
                    SiteName = table.Column<string>(nullable: false),
                    SmsClientId = table.Column<string>(nullable: true),
                    SmsFrom = table.Column<string>(nullable: true),
                    SmsSecureToken = table.Column<string>(nullable: true),
                    SmtpPassword = table.Column<string>(nullable: true),
                    SmtpPort = table.Column<int>(type: "int", nullable: false, defaultValue: 25),
                    SmtpPreferredEncoding = table.Column<string>(nullable: true),
                    SmtpRequiresAuth = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    SmtpServer = table.Column<string>(nullable: true),
                    SmtpUseSsl = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    SmtpUser = table.Column<string>(nullable: true),
                    Theme = table.Column<string>(nullable: true),
                    TimeZoneId = table.Column<string>(nullable: true),
                    TwitterConsumerKey = table.Column<string>(nullable: true),
                    TwitterConsumerSecret = table.Column<string>(nullable: true),
                    UseEmailForLogin = table.Column<bool>(type: "bit", nullable: false, defaultValue: 1),
                    UseLdapAuth = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSettings", x => x.SiteGuid);
                });
            migrationBuilder.CreateTable(
                name: "mp_Users",
                columns: table => new
                {
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    AccountApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: 1),
                    AuthorBio = table.Column<string>(nullable: true),
                    AvatarUrl = table.Column<string>(nullable: true),
                    CanAutoLockout = table.Column<bool>(type: "bit", nullable: false, defaultValue: 1),
                    Comment = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    DisplayInMemberList = table.Column<bool>(type: "bit", nullable: false, defaultValue: 1),
                    DisplayName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    FirstName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    IsLockedOut = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    LastLoginDate = table.Column<DateTime>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: true),
                    LockoutEndDateUtc = table.Column<DateTime>(nullable: true),
                    MustChangePwd = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    NewEmail = table.Column<string>(nullable: true),
                    NewEmailApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    NormalizedEmail = table.Column<string>(nullable: false),
                    NormalizedUserName = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    RolesChanged = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    SecurityStamp = table.Column<string>(nullable: true),
                    Signature = table.Column<string>(nullable: true),
                    SiteGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<string>(nullable: true),
                    TimeZoneId = table.Column<string>(nullable: true),
                    Trusted = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    UserName = table.Column<string>(nullable: false),
                    WebSiteUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteUser", x => x.UserGuid);
                });
            migrationBuilder.CreateTable(
                name: "mp_UserClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    SiteGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "mp_UserLocation",
                columns: table => new
                {
                    RowID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    CaptureCount = table.Column<int>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Continent = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    FirstCaptureUTC = table.Column<DateTime>(nullable: false),
                    HostName = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    IPAddressLong = table.Column<long>(nullable: false),
                    ISP = table.Column<string>(nullable: true),
                    LastCaptureUTC = table.Column<DateTime>(nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Region = table.Column<string>(nullable: true),
                    SiteGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeZone = table.Column<string>(nullable: true),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocation", x => x.RowID);
                });
            migrationBuilder.CreateTable(
                name: "mp_UserLogins",
                columns: table => new
                {
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.UserGuid, x.SiteGuid, x.LoginProvider, x.ProviderKey });
                });
            migrationBuilder.CreateTable(
                name: "mp_UserRoles",
                columns: table => new
                {
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserGuid, x.RoleGuid });
                });
            migrationBuilder.CreateIndex(
                name: "IX_GeoCountry_ISOCode2",
                table: "mp_GeoCountry",
                column: "ISOCode2");
            migrationBuilder.CreateIndex(
                name: "IX_GeoZone_CountryGuid",
                table: "mp_GeoZone",
                column: "CountryGuid");
            migrationBuilder.CreateIndex(
                name: "IX_SiteHost_HostName",
                table: "mp_SiteHosts",
                column: "HostName");
            migrationBuilder.CreateIndex(
                name: "IX_SiteHost_SiteGuid",
                table: "mp_SiteHosts",
                column: "SiteGuid");
            migrationBuilder.CreateIndex(
                name: "IX_SiteRole_RoleGuid",
                table: "mp_Roles",
                column: "RoleGuid",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "IX_SiteRole_RoleName",
                table: "mp_Roles",
                column: "RoleName");
            migrationBuilder.CreateIndex(
                name: "IX_SiteRole_SiteGuid",
                table: "mp_Roles",
                column: "SiteGuid");
            migrationBuilder.CreateIndex(
                name: "IX_SiteSettings_SiteFolderName",
                table: "mp_Sites",
                column: "SiteFolderName");
            migrationBuilder.CreateIndex(
                name: "IX_SiteUser_NormalizedEmail",
                table: "mp_Users",
                column: "NormalizedEmail");
            migrationBuilder.CreateIndex(
                name: "IX_SiteUser_NormalizedUserName",
                table: "mp_Users",
                column: "NormalizedUserName");
            migrationBuilder.CreateIndex(
                name: "IX_SiteUser_SiteGuid",
                table: "mp_Users",
                column: "SiteGuid");
            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_ClaimType",
                table: "mp_UserClaims",
                column: "ClaimType");
            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_SiteGuid",
                table: "mp_UserClaims",
                column: "SiteGuid");
            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserGuid",
                table: "mp_UserClaims",
                column: "UserGuid");
            migrationBuilder.CreateIndex(
                name: "IX_UserLocation_UserGuid",
                table: "mp_UserLocation",
                column: "UserGuid");
            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_SiteGuid",
                table: "mp_UserLogins",
                column: "SiteGuid");
            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserGuid",
                table: "mp_UserLogins",
                column: "UserGuid");
            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleGuid",
                table: "mp_UserRoles",
                column: "RoleGuid");
            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserGuid",
                table: "mp_UserRoles",
                column: "UserGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("mp_Currency");
            migrationBuilder.DropTable("mp_GeoCountry");
            migrationBuilder.DropTable("mp_GeoZone");
            migrationBuilder.DropTable("mp_Language");
            migrationBuilder.DropTable("mp_SiteHosts");
            migrationBuilder.DropTable("mp_Roles");
            migrationBuilder.DropTable("mp_Sites");
            migrationBuilder.DropTable("mp_Users");
            migrationBuilder.DropTable("mp_UserClaims");
            migrationBuilder.DropTable("mp_UserLocation");
            migrationBuilder.DropTable("mp_UserLogins");
            migrationBuilder.DropTable("mp_UserRoles");
        }
    }
}
