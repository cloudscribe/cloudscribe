using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using cloudscribe.Core.Storage.EFCore.MySql;

namespace cloudscribe.Core.Storage.EFCore.MySql.Migrations
{
    [DbContext(typeof(CoreDbContext))]
    [Migration("20161111133959_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("cloudscribe.Core.Models.Geography.GeoCountry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

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

                    b.ToTable("cs_GeoCountry");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.Geography.GeoZone", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("CountryId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.HasIndex("Code");

                    b.HasIndex("CountryId");

                    b.ToTable("cs_GeoZone");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteHost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HostName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("SiteId");

                    b.HasKey("Id");

                    b.HasIndex("HostName");

                    b.HasIndex("SiteId");

                    b.ToTable("cs_SiteHost");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NormalizedRoleName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<Guid>("SiteId");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("NormalizedRoleName");

                    b.HasIndex("SiteId");

                    b.ToTable("cs_Role");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountApprovalEmailCsv");

                    b.Property<string>("AddThisDotComUsername")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("AliasId")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<bool>("AllowDbFallbackWithLdap");

                    b.Property<bool>("AllowNewRegistration");

                    b.Property<bool>("AllowPersistentLogin");

                    b.Property<bool>("AutoCreateLdapUserOnFirstLogin");

                    b.Property<bool>("CaptchaOnLogin");

                    b.Property<bool>("CaptchaOnRegistration");

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

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<string>("DefaultEmailFromAddress")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("DefaultEmailFromAlias")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("DisableDbAuth");

                    b.Property<string>("DkimDomain")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("DkimPrivateKey");

                    b.Property<string>("DkimPublicKey");

                    b.Property<string>("DkimSelector")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<bool>("EmailLdapDbFallback");

                    b.Property<string>("FacebookAppId")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("FacebookAppSecret");

                    b.Property<string>("GoogleAnalyticsProfileId")
                        .HasAnnotation("MaxLength", 25);

                    b.Property<string>("GoogleClientId")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("GoogleClientSecret");

                    b.Property<bool>("IsDataProtected");

                    b.Property<bool>("IsServerAdminSite");

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

                    b.Property<bool>("ReallyDeleteUsers");

                    b.Property<string>("RecaptchaPrivateKey")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("RecaptchaPublicKey")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("RegistrationAgreement");

                    b.Property<string>("RegistrationPreamble");

                    b.Property<bool>("RequireApprovalBeforeLogin");

                    b.Property<bool>("RequireConfirmedEmail");

                    b.Property<bool>("RequireConfirmedPhone");

                    b.Property<bool>("RequiresQuestionAndAnswer");

                    b.Property<bool>("SignEmailWithDkim");

                    b.Property<string>("SiteFolderName")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("SiteIsClosed");

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

                    b.Property<bool>("SmtpRequiresAuth");

                    b.Property<string>("SmtpServer")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<bool>("SmtpUseSsl");

                    b.Property<string>("SmtpUser")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<string>("Theme")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("TimeZoneId")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("TwitterConsumerKey")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("TwitterConsumerSecret");

                    b.Property<bool>("UseEmailForLogin");

                    b.Property<bool>("UseLdapAuth");

                    b.HasKey("Id");

                    b.HasIndex("AliasId");

                    b.HasIndex("SiteFolderName");

                    b.ToTable("cs_Site");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.SiteUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<bool>("AccountApproved");

                    b.Property<string>("AuthorBio");

                    b.Property<string>("AvatarUrl")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<bool>("CanAutoLockout");

                    b.Property<string>("Comment");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<bool>("DisplayInMemberList");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Gender");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsLockedOut");

                    b.Property<DateTime?>("LastLoginUtc");

                    b.Property<DateTime>("LastModifiedUtc");

                    b.Property<string>("LastName")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<DateTime?>("LastPasswordChangeUtc");

                    b.Property<DateTime?>("LockoutEndDateUtc");

                    b.Property<bool>("MustChangePwd");

                    b.Property<string>("NewEmail")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("NewEmailApproved");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<bool>("RolesChanged");

                    b.Property<string>("SecurityStamp")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("Signature");

                    b.Property<Guid>("SiteId");

                    b.Property<string>("TimeZoneId")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("Trusted");

                    b.Property<bool>("TwoFactorEnabled");

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

                    b.ToTable("cs_User");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.UserClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("SiteId");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ClaimType");

                    b.HasIndex("SiteId");

                    b.HasIndex("UserId");

                    b.ToTable("cs_UserClaim");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.UserLocation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

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

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Region")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("SiteId");

                    b.Property<string>("TimeZone")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("IpAddress");

                    b.HasIndex("UserId");

                    b.ToTable("cs_UserLocation");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.UserLogin", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("SiteId");

                    b.Property<string>("LoginProvider")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("ProviderKey")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("ProviderDisplayName")
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("UserId", "SiteId", "LoginProvider", "ProviderKey");

                    b.HasIndex("SiteId");

                    b.HasIndex("UserId");

                    b.ToTable("cs_UserLogin");
                });

            modelBuilder.Entity("cloudscribe.Core.Models.UserRole", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("cs_UserRole");
                });
        }
    }
}
