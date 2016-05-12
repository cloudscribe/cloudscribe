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
                name: "cs_Currency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
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
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "cs_GeoCountry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    ISOCode2 = table.Column<string>(nullable: false),
                    ISOCode3 = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoCountry", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "cs_GeoZone",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Code = table.Column<string>(nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoZone", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "cs_Language",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "cs_SiteHost",
                columns: table => new
                {
                    HostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    HostName = table.Column<string>(nullable: false),
                    SiteGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteHost", x => x.HostId);
                });
            migrationBuilder.CreateTable(
                name: "cs_Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    DisplayName = table.Column<string>(nullable: false),
                    RoleName = table.Column<string>(nullable: false),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteRole", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "cs_Site",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
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
                    table.PrimaryKey("PK_SiteSettings", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "cs_User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
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
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<string>(nullable: true),
                    TimeZoneId = table.Column<string>(nullable: true),
                    Trusted = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    UserName = table.Column<string>(nullable: false),
                    WebSiteUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteUser", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "cs_UserClaim",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "cs_UserLocation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    CaptureCount = table.Column<int>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Continent = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    FirstCaptureUtc = table.Column<DateTime>(nullable: false),
                    HostName = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    IpAddressLong = table.Column<long>(nullable: false),
                    Isp = table.Column<string>(nullable: true),
                    LastCaptureUtc = table.Column<DateTime>(nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Region = table.Column<string>(nullable: true),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeZone = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocation", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "cs_UserLogin",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.UserId, x.SiteId, x.LoginProvider, x.ProviderKey });
                });
            migrationBuilder.CreateTable(
                name: "cs_UserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                });
            migrationBuilder.CreateIndex(
                name: "IX_GeoCountry_ISOCode2",
                table: "cs_GeoCountry",
                column: "ISOCode2");
            migrationBuilder.CreateIndex(
                name: "IX_GeoZone_CountryId",
                table: "cs_GeoZone",
                column: "CountryId");
            migrationBuilder.CreateIndex(
                name: "IX_SiteHost_HostName",
                table: "cs_SiteHost",
                column: "HostName");
            migrationBuilder.CreateIndex(
                name: "IX_SiteHost_SiteGuid",
                table: "cs_SiteHost",
                column: "SiteGuid");
            migrationBuilder.CreateIndex(
                name: "IX_SiteRole_Id",
                table: "cs_Role",
                column: "Id",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "IX_SiteRole_RoleName",
                table: "cs_Role",
                column: "RoleName");
            migrationBuilder.CreateIndex(
                name: "IX_SiteRole_SiteId",
                table: "cs_Role",
                column: "SiteId");
            migrationBuilder.CreateIndex(
                name: "IX_SiteSettings_AliasId",
                table: "cs_Site",
                column: "AliasId");
            migrationBuilder.CreateIndex(
                name: "IX_SiteSettings_SiteFolderName",
                table: "cs_Site",
                column: "SiteFolderName");
            migrationBuilder.CreateIndex(
                name: "IX_SiteUser_NormalizedEmail",
                table: "cs_User",
                column: "NormalizedEmail");
            migrationBuilder.CreateIndex(
                name: "IX_SiteUser_NormalizedUserName",
                table: "cs_User",
                column: "NormalizedUserName");
            migrationBuilder.CreateIndex(
                name: "IX_SiteUser_SiteId",
                table: "cs_User",
                column: "SiteId");
            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_ClaimType",
                table: "cs_UserClaim",
                column: "ClaimType");
            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_SiteId",
                table: "cs_UserClaim",
                column: "SiteId");
            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                table: "cs_UserClaim",
                column: "UserId");
            migrationBuilder.CreateIndex(
                name: "IX_UserLocation_UserId",
                table: "cs_UserLocation",
                column: "UserId");
            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_SiteId",
                table: "cs_UserLogin",
                column: "SiteId");
            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                table: "cs_UserLogin",
                column: "UserId");
            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "cs_UserRole",
                column: "RoleId");
            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "cs_UserRole",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("cs_Currency");
            migrationBuilder.DropTable("cs_GeoCountry");
            migrationBuilder.DropTable("cs_GeoZone");
            migrationBuilder.DropTable("cs_Language");
            migrationBuilder.DropTable("cs_SiteHost");
            migrationBuilder.DropTable("cs_Role");
            migrationBuilder.DropTable("cs_Site");
            migrationBuilder.DropTable("cs_User");
            migrationBuilder.DropTable("cs_UserClaim");
            migrationBuilder.DropTable("cs_UserLocation");
            migrationBuilder.DropTable("cs_UserLogin");
            migrationBuilder.DropTable("cs_UserRole");
        }
    }
}
