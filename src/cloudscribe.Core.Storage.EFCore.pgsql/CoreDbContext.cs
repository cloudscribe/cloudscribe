// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2017-06-29
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Storage.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL;

//http://ef.readthedocs.org/en/latest/modeling/configuring.html
// "If you are targeting more than one relational provider with the same model then you 
// probably want to specify a data type for each provider rather than a global one to be used for 
// all relational providers."

// ie this:
// modelBuilder.Entity<Blog>()
//         .Property(b => b.Url)
//         .HasSqlServerColumnType("varchar(200)");  //sql server specific

// rather than this:

// modelBuilder.Entity<Blog>()
//          .Property(b => b.Url)
//          .HasColumnType("varchar(200)");
//
// https://github.com/aspnet/EntityFramework/wiki/Configuring-a-DbContext

namespace cloudscribe.Core.Storage.EFCore.pgsql
{
    public class CoreDbContext : CoreDbContextBase, ICoreDbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ICoreTableNames tableNames = this.GetService<ICoreTableNames>();
            if (tableNames == null)
            {
                tableNames = new CoreTableNames();
            }
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<SiteSettings>(entity =>
            {
                entity.ForNpgsqlToTable(tableNames.TablePrefix + tableNames.SiteTableName);
                
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                
                //.ForSqlServerHasColumnType("uniqueidentifier")
                //.ForSqlServerHasDefaultValueSql("newid()")
                ;

                entity.Property(p => p.AliasId)
                .HasMaxLength(36)
                ;

                //entity.Property(u => u.ConcurrencyStamp)
                //    .IsConcurrencyToken()
                //    .HasMaxLength(50)
                //    ;

                entity.HasIndex(p => p.AliasId);

                entity.Property(p => p.SiteName)
                .HasMaxLength(255)
                .IsRequired();

                entity.Property(p => p.Theme)
                .HasMaxLength(100);

                entity.Property(p => p.AllowNewRegistration)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(true)
                ;

                entity.Property(p => p.RequireConfirmedEmail)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.RequireConfirmedPhone)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.IsServerAdminSite)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.UseLdapAuth)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.AutoCreateLdapUserOnFirstLogin)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
               // .ForSqlServerHasDefaultValue(true)
                ;

                entity.Property(p => p.LdapServer)
                .HasMaxLength(255);
                ;

                entity.Property(p => p.LdapPort)
                //.IsRequired()
                //.ForSqlServerHasColumnType("int")
                // .HasDefaultValue(389)
                //.ValueGeneratedNever()
                ;

                entity.Property(p => p.LdapDomain)
                .HasMaxLength(255);
                ;

                entity.Property(p => p.LdapRootDN)
                .HasMaxLength(255);
                ;

                entity.Property(p => p.LdapUserDNKey)
                .HasMaxLength(10);
                ;

                entity.Property(p => p.ReallyDeleteUsers)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
               // .ForSqlServerHasDefaultValue(true)
                ;

                entity.Property(p => p.UseEmailForLogin)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(true)
                ;

                entity.Property(p => p.RequiresQuestionAndAnswer)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.MaxInvalidPasswordAttempts)
                //.IsRequired()
                //.ForSqlServerHasColumnType("int")
                //.ValueGeneratedNever()
                // .HasDefaultValue(5)
                ;


                entity.Property(p => p.MinRequiredPasswordLength)
                // .IsRequired()
                // .ForSqlServerHasColumnType("int")
                //.HasDefaultValue(5)
                //.ValueGeneratedNever()
                ;

                entity.Property(p => p.DefaultEmailFromAddress)
                .HasMaxLength(100);
                ;

                entity.Property(p => p.DefaultEmailFromAlias)
                .HasMaxLength(100);
                ;

                entity.Property(p => p.RecaptchaPrivateKey)
                .HasMaxLength(255);
                ;

                entity.Property(p => p.RecaptchaPublicKey)
                .HasMaxLength(255);
                ;

                entity.Property(p => p.UseInvisibleRecaptcha)
               .IsRequired()
               //.ForSqlServerHasColumnType("bit")
               //.ForSqlServerHasDefaultValue(false)
               ;

                entity.Property(p => p.DisableDbAuth)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.RequireApprovalBeforeLogin)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.AllowDbFallbackWithLdap)
                .IsRequired()
               // .ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.EmailLdapDbFallback)
                .IsRequired()
               // .ForSqlServerHasColumnType("bit")
               // .ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.AllowPersistentLogin)
                .IsRequired()
               // .ForSqlServerHasColumnType("bit")
               // .ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.CaptchaOnLogin)
                .IsRequired()
               // .ForSqlServerHasColumnType("bit")
               // .ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.CaptchaOnRegistration)
                .IsRequired()
               // .ForSqlServerHasColumnType("bit")
               // .ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.SiteIsClosed)
                .IsRequired()
               // .ForSqlServerHasColumnType("bit")
               // .ForSqlServerHasDefaultValue(false)
                ;

                //not mapped should map to nvarchar(max) by default I think
                // SiteIsClosedMessage 
                // PrivacyPolicy

                entity.Property(p => p.TimeZoneId)
                .HasMaxLength(50);
                ;

                entity.Property(p => p.GoogleAnalyticsProfileId)
                .HasMaxLength(25);
                ;

                entity.Property(p => p.CompanyName)
                .HasMaxLength(250);
                ;

                entity.Property(p => p.CompanyStreetAddress)
                .HasMaxLength(250);
                ;

                entity.Property(p => p.CompanyStreetAddress2)
                .HasMaxLength(250);
                ;

                entity.Property(p => p.CompanyRegion)
                .HasMaxLength(200);
                ;

                entity.Property(p => p.CompanyLocality)
                .HasMaxLength(200);
                ;

                entity.Property(p => p.CompanyCountry)
                .HasMaxLength(10);
                ;

                entity.Property(p => p.CompanyPostalCode)
                .HasMaxLength(20);
                ;

                entity.Property(p => p.CompanyPublicEmail)
                .HasMaxLength(100);
                ;

                entity.Property(p => p.CompanyPhone)
                .HasMaxLength(20);
                ;

                entity.Property(p => p.CompanyFax)
                .HasMaxLength(20);
                ;

                entity.Property(p => p.FacebookAppId)
                .HasMaxLength(100);
                ;

                entity.Property(p => p.FacebookAppSecret)
                ;

                entity.Property(p => p.GoogleClientId)
                .HasMaxLength(100);
                ;

                entity.Property(p => p.GoogleClientSecret)
                ;

                entity.Property(p => p.TwitterConsumerKey)
                .HasMaxLength(100);
                ;

                entity.Property(p => p.TwitterConsumerSecret)
                ;

                entity.Property(p => p.MicrosoftClientId)
                .HasMaxLength(100);
                ;

                entity.Property(p => p.MicrosoftClientSecret)
                ;

                entity.Property(p => p.OidConnectAppId)
               .HasMaxLength(255);
                ;

                entity.Property(p => p.OidConnectAppSecret)
               .HasMaxLength(255);
                ;

                entity.Property(p => p.OidConnectAuthority)
               .HasMaxLength(255);
                ;

                entity.Property(p => p.OidConnectDisplayName)
               .HasMaxLength(150);
                ;

                entity.Property(p => p.PreferredHostName)
                .HasMaxLength(250);
                ;

                entity.Property(p => p.SiteFolderName)
                .HasMaxLength(50)
                .HasDefaultValue(string.Empty)
                ;

                entity.HasIndex(p => p.SiteFolderName);

                entity.Property(p => p.AddThisDotComUsername)
                .HasMaxLength(50);
                ;

                //not mapped should map to ntext by default I think
                // LoginInfoTop 
                // LoginInfoBottom
                // RegistrationAgreement
                // RegistrationPreamble

                entity.Property(p => p.SmtpServer)
                .HasMaxLength(200);
                ;

                entity.Property(p => p.SmtpPort)
                //.IsRequired()
                //.ForSqlServerHasColumnType("int")
                //.HasDefaultValue(25)
                //.ValueGeneratedNever()
                ;

                entity.Property(p => p.SmtpUser)
                .HasMaxLength(500); // large so it can be encrypted
                ;

                entity.Property(p => p.SmtpPassword)
                ;

                entity.Property(p => p.SmtpPreferredEncoding)
                .HasMaxLength(20);
                ;

                entity.Property(p => p.SmtpRequiresAuth)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.SmtpUseSsl)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.DkimDomain)
                .HasMaxLength(255);
                ;

                entity.Property(p => p.DkimSelector)
                .HasMaxLength(128);
                ;

                entity.Property(p => p.SignEmailWithDkim)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.SmsClientId)
                .HasMaxLength(255);
                ;

                entity.Property(p => p.SmsFrom)
                .HasMaxLength(100);
                ;

                entity.Property(p => p.IsDataProtected)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.CreatedUtc)
                //.ForSqlServerHasColumnType("datetime")
                //.ForSqlServerHasDefaultValueSql("getutcdate()")
                ;

                entity.Property(p => p.ForcedCulture)
                .HasMaxLength(10);
                ;

                entity.Property(p => p.ForcedUICulture)
                .HasMaxLength(10);
                ;
            });

            modelBuilder.Entity<SiteHost>(entity =>
            {
                entity.ForNpgsqlToTable(tableNames.TablePrefix + tableNames.SiteHostTableName);

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
               //.ForSqlServerHasColumnType("uniqueidentifier")
               //.ForSqlServerHasDefaultValueSql("newid()")
               ;

                entity.Property(p => p.SiteId)
                //.ForSqlServerHasColumnType("uniqueidentifier")
                .IsRequired()
                ;
                entity.HasIndex(p => p.SiteId);

                entity.Property(p => p.HostName)
                .IsRequired()
                .HasMaxLength(255);
                ;

                entity.HasIndex(p => p.HostName);
            });
            
            modelBuilder.Entity<SiteUser>(entity =>
            {
                entity.ForNpgsqlToTable(tableNames.TablePrefix + tableNames.UserTableName);

                entity.Property(p => p.Id)
                   //.ForSqlServerHasColumnType("uniqueidentifier")
                   //.ForSqlServerHasDefaultValueSql("newid()")
                   .IsRequired()
                   ;

                entity.HasKey(p => p.Id);

                entity.Property(p => p.SiteId)
                    //.ForSqlServerHasColumnType("uniqueidentifier")
                    .IsRequired()
                    ;

                entity.HasIndex(p => p.SiteId);

                //entity.Property(u => u.ConcurrencyStamp)
                //    .IsConcurrencyToken()
                //    .HasMaxLength(50)
                //    ;

                entity.Property(p => p.AccountApproved)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
               // .ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.DisplayInMemberList)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(100)
                ;

                entity.Property(p => p.NormalizedEmail)
                .IsRequired()
                .HasMaxLength(100)
                ;

                entity.Property(p => p.NewEmail)
                .HasMaxLength(100)
                ;

                entity.HasIndex(p => p.NormalizedEmail);

                entity.Property(p => p.UserName)
                .IsRequired()
                .HasMaxLength(50)
                ;

                entity.Property(p => p.NormalizedUserName)
                .IsRequired()
                .HasMaxLength(50)
                ;
                entity.HasIndex(p => p.NormalizedUserName);

                entity.Property(p => p.DisplayName)
                .IsRequired()
                .HasMaxLength(100);

                entity.HasIndex(p => p.DisplayName);

                entity.Property(p => p.FirstName)
                .HasMaxLength(100);

                entity.Property(p => p.LastName)
                .HasMaxLength(100);

                entity.Property(p => p.EmailConfirmed)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.IsDeleted)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.IsLockedOut)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.MustChangePwd)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.NewEmailApproved)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
               // .ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.RolesChanged)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.TwoFactorEnabled)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.PhoneNumber)
               .HasMaxLength(50)
               ;

                entity.Property(p => p.PhoneNumberConfirmed)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
                //.ForSqlServerHasDefaultValue(false)
                ;

                entity.Property(p => p.CanAutoLockout)
                .IsRequired()
                //.ForSqlServerHasColumnType("bit")
               // .ForSqlServerHasDefaultValue(true)
                ;

                entity.Property(p => p.AvatarUrl)
                .HasMaxLength(255)
                ;

                entity.Property(p => p.WebSiteUrl)
                .HasMaxLength(100)
                ;

                entity.Property(p => p.SecurityStamp)
                .HasMaxLength(50)
                ;

                entity.Property(p => p.TimeZoneId)
                .HasMaxLength(50)
                ;
            });

            modelBuilder.Entity<SiteRole>(entity =>
            {
                entity.Ignore(x => x.MemberCount);

                entity.ForNpgsqlToTable(tableNames.TablePrefix + tableNames.RoleTableName);

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                   //.ForSqlServerHasColumnType("uniqueidentifier")
                  // .ForSqlServerHasDefaultValueSql("newid()")
                   .IsRequired()
                   ;

                entity.HasIndex(p => p.Id)
                .IsUnique();

                entity.Property(p => p.SiteId)
                  // .ForSqlServerHasColumnType("uniqueidentifier")
                   .IsRequired()
                   ;

                entity.HasIndex(p => p.SiteId);

                //entity.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                entity.Property(p => p.NormalizedRoleName)
                .IsRequired()
                .HasMaxLength(50);
                ;

                entity.HasIndex(p => p.NormalizedRoleName);

                entity.Property(p => p.RoleName)
                .IsRequired()
                .HasMaxLength(50);
                ;


            });

            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.ForNpgsqlToTable(tableNames.TablePrefix + tableNames.UserClaimTableName);
                
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                  // .ForSqlServerHasColumnType("uniqueidentifier")
                 //  .ForSqlServerHasDefaultValueSql("newid()")
                   .IsRequired()
                ;

                entity.Property(p => p.UserId)
                //.ForSqlServerHasColumnType("uniqueidentifier")
                .IsRequired()
                ;

                entity.HasIndex(p => p.UserId);

                entity.Property(p => p.SiteId)
                 //.ForSqlServerHasColumnType("uniqueidentifier")
                 .IsRequired()
                 ;

                entity.HasIndex(p => p.SiteId);

                entity.Property(p => p.ClaimType)
                .HasMaxLength(255)
                ;

                entity.HasIndex(p => p.ClaimType);
                // should we limit claim value?



                // not mapped will result in nvarchar(max) I think
                //ClaimType
                //ClaimValue
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.ForNpgsqlToTable(tableNames.TablePrefix + tableNames.UserLoginTableName);


                entity.HasKey(p => new { p.UserId, p.SiteId, p.LoginProvider, p.ProviderKey });

                entity.Property(p => p.LoginProvider)
                .HasMaxLength(128)
                ;

                entity.Property(p => p.ProviderKey)
                .HasMaxLength(128)
                ;

                entity.Property(p => p.UserId)
               // .ForSqlServerHasColumnType("uniqueidentifier")
                .IsRequired()
                ;

                entity.HasIndex(p => p.UserId);

                entity.Property(p => p.SiteId)
                //.ForSqlServerHasColumnType("uniqueidentifier")
                .IsRequired()
                ;

                entity.HasIndex(p => p.SiteId);

                entity.Property(p => p.ProviderDisplayName)
                .HasMaxLength(100)
                ;
            });

            modelBuilder.Entity<GeoCountry>(entity =>
            {
                entity.ForNpgsqlToTable(tableNames.TablePrefix + tableNames.GeoCountryTableName);

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                  // .ForSqlServerHasColumnType("uniqueidentifier")
                  // .ForSqlServerHasDefaultValueSql("newid()")
                   .IsRequired()
                   ;

                entity.Property(p => p.Name)
                .HasMaxLength(255)
                .IsRequired()
                ;

                entity.Property(p => p.ISOCode2)
                .HasMaxLength(2)
                .IsRequired()
                ;

                entity.HasIndex(p => p.ISOCode2);

                entity.Property(p => p.ISOCode3)
                .HasMaxLength(3)
                .IsRequired()
                ;
            });

            modelBuilder.Entity<GeoZone>(entity =>
            {
                entity.ForNpgsqlToTable(tableNames.TablePrefix + tableNames.GeoZoneTableName);

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                   //.ForSqlServerHasColumnType("uniqueidentifier")
                   //.ForSqlServerHasDefaultValueSql("newid()")
                   .IsRequired()
                   ;

                entity.Property(p => p.CountryId)
                   //.ForSqlServerHasColumnType("uniqueidentifier")
                   .IsRequired()
                   ;

                entity.HasIndex(p => p.CountryId);

                entity.Property(p => p.Name)
                .HasMaxLength(255)
                .IsRequired()
                ;

                entity.Property(p => p.Code)
                .HasMaxLength(255)
                .IsRequired()
                ;

                entity.HasIndex(p => p.Code);
            });
            
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ForNpgsqlToTable(tableNames.TablePrefix + tableNames.UserRoleTableName);

                entity.Property(p => p.UserId)
                //.ForSqlServerHasColumnType("uniqueidentifier")

                ;
                entity.HasIndex(p => p.UserId);

                entity.Property(p => p.RoleId)
                //.ForSqlServerHasColumnType("uniqueidentifier")

                ;
                entity.HasIndex(p => p.RoleId);

                entity.HasKey(p => new { p.UserId, p.RoleId });
            });
            
            modelBuilder.Entity<UserLocation>(entity =>
            {
                entity.ForNpgsqlToTable(tableNames.TablePrefix + tableNames.UserLocationTableName);

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                //.ForSqlServerHasColumnType("uniqueidentifier")
                //.ForSqlServerHasDefaultValueSql("newid()")
               ;

                entity.Property(p => p.UserId)
                //.ForSqlServerHasColumnType("uniqueidentifier")
                ;

                entity.Property(p => p.SiteId)
                //.ForSqlServerHasColumnType("uniqueidentifier")
                ;

                entity.Property(p => p.IpAddress)
                    .HasMaxLength(50)
                 ;

                entity.HasIndex(p => p.IpAddress);

                entity.Property(p => p.IpAddressLong)
                 ;

                entity.Property(p => p.Isp)
                    .HasMaxLength(255)
                 ;

                entity.Property(p => p.Continent)
                    .HasMaxLength(255)
                 ;

                entity.Property(p => p.Country)
                    .HasMaxLength(255)
                 ;

                entity.Property(p => p.Region)
                    .HasMaxLength(255)
                 ;

                entity.Property(p => p.City)
                    .HasMaxLength(255)
                 ;

                entity.Property(p => p.TimeZone)
                    .HasMaxLength(255)
                 ;

                entity.Property(p => p.FirstCaptureUtc)
                ;

                entity.Property(p => p.LastCaptureUtc)
                ;

                entity.Property(p => p.Latitude)
                   //.ForSqlServerHasColumnType("float")
                ;

                entity.Property(p => p.Longitude)
                  // .ForSqlServerHasColumnType("float")
                ;

                entity.Property(p => p.HostName)
                    .HasMaxLength(255)
                 ;

                entity.HasIndex(p => p.UserId);

                // goode idea or not?
                //entity.HasIndex(p => p.Latitude);
                //entity.HasIndex(p => p.Longitude);
            });

            // should this be called before or after we do our thing?

            base.OnModelCreating(modelBuilder);

        }

    }
}
