using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using cloudscribe.Core.Storage.EF;

namespace cloudscribe.Core.Storage.EF.Migrations
{
    [DbContext(typeof(CoreDbContext))]
    [Migration("20160515164851_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("cloudscribe.Core.Models.Geography.Currency", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "cs_Currency");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.Geography.GeoCountry", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.HasKey("Id");

                    b.HasIndex("ISOCode2");

                    b.HasAnnotation("Relational:TableName", "cs_GeoCountry");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.Geography.GeoZone", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("CountryId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasAnnotation("Relational:TableName", "cs_GeoZone");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.Geography.Language", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "cs_Language");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteHost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<string>("HostName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("SiteId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("HostName");

                    b.HasIndex("SiteId");

                    b.HasAnnotation("Relational:TableName", "cs_SiteHost");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("NormalizedRoleName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<Guid>("SiteId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("NormalizedRoleName");

                    b.HasIndex("SiteId");

                    b.HasAnnotation("Relational:TableName", "cs_Role");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<string>("AccountApprovalEmailCsv");

                    b.Property<string>("AddThisDotComUsername")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("AliasId")
                        .HasAnnotation("MaxLength", 36);

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

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedUtc")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "datetime")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "getutcdate()");

                    b.Property<string>("DefaultEmailFromAddress")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("DefaultEmailFromAlias")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("DisableDbAuth")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("DkimDomain")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("DkimPrivateKey");

                    b.Property<string>("DkimPublicKey");

                    b.Property<string>("DkimSelector")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<bool>("EmailLdapDbFallback")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("FacebookAppId")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("FacebookAppSecret");

                    b.Property<string>("GoogleAnalyticsProfileId")
                        .HasAnnotation("MaxLength", 25);

                    b.Property<string>("GoogleClientId")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("GoogleClientSecret");

                    b.Property<bool>("IsDataProtected")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("IsServerAdminSite")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

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

                    b.Property<string>("MicrosoftClientSecret");

                    b.Property<int>("MinRequiredPasswordLength")
                        .HasAnnotation("SqlServer:ColumnType", "int")
                        .HasAnnotation("SqlServer:DefaultValue", "5")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("OidConnectAppId")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("OidConnectAppSecret");

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

                    b.Property<bool>("RequireConfirmedEmail")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("RequireConfirmedPhone")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("RequiresQuestionAndAnswer")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("SignEmailWithDkim")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("SiteFolderName")
                        .HasAnnotation("MaxLength", 50)
                        .HasAnnotation("Relational:DefaultValue", "")
                        .HasAnnotation("Relational:DefaultValueType", "System.String");

                    b.Property<bool>("SiteIsClosed")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("SiteIsClosedMessage");

                    b.Property<string>("SiteName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("SmsClientId")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("SmsFrom")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("SmsSecureToken");

                    b.Property<string>("SmtpPassword");

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

                    b.Property<string>("Theme")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("TimeZoneId")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("TwitterConsumerKey")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("TwitterConsumerSecret");

                    b.Property<bool>("UseEmailForLogin")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "1")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("UseLdapAuth")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.HasKey("Id");

                    b.HasIndex("AliasId");

                    b.HasIndex("SiteFolderName");

                    b.HasAnnotation("Relational:TableName", "cs_Site");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<int>("AccessFailedCount");

                    b.Property<bool>("AccountApproved")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "1")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("AuthorBio");

                    b.Property<string>("AvatarUrl")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<bool>("CanAutoLockout")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "1")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("Comment");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Country");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<bool>("DisplayInMemberList")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "1")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("DisplayName");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("EmailConfirmed")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("FirstName");

                    b.Property<string>("Gender");

                    b.Property<bool>("IsDeleted")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("IsLockedOut")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<DateTime?>("LastLoginDate");

                    b.Property<string>("LastName");

                    b.Property<DateTime?>("LastPasswordChangedDate");

                    b.Property<DateTime?>("LockoutEndDateUtc");

                    b.Property<bool>("MustChangePwd")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("NewEmail");

                    b.Property<bool>("NewEmailApproved")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<bool>("RolesChanged")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Signature");

                    b.Property<Guid>("SiteId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<string>("State");

                    b.Property<string>("TimeZoneId");

                    b.Property<bool>("Trusted");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", "0")
                        .HasAnnotation("SqlServer:DefaultValueType", "System.Int32");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("WebSiteUrl")
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail");

                    b.HasIndex("NormalizedUserName");

                    b.HasIndex("SiteId");

                    b.HasAnnotation("Relational:TableName", "cs_User");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.UserClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<string>("ClaimType")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("SiteId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ClaimType");

                    b.HasIndex("SiteId");

                    b.HasIndex("UserId");

                    b.HasAnnotation("Relational:TableName", "cs_UserClaim");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.UserLocation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<int>("CaptureCount");

                    b.Property<string>("City")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Continent")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Country")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<DateTime>("FirstCaptureUtc");

                    b.Property<string>("HostName")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("IpAddress")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<long>("IpAddressLong");

                    b.Property<string>("Isp")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<DateTime>("LastCaptureUtc");

                    b.Property<double>("Latitude")
                        .HasAnnotation("SqlServer:ColumnType", "float");

                    b.Property<double>("Longitude")
                        .HasAnnotation("SqlServer:ColumnType", "float");

                    b.Property<string>("Region")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("SiteId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<string>("TimeZone")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("UserId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasAnnotation("Relational:TableName", "cs_UserLocation");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.UserLogin", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<Guid>("SiteId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("ProviderKey")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("ProviderDisplayName")
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("UserId", "SiteId", "LoginProvider", "ProviderKey");

                    b.HasIndex("SiteId");

                    b.HasIndex("UserId");

                    b.HasAnnotation("Relational:TableName", "cs_UserLogin");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.HasAnnotation("Relational:TableName", "cs_UserRole");
                });
        }
    }
}
