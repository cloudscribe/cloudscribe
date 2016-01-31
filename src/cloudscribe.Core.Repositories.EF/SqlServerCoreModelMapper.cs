// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-17
// Last Modified:			2015-12-27
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using Microsoft.Data.Entity.Scaffolding.Metadata;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;

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
// JA - since we do want to target more than one relational provider
// and we don't want to have dependencies on multiple providers in this project
// then we need to abstract the platform specific model mapping so the default one is for mssql
// but a different one can be injected
// that is why we are using ICoreModelMapper

namespace cloudscribe.Core.Repositories.EF
{
    /// <summary>
    /// To really support other dbplatfroms with EF it would require implementing this for other platforms
    /// and registering it with DI
    /// 
    /// https://github.com/aspnet/EntityFramework/wiki/Configuring-a-DbContext
    /// </summary>
    public class SqlServerCoreModelMapper : ICoreModelMapper
    {

        public void Map(EntityTypeBuilder<SiteSettings> entity)
        {
            // trying to keep table and column naming consistent so that
            // one could change from cloudscribe.Core.Repositories.MSSQL
            // to cloudscribe.Core.Repositories.EF or vice versa

            entity.ToTable("mp_Sites");
            entity.HasKey(p => p.SiteId);

            entity.Property(p => p.SiteId)
            //.HasSqlServerColumnType("int")
            .UseSqlServerIdentityColumn<int>()
            .HasColumnName("SiteID")
            .ValueGeneratedOnAdd()
            //.Metadata.SentinelValue = -1
            ;

            entity.Property(p => p.SiteGuid)
            .ForSqlServerHasColumnType("uniqueidentifier")
            .ForSqlServerHasDefaultValueSql("newid()")
            //.Metadata.SentinelValue = Guid.Empty
            ;

       

            entity.HasIndex(p => p.SiteGuid)  
            .IsUnique();

            entity.Property(p => p.SiteName)
            //.HasSqlServerColumnType("nvarchar(255)")
            .HasMaxLength(255)
            .IsRequired();

            entity.Property(p => p.Layout)
            //.HasSqlServerColumnType("nvarchar(100)")
            .HasColumnName("Skin")
            .HasMaxLength(100);

            entity.Property(p => p.AllowNewRegistration)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(1)
            ;

            entity.Property(p => p.RequireConfirmedEmail)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.RequireConfirmedPhone)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.IsServerAdminSite)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.UseLdapAuth)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.AutoCreateLdapUserOnFirstLogin)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(1)
            ;

            entity.Property(p => p.LdapServer)
            .HasMaxLength(255);
            ;

            entity.Property(p => p.LdapPort)
            .IsRequired()
            .ForSqlServerHasColumnType("int")
            .ForSqlServerHasDefaultValue(389)
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
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(1)
            ;

            entity.Property(p => p.UseEmailForLogin)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(1)
            ;

            
            entity.Property(p => p.RequiresQuestionAndAnswer)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.MaxInvalidPasswordAttempts)
            .IsRequired()
            .ForSqlServerHasColumnType("int")
            .ForSqlServerHasDefaultValue(5)
            ;

            
            entity.Property(p => p.MinRequiredPasswordLength)
            .IsRequired()
            .ForSqlServerHasColumnType("int")
            .ForSqlServerHasDefaultValue(5)
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

            
           

            entity.Property(p => p.DisableDbAuth)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.RequireApprovalBeforeLogin)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.AllowDbFallbackWithLdap)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.EmailLdapDbFallback)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.AllowPersistentLogin)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(1)
            ;

            entity.Property(p => p.CaptchaOnLogin)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.CaptchaOnRegistration)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.SiteIsClosed)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
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
            //.HasMaxLength(100);
            ;

            entity.Property(p => p.GoogleClientId)
            .HasMaxLength(100);
            ;

            entity.Property(p => p.GoogleClientSecret)
            //.HasMaxLength(100);
            ;

            entity.Property(p => p.TwitterConsumerKey)
            .HasMaxLength(100);
            ;

            entity.Property(p => p.TwitterConsumerSecret)
            //.HasMaxLength(100);
            ;

            entity.Property(p => p.MicrosoftClientId)
            .HasMaxLength(100);
            ;

            entity.Property(p => p.MicrosoftClientSecret)
            ;

            entity.Property(p => p.OidConnectAppId)
           .HasMaxLength(255);
            ;

            //OidConnectAppSecret nvarchar(max)

            entity.Property(p => p.PreferredHostName)
            .HasMaxLength(250);
            ;

            //entity.Metadata.Model.
            //entity.

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
            .IsRequired()
            .ForSqlServerHasColumnType("int")
            .ForSqlServerHasDefaultValue(25)
            ;

            entity.Property(p => p.SmtpUser)
            .HasMaxLength(500);
            ;

            entity.Property(p => p.SmtpPassword)
            //.HasMaxLength(500);
            ;

            entity.Property(p => p.SmtpPreferredEncoding)
            .HasMaxLength(20);
            ;

            entity.Property(p => p.SmtpRequiresAuth)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.SmtpUseSsl)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.DkimDomain)
            .HasMaxLength(255);
            ;

            entity.Property(p => p.DkimSelector)
            .HasMaxLength(128);
            ;

            entity.Property(p => p.SignEmailWithDkim)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.SmsClientId)
            .HasMaxLength(255);
            ;

            //SmsSecureToken nvarchar(max)

            entity.Property(p => p.SmsFrom)
            .HasMaxLength(100);
            ;

            entity.Property(p => p.IsDataProtected)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.CreatedUtc)
            .ForSqlServerHasColumnType("datetime")
            .ForSqlServerHasDefaultValueSql("getutcdate()")
            ;

        }

        public void Map(EntityTypeBuilder<SiteHost> entity)
        {
            entity.ToTable("mp_SiteHosts");
            entity.HasKey(p => p.HostId);

            entity.Property(p => p.HostId)
            .UseSqlServerIdentityColumn()
            .HasColumnName("HostID")
            //.Metadata.SentinelValue = -1
            ;

            entity.Property(p => p.SiteId)
            .HasColumnName("SiteID")
            .ForSqlServerHasColumnType("int")
            .IsRequired()
            ;

            entity.Property(p => p.SiteGuid)
            .HasColumnName("SiteGuid")
            .ForSqlServerHasColumnType("uniqueidentifier")
            .IsRequired()
            ;


            entity.Property(p => p.HostName)
            .IsRequired()
            .HasMaxLength(255);
            ;

        }

        //public void Map(EntityTypeBuilder<SiteFolder> entity)
        //{
        //    entity.ToTable("mp_SiteFolders");
        //    entity.HasKey(p => p.Guid);

        //    entity.Property(p => p.Guid)
        //       .ForSqlServerHasColumnType("uniqueidentifier")
        //       .ForSqlServerHasDefaultValueSql("newid()")
        //       .IsRequired()
        //       ;

        //    entity.Property(p => p.SiteGuid)
        //        .HasColumnName("SiteGuid")
        //        .ForSqlServerHasColumnType("uniqueidentifier")
        //        .IsRequired()
        //        ;

        //    entity.Property(p => p.FolderName)
        //    .IsRequired()
        //    .HasMaxLength(255);
        //    ;

        //}

        public void Map(EntityTypeBuilder<SiteUser> entity)
        {
            entity.ToTable("mp_Users");
            entity.HasKey(p => p.UserId);

            entity.Property(p => p.UserId)
            .UseSqlServerIdentityColumn<int>()
            .ValueGeneratedOnAdd()
            .HasColumnName("UserID")
           // .Metadata.SentinelValue = -1
            ;

            entity.Property(p => p.UserGuid)
               .ForSqlServerHasColumnType("uniqueidentifier")
               .ForSqlServerHasDefaultValueSql("newid()")
               .IsRequired()
               ;

            entity.HasIndex(p => p.UserGuid)
            .IsUnique();

            entity.Property(p => p.SiteId)
            .HasColumnName("SiteID")
            .ForSqlServerHasColumnType("int")
            .IsRequired()
            ;

            entity.HasIndex(p => p.SiteId);

            entity.Property(p => p.SiteGuid)
                .HasColumnName("SiteGuid")
                .ForSqlServerHasColumnType("uniqueidentifier")
                .IsRequired()
                ;

            entity.Property(p => p.AccountApproved)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(1)
            ;

            entity.Property(p => p.DisplayInMemberList)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(1)
            ;

            entity.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(100)
            ;

            entity.Property(p => p.NormalizedEmail)
            .IsRequired()
            .HasMaxLength(100)
            ;

            entity.Property(p => p.UserName)
            .IsRequired()
            .HasMaxLength(50)
            ;

            entity.Property(p => p.NormalizedUserName)
            .IsRequired()
            .HasMaxLength(50)
            ;

            entity.Property(p => p.NormalizedEmail)
            .IsRequired()
            .HasMaxLength(100)
            ;

            entity.Property(p => p.EmailConfirmed)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.IsDeleted)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.IsLockedOut)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.MustChangePwd)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.NewEmailApproved)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.RolesChanged)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.TwoFactorEnabled)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;


            entity.Property(p => p.PhoneNumber)
           .HasMaxLength(50)
           ;

            entity.Property(p => p.PhoneNumberConfirmed)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(0)
            ;

            entity.Property(p => p.CanAutoLockout)
            .IsRequired()
            .ForSqlServerHasColumnType("bit")
            .ForSqlServerHasDefaultValue(1)
            ;

            entity.Property(p => p.AvatarUrl)
            .HasMaxLength(255)
            ;

            entity.Property(p => p.WebSiteUrl)
            .HasMaxLength(100)
            ;

           
            entity.HasIndex(p => p.SiteGuid);

            entity.HasIndex(p => p.Email);
            entity.HasIndex(p => p.NormalizedEmail);
            entity.HasIndex(p => p.UserName);
            entity.HasIndex(p => p.NormalizedUserName);


        }

        public void Map(EntityTypeBuilder<SiteRole> entity)
        {
            entity.ToTable("mp_Roles");
            entity.HasKey(p => p.RoleId);

            entity.Property(p => p.RoleId)
            .UseSqlServerIdentityColumn<int>()
            .ValueGeneratedOnAdd()
            .ForSqlServerHasColumnName("RoleID")
            // .Metadata.SentinelValue = -1
            ;

            entity.Property(p => p.RoleGuid)
               .ForSqlServerHasColumnType("uniqueidentifier")
               .ForSqlServerHasDefaultValueSql("newid()")
               .IsRequired()
               ;

            entity.HasIndex(p => p.RoleGuid)
            .IsUnique();

            entity.Property(p => p.SiteId)
            .HasColumnName("SiteID")
            .ForSqlServerHasColumnType("int")
            .IsRequired()
            ;

            entity.HasIndex(p => p.SiteId);

            entity.Property(p => p.SiteGuid)
               .ForSqlServerHasColumnType("uniqueidentifier")
               .IsRequired()
               ;

            entity.HasIndex(p => p.SiteGuid);

            entity.Property(p => p.RoleName)
            .IsRequired()
            .HasMaxLength(50);
            ;

            entity.HasIndex(p => p.RoleName);

            entity.Property(p => p.DisplayName)
            .IsRequired()
            .HasMaxLength(50);
            ;

        }

        public void Map(EntityTypeBuilder<UserClaim> entity)
        {
            entity.ToTable("mp_UserClaims");
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id)
            .UseSqlServerIdentityColumn<int>()
            .ValueGeneratedOnAdd()
            // .Metadata.SentinelValue = -1
            ;

            entity.Property(p => p.UserId)
            .HasMaxLength(128)
            .IsRequired()
            ;

            entity.Property(p => p.SiteId)
            .HasColumnName("SiteId")
            .ForSqlServerHasColumnType("int")
            .IsRequired()
            ;

            entity.Property(p => p.ClaimType)
            .HasMaxLength(255)
            ;

            // should we limit claim value?

            entity.HasIndex(p => p.SiteId);

            // not mapped will result in nvarchar(max) I think
            //ClaimType
            //ClaimValue

        }

        public void Map(EntityTypeBuilder<UserLogin> entity)
        {
            entity.ToTable("mp_UserLogins");
            entity.HasKey(p => new { p.LoginProvider, p.ProviderKey, p.UserId });

            entity.Property(p => p.LoginProvider)
            .HasMaxLength(128)
            ;

            entity.Property(p => p.ProviderKey)
            .HasMaxLength(128)
            ;

            entity.Property(p => p.UserId)
            .HasMaxLength(128)
            ;

            entity.Property(p => p.SiteId)
            .HasColumnName("SiteId")
            .ForSqlServerHasColumnType("int")
            .IsRequired()
            ;

            entity.HasIndex(p => p.SiteId);

            entity.Property(p => p.ProviderDisplayName)
            .HasMaxLength(100)
            ;

        }

        public void Map(EntityTypeBuilder<GeoCountry> entity)
        {
            entity.ToTable("mp_GeoCountry");
            entity.HasKey(p => p.Guid);

            entity.Property(p => p.Guid)
               .ForSqlServerHasColumnType("uniqueidentifier")
               .ForSqlServerHasDefaultValueSql("newid()")
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

        }

        public void Map(EntityTypeBuilder<GeoZone> entity)
        {
            entity.ToTable("mp_GeoZone");
            entity.HasKey(p => p.Guid);

            entity.Property(p => p.Guid)
               .ForSqlServerHasColumnType("uniqueidentifier")
               .ForSqlServerHasDefaultValueSql("newid()")
               .IsRequired()
               ;

            entity.Property(p => p.CountryGuid)
               .ForSqlServerHasColumnType("uniqueidentifier")
               .IsRequired()
               ;

            entity.HasIndex(p => p.CountryGuid);

            entity.Property(p => p.Name)
            .HasMaxLength(255)
            .IsRequired()
            ;

            entity.Property(p => p.Code)
            .HasMaxLength(255)
            .IsRequired()
            ;

            
        }

        public void Map(EntityTypeBuilder<Currency> entity)
        {
            entity.ToTable("mp_Currency");
            entity.HasKey(p => p.Guid);

            entity.Property(p => p.Guid)
               .ForSqlServerHasColumnType("uniqueidentifier")
               .ForSqlServerHasDefaultValueSql("newid()")
               .IsRequired()
               ;

            entity.Property(p => p.Title)
            .HasMaxLength(50)
            .IsRequired()
            ;

            entity.Property(p => p.Code)
            .HasMaxLength(3)
            .IsRequired()
            ;

            entity.Property(p => p.SymbolLeft)
            .HasMaxLength(15)
            ;

            entity.Property(p => p.SymbolRight)
            .HasMaxLength(15)
            ;

            entity.Property(p => p.DecimalPointChar)
            .HasMaxLength(1)
            ;

            entity.Property(p => p.ThousandsPointChar)
            .HasMaxLength(1)
            ;

            entity.Property(p => p.DecimalPlaces)
            .HasMaxLength(1)
            ;

            entity.Property(p => p.LastModified)
            .ForSqlServerHasColumnType("datetime")
            .ForSqlServerHasDefaultValueSql("getutcdate()")
            ;

            entity.Property(p => p.Created)
            .ForSqlServerHasColumnType("datetime")
            .ForSqlServerHasDefaultValueSql("getutcdate()")
            ;

        }

        public void Map(EntityTypeBuilder<Language> entity)
        {
            entity.ToTable("mp_Language");
            entity.HasKey(p => p.Guid);

            entity.Property(p => p.Guid)
               .ForSqlServerHasColumnType("uniqueidentifier")
               .ForSqlServerHasDefaultValueSql("newid()")
               .IsRequired()
               ;

            entity.Property(p => p.Name)
            .HasMaxLength(255)
            .IsRequired()
            ;

            entity.Property(p => p.Code)
            .HasMaxLength(2)
            .IsRequired()
            ;

            entity.Property(p => p.Sort)
            .ForSqlServerHasColumnType("int")
            .ForSqlServerHasDefaultValue(1)
            .IsRequired()
            ;

        }


        

        public void Map(EntityTypeBuilder<UserLocation> entity)
        {
            entity.ToTable("mp_UserLocation");
            entity.HasKey(p => p.RowId);

            entity.Property(p => p.RowId)
            .ForSqlServerHasColumnType("uniqueidentifier")
            .ForSqlServerHasDefaultValueSql("newid()")
            .HasColumnName("RowID")  
           ;

            entity.Property(p => p.UserGuid)
            .ForSqlServerHasColumnType("uniqueidentifier")
            ;

            entity.Property(p => p.SiteGuid)
            .ForSqlServerHasColumnType("uniqueidentifier")
            ;

            entity.Property(p => p.IpAddress)
                .HasColumnName("IPAddress")
                .HasMaxLength(50)
             ;

            entity.Property(p => p.IpAddressLong)
                .HasColumnName("IPAddressLong")
             ;

            entity.Property(p => p.Isp)
                .HasColumnName("ISP")
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
               .HasColumnName("FirstCaptureUTC")
            ;

            entity.Property(p => p.LastCaptureUtc)
               .HasColumnName("LastCaptureUTC")
            ;

            entity.Property(p => p.Latitude)
               .ForSqlServerHasColumnType("float")
            ;

            entity.Property(p => p.Longitude)
               .ForSqlServerHasColumnType("float")
            ;

            entity.Property(p => p.HostName)
                .HasMaxLength(255)
             ;

            entity.HasIndex(p => p.UserGuid);

            // goode idea or not?
            //entity.HasIndex(p => p.Latitude);
            //entity.HasIndex(p => p.Longitude);

        }


        // this entity is not part of models is just needed for a join table

        public void Map(EntityTypeBuilder<UserRole> entity)
        {
            entity.ToTable("mp_UserRoles");
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id)
            //.HasSqlServerColumnType("int")
            .UseSqlServerIdentityColumn()
            .HasColumnName("ID")
            //.Metadata.SentinelValue = -1
            ;

            entity.Property(p => p.UserId)
            .ForSqlServerHasColumnType("int")
            .HasColumnName("UserID")
            .IsRequired()
            ;

            entity.Property(p => p.UserGuid)
            .ForSqlServerHasColumnType("uniqueidentifier")
            //.Metadata.SentinelValue = Guid.Empty
            ;

            entity.Property(p => p.RoleId)
            .ForSqlServerHasColumnType("int")
            .HasColumnName("RoleID")
           
            .IsRequired()
            ;

            entity.Property(p => p.RoleGuid)
            .ForSqlServerHasColumnType("uniqueidentifier")
            //.Metadata.SentinelValue = Guid.Empty
            ;

           
                
            
        }

    }
}
