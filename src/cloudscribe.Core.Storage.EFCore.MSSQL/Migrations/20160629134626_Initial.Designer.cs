using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using cloudscribe.Core.Storage.EFCore.MSSQL;

namespace cloudscribe.Core.Storage.EFCore.MSSQL.Migrations
{
    [DbContext(typeof(CoreDbContext))]
    [Migration("20160629134626_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("cloudscribe.Core.Models.Geography.GeoCountry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

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

                    b.ToTable("Countries");

                    b.HasAnnotation("SqlServer:TableName", "cs_GeoCountry");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.Geography.GeoZone", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("CountryId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.HasIndex("Code");

                    b.HasIndex("CountryId");

                    b.ToTable("States");

                    b.HasAnnotation("SqlServer:TableName", "cs_GeoZone");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteHost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<string>("HostName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("SiteId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("HostName");

                    b.HasIndex("SiteId");

                    b.ToTable("SiteHosts");

                    b.HasAnnotation("SqlServer:TableName", "cs_SiteHost");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

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

                    b.ToTable("Roles");

                    b.HasAnnotation("SqlServer:TableName", "cs_Role");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<string>("AccountApprovalEmailCsv");

                    b.Property<string>("AddThisDotComUsername")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("AliasId")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<bool>("AllowDbFallbackWithLdap")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<bool>("AllowNewRegistration")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", true);

                    b.Property<bool>("AllowPersistentLogin")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<bool>("AutoCreateLdapUserOnFirstLogin")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", true);

                    b.Property<bool>("CaptchaOnLogin")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<bool>("CaptchaOnRegistration")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

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

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<DateTime>("CreatedUtc")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "datetime")
                        .HasAnnotation("SqlServer:DefaultValueSql", "getutcdate()");

                    b.Property<string>("DefaultEmailFromAddress")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("DefaultEmailFromAlias")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("DisableDbAuth")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("DkimDomain")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("DkimPrivateKey");

                    b.Property<string>("DkimPublicKey");

                    b.Property<string>("DkimSelector")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<bool>("EmailLdapDbFallback")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("FacebookAppId")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("FacebookAppSecret");

                    b.Property<string>("GoogleAnalyticsProfileId")
                        .HasAnnotation("MaxLength", 25);

                    b.Property<string>("GoogleClientId")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("GoogleClientSecret");

                    b.Property<bool>("IsDataProtected")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<bool>("IsServerAdminSite")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("LdapDomain")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("LdapPort");

                    b.Property<string>("LdapRootDN")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("LdapServer")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("LdapUserDNKey")
                        .HasAnnotation("MaxLength", 10);

                    b.Property<string>("LoginInfoBottom");

                    b.Property<string>("LoginInfoTop");

                    b.Property<int>("MaxInvalidPasswordAttempts");

                    b.Property<string>("MicrosoftClientId")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("MicrosoftClientSecret");

                    b.Property<int>("MinRequiredPasswordLength");

                    b.Property<string>("OidConnectAppId")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("OidConnectAppSecret");

                    b.Property<string>("PreferredHostName")
                        .HasAnnotation("MaxLength", 250);

                    b.Property<string>("PrivacyPolicy");

                    b.Property<bool>("ReallyDeleteUsers")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", true);

                    b.Property<string>("RecaptchaPrivateKey")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("RecaptchaPublicKey")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("RegistrationAgreement");

                    b.Property<string>("RegistrationPreamble");

                    b.Property<bool>("RequireApprovalBeforeLogin")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<bool>("RequireConfirmedEmail")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<bool>("RequireConfirmedPhone")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<bool>("RequiresQuestionAndAnswer")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<bool>("SignEmailWithDkim")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("SiteFolderName")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("SiteIsClosed")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

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

                    b.Property<int>("SmtpPort");

                    b.Property<string>("SmtpPreferredEncoding")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<bool>("SmtpRequiresAuth")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("SmtpServer")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<bool>("SmtpUseSsl")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

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
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", true);

                    b.Property<bool>("UseLdapAuth")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.HasKey("Id");

                    b.HasIndex("AliasId");

                    b.HasIndex("SiteFolderName");

                    b.ToTable("Sites");

                    b.HasAnnotation("SqlServer:TableName", "cs_Site");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<int>("AccessFailedCount");

                    b.Property<bool>("AccountApproved")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("AuthorBio");

                    b.Property<string>("AvatarUrl")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<bool>("CanAutoLockout")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", true);

                    b.Property<string>("Comment");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<bool>("DisplayInMemberList")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("EmailConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("FirstName")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Gender");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<bool>("IsLockedOut")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<DateTime?>("LastLoginUtc");

                    b.Property<DateTime>("LastModifiedUtc");

                    b.Property<string>("LastName")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<DateTime?>("LastPasswordChangeUtc");

                    b.Property<DateTime?>("LockoutEndDateUtc");

                    b.Property<bool>("MustChangePwd")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("NewEmail")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("NewEmailApproved")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

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
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<bool>("RolesChanged")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("SecurityStamp")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("Signature");

                    b.Property<Guid>("SiteId")
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier");

                    b.Property<string>("TimeZoneId")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("Trusted");

                    b.Property<bool>("TwoFactorEnabled")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("WebSiteUrl")
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("Id");

                    b.HasIndex("DisplayName");

                    b.HasIndex("NormalizedEmail");

                    b.HasIndex("NormalizedUserName");

                    b.HasIndex("SiteId");

                    b.ToTable("Users");

                    b.HasAnnotation("SqlServer:TableName", "cs_User");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.UserClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

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

                    b.ToTable("UserClaims");

                    b.HasAnnotation("SqlServer:TableName", "cs_UserClaim");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.UserLocation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

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

                    b.HasIndex("IpAddress");

                    b.HasIndex("UserId");

                    b.ToTable("UserLocations");

                    b.HasAnnotation("SqlServer:TableName", "cs_UserLocation");
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

                    b.ToTable("UserLogins");

                    b.HasAnnotation("SqlServer:TableName", "cs_UserLogin");
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

                    b.ToTable("UserRoles");

                    b.HasAnnotation("SqlServer:TableName", "cs_UserRole");
                });
        }
    }
}
