using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using cloudscribe.Core.Repositories.EF;

namespace cloudscribe.Core.Repositories.EF.Migrations
{
    [DbContext(typeof(CoreDbContext))]
    [Migration("20151208143436_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("cloudscribe.Core.Models.Geography.Currency", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 3);

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "datetime")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "getutcdate()");

                    b.Property<string>("DecimalPlaces")
                        .HasAnnotation("MaxLength", 1);

                    b.Property<string>("DecimalPointChar")
                        .HasAnnotation("MaxLength", 1);

                    b.Property<DateTime>("LastModified")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "datetime")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "getutcdate()");

                    b.Property<string>("SymbolLeft")
                        .HasAnnotation("MaxLength", 15);

                    b.Property<string>("SymbolRight")
                        .HasAnnotation("MaxLength", 15);

                    b.Property<string>("ThousandsPointChar")
                        .HasAnnotation("MaxLength", 1);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<decimal>("Value");

                    b.HasKey("Guid");

                    b.HasAnnotation("Relational:TableName", "mp_Currency");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.Geography.GeoCountry", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<string>("ISOCode2")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 2);

                    b.Property<string>("ISOCode3")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 3);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Guid");

                    b.HasIndex("ISOCode2");

                    b.HasAnnotation("Relational:TableName", "mp_GeoCountry");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.Geography.GeoZone", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("CountryGuid")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Guid");

                    b.HasIndex("CountryGuid");

                    b.HasAnnotation("Relational:TableName", "mp_GeoZone");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.Geography.Language", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 2);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("Sort")
                        .HasAnnotation("SqlServer:ColumnType", "int")
                        .HasAnnotation("SqlServer:DefaultValue", "1")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.HasKey("Guid");

                    b.HasAnnotation("Relational:TableName", "mp_Language");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.Logging.LogItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnName", "ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Culture")
                        .HasAnnotation("MaxLength", 10);

                    b.Property<string>("IpAddress")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<DateTime>("LogDateUtc")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnName", "LogDate")
                        .HasAnnotation("SqlServer:ColumnType", "datetime")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "getutcdate()");

                    b.Property<string>("LogLevel")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("Logger")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Message");

                    b.Property<string>("ShortUrl")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Thread")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "mp_SystemLog");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteFolder", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<string>("FolderName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("SiteGuid")
                        .HasAnnotation("Relational:ColumnName", "SiteGuid")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasAnnotation("Relational:TableName", "mp_SiteFolders");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteHost", b =>
                {
                    b.Property<int>("HostId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnName", "HostID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("HostName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("SiteGuid")
                        .HasAnnotation("Relational:ColumnName", "SiteGuid")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<int>("SiteId")
                        .HasAnnotation("Relational:ColumnName", "SiteID")
                        .HasAnnotation("SqlServer:ColumnType", "int");

                    b.HasKey("HostId");

                    b.HasAnnotation("Relational:TableName", "mp_SiteHosts");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteRole", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnName", "RoleID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<Guid>("RoleGuid")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<Guid>("SiteGuid")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<int>("SiteId")
                        .HasAnnotation("Relational:ColumnName", "SiteID")
                        .HasAnnotation("SqlServer:ColumnType", "int");

                    b.HasKey("RoleId");

                    b.HasIndex("RoleGuid")
                        .IsUnique();

                    b.HasIndex("RoleName");

                    b.HasIndex("SiteGuid");

                    b.HasIndex("SiteId");

                    b.HasAnnotation("Relational:TableName", "mp_Roles");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteSettings", b =>
                {
                    b.Property<int>("SiteId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnName", "SiteID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddThisDotComUsername")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("AllowDbFallbackWithLdap")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("AllowNewRegistration")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "1")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("AllowPersistentLogin")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "1")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("AllowUserFullNameChange")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("ApiKeyExtra1")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("ApiKeyExtra2")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("ApiKeyExtra3")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("ApiKeyExtra4")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("ApiKeyExtra5")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<bool>("AutoCreateLdapUserOnFirstLogin")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "1")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("CaptchaOnLogin")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("CaptchaOnRegistration")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("CompanyCountry")
                        .HasAnnotation("MaxLength", 10);

                    b.Property<string>("CompanyFax")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("CompanyLocality")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("CompanyName")
                        .HasAnnotation("MaxLength", 250);

                    b.Property<string>("CompanyPhone")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("CompanyPostalCode")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("CompanyPublicEmail")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("CompanyRegion")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("CompanyStreetAddress")
                        .HasAnnotation("MaxLength", 250);

                    b.Property<string>("CompanyStreetAddress2")
                        .HasAnnotation("MaxLength", 250);

                    b.Property<string>("DefaultEmailFromAddress")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("DisableDbAuth")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("EmailLdapDbFallback")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("FacebookAppId")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("FacebookAppSecret")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("GoogleAnalyticsProfileId")
                        .HasAnnotation("MaxLength", 25);

                    b.Property<string>("GoogleClientId")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("GoogleClientSecret")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("IsServerAdminSite")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("Layout")
                        .HasAnnotation("MaxLength", 100)
                        .HasAnnotation("Relational:ColumnName", "Skin");

                    b.Property<string>("LdapDomain")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("LdapPort")
                        .HasAnnotation("SqlServer:ColumnType", "int")
                        .HasAnnotation("SqlServer:DefaultValue", "389")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("LdapRootDN")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("LdapServer")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("LdapUserDNKey")
                        .HasAnnotation("MaxLength", 10);

                    b.Property<string>("LoginInfoBottom");

                    b.Property<string>("LoginInfoTop");

                    b.Property<int>("MaxInvalidPasswordAttempts")
                        .HasAnnotation("SqlServer:ColumnType", "int")
                        .HasAnnotation("SqlServer:DefaultValue", "5")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("MicrosoftClientId")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("MicrosoftClientSecret")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<int>("MinReqNonAlphaChars")
                        .HasAnnotation("SqlServer:ColumnType", "int")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<int>("MinRequiredPasswordLength")
                        .HasAnnotation("SqlServer:ColumnType", "int")
                        .HasAnnotation("SqlServer:DefaultValue", "5")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<int>("PasswordAttemptWindowMinutes")
                        .HasAnnotation("SqlServer:ColumnType", "int")
                        .HasAnnotation("SqlServer:DefaultValue", "5")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("PreferredHostName")
                        .HasAnnotation("MaxLength", 250);

                    b.Property<string>("PrivacyPolicy");

                    b.Property<bool>("ReallyDeleteUsers")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "1")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("RecaptchaPrivateKey")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("RecaptchaPublicKey")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("RegistrationAgreement");

                    b.Property<string>("RegistrationPreamble");

                    b.Property<bool>("RequireApprovalBeforeLogin")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("RequiresQuestionAndAnswer")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("SiteFolderName")
                        .HasAnnotation("MaxLength", 50)
                        .HasAnnotation("Relational:DefaultValue", "")
                        .HasAnnotation("Relational:DefaultValueType", "System.String");

                    b.Property<Guid>("SiteGuid")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<bool>("SiteIsClosed")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("SiteIsClosedMessage");

                    b.Property<string>("SiteName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("SmtpPassword")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<int>("SmtpPort")
                        .HasAnnotation("SqlServer:ColumnType", "int")
                        .HasAnnotation("SqlServer:DefaultValue", "25")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("SmtpPreferredEncoding")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<bool>("SmtpRequiresAuth")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("SmtpServer")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<bool>("SmtpUseSsl")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("SmtpUser")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<string>("TimeZoneId")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("TwitterConsumerKey")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("TwitterConsumerSecret")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("UseEmailForLogin")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "1")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("UseLdapAuth")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("UseSecureRegistration")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("UseSslOnAllPages")
                        .HasAnnotation("Relational:ColumnName", "UseSSLOnAllPages")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.HasKey("SiteId");

                    b.HasIndex("SiteGuid")
                        .IsUnique();

                    b.HasAnnotation("Relational:TableName", "mp_Sites");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteUser", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnName", "UserID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AccountApproved");

                    b.Property<string>("AuthorBio");

                    b.Property<string>("AvatarUrl");

                    b.Property<string>("Comment");

                    b.Property<string>("Country");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<bool>("DisplayInMemberList");

                    b.Property<string>("DisplayName");

                    b.Property<string>("Email");

                    b.Property<Guid>("EmailChangeGuid");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int>("FailedPasswordAnswerAttemptCount");

                    b.Property<DateTime>("FailedPasswordAnswerAttemptWindowStart");

                    b.Property<int>("FailedPasswordAttemptCount");

                    b.Property<DateTime>("FailedPasswordAttemptWindowStart");

                    b.Property<string>("FirstName");

                    b.Property<string>("Gender");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsLockedOut");

                    b.Property<DateTime>("LastActivityDate");

                    b.Property<DateTime>("LastLockoutDate");

                    b.Property<DateTime>("LastLoginDate");

                    b.Property<string>("LastName");

                    b.Property<DateTime>("LastPasswordChangedDate");

                    b.Property<DateTime?>("LockoutEndDateUtc");

                    b.Property<string>("LoweredEmail");

                    b.Property<bool>("MustChangePwd");

                    b.Property<string>("NewEmail");

                    b.Property<string>("PasswordHash");

                    b.Property<Guid>("PasswordResetGuid");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<Guid>("RegisterConfirmGuid");

                    b.Property<bool>("RolesChanged");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Signature");

                    b.Property<Guid>("SiteGuid")
                        .HasAnnotation("Relational:ColumnName", "SiteGuid")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<int>("SiteId")
                        .HasAnnotation("Relational:ColumnName", "SiteID")
                        .HasAnnotation("SqlServer:ColumnType", "int");

                    b.Property<string>("State");

                    b.Property<string>("TimeZoneId");

                    b.Property<bool>("Trusted");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<Guid>("UserGuid")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<string>("UserName");

                    b.Property<string>("WebSiteUrl");

                    b.HasKey("UserId");

                    b.HasIndex("SiteGuid");

                    b.HasIndex("SiteId");

                    b.HasIndex("UserGuid")
                        .IsUnique();

                    b.HasAnnotation("Relational:TableName", "mp_Users");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("SiteId")
                        .HasAnnotation("Relational:ColumnName", "SiteId")
                        .HasAnnotation("SqlServer:ColumnType", "int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.HasAnnotation("Relational:TableName", "mp_UserClaims");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.UserLocation", b =>
                {
                    b.Property<Guid>("RowId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnName", "RowID")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<int>("CaptureCount");

                    b.Property<string>("City")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Continent")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Country")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<DateTime>("FirstCaptureUtc")
                        .HasAnnotation("Relational:ColumnName", "FirstCaptureUTC");

                    b.Property<string>("HostName");

                    b.Property<string>("IpAddress")
                        .HasAnnotation("MaxLength", 50)
                        .HasAnnotation("Relational:ColumnName", "IPAddress");

                    b.Property<long>("IpAddressLong")
                        .HasAnnotation("Relational:ColumnName", "IPAddressLong");

                    b.Property<string>("Isp")
                        .HasAnnotation("MaxLength", 255)
                        .HasAnnotation("Relational:ColumnName", "ISP");

                    b.Property<DateTime>("LastCaptureUtc")
                        .HasAnnotation("Relational:ColumnName", "LastCaptureUTC");

                    b.Property<float>("Latitude");

                    b.Property<float>("Longitude");

                    b.Property<string>("Region")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("SiteGuid")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<string>("TimeZone")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("UserGuid")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.HasKey("RowId");

                    b.HasAnnotation("Relational:TableName", "mp_UserLocation");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.UserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("ProviderKey")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("UserId")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("ProviderDisplayName")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<int>("SiteId")
                        .HasAnnotation("Relational:ColumnName", "SiteId")
                        .HasAnnotation("SqlServer:ColumnType", "int");

                    b.HasKey("LoginProvider", "ProviderKey", "UserId");

                    b.HasIndex("SiteId");

                    b.HasAnnotation("Relational:TableName", "mp_UserLogins");
                });

            modelBuilder.Entity("cloudscribe.Core.Repositories.EF.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnName", "ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("RoleGuid")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<int>("RoleId")
                        .HasAnnotation("Relational:ColumnName", "RoleID")
                        .HasAnnotation("SqlServer:ColumnType", "int");

                    b.Property<Guid>("UserGuid")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<int>("UserId")
                        .HasAnnotation("Relational:ColumnName", "UserID")
                        .HasAnnotation("SqlServer:ColumnType", "int");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "mp_UserRoles");
                });
        }
    }
}
