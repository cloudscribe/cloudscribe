// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-17
// Last Modified:			2015-11-18
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
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
            .UseSqlServerIdentityColumn()
            .HasColumnName("SiteID")
            .Metadata.SentinelValue = -1
            ;

            entity.Property(p => p.SiteGuid)
            .HasSqlServerColumnType("uniqueidentifier")
            .HasDefaultValueSql("newid()")
            .Metadata.SentinelValue = Guid.Empty
            ;

            entity.Index(p => p.SiteGuid)
            .Unique();

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
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(1)
            ;

            entity.Property(p => p.UseSecureRegistration)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

            entity.Property(p => p.UseSslOnAllPages)
            .IsRequired()
            .HasColumnName("UseSSLOnAllPages")
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

            entity.Property(p => p.IsServerAdminSite)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

            entity.Property(p => p.UseLdapAuth)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

            entity.Property(p => p.AutoCreateLdapUserOnFirstLogin)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(1)
            ;

            entity.Property(p => p.LdapServer)
            .HasMaxLength(255);
            ;

            entity.Property(p => p.LdapPort)
            .IsRequired()
            .HasSqlServerColumnType("int")
            .HasSqlServerDefaultValue(389)
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
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(1)
            ;

            entity.Property(p => p.UseEmailForLogin)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(1)
            ;

            entity.Property(p => p.AllowUserFullNameChange)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

            entity.Property(p => p.RequiresQuestionAndAnswer)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

            entity.Property(p => p.MaxInvalidPasswordAttempts)
            .IsRequired()
            .HasSqlServerColumnType("int")
            .HasSqlServerDefaultValue(5)
            ;

            entity.Property(p => p.PasswordAttemptWindowMinutes)
            .IsRequired()
            .HasSqlServerColumnType("int")
            .HasSqlServerDefaultValue(5)
            ;

            entity.Property(p => p.MinRequiredPasswordLength)
            .IsRequired()
            .HasSqlServerColumnType("int")
            .HasSqlServerDefaultValue(5)
            ;

            entity.Property(p => p.MinReqNonAlphaChars)
            .IsRequired()
            .HasSqlServerColumnType("int")
            .HasSqlServerDefaultValue(0)
            ;

            entity.Property(p => p.DefaultEmailFromAddress)
            .HasMaxLength(100);
            ;

            entity.Property(p => p.RecaptchaPrivateKey)
            .HasMaxLength(255);
            ;

            entity.Property(p => p.RecaptchaPublicKey)
            .HasMaxLength(255);
            ;

            entity.Property(p => p.ApiKeyExtra1)
            .HasMaxLength(255);
            ;

            entity.Property(p => p.ApiKeyExtra2)
            .HasMaxLength(255);
            ;

            entity.Property(p => p.ApiKeyExtra3)
            .HasMaxLength(255);
            ;

            entity.Property(p => p.ApiKeyExtra4)
            .HasMaxLength(255);
            ;

            entity.Property(p => p.ApiKeyExtra5)
            .HasMaxLength(255);
            ;

            entity.Property(p => p.DisableDbAuth)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

            entity.Property(p => p.RequireApprovalBeforeLogin)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

            entity.Property(p => p.AllowDbFallbackWithLdap)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

            entity.Property(p => p.EmailLdapDbFallback)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

            entity.Property(p => p.AllowPersistentLogin)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(1)
            ;

            entity.Property(p => p.CaptchaOnLogin)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

            entity.Property(p => p.CaptchaOnRegistration)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

            entity.Property(p => p.SiteIsClosed)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

            //not mapped should map to ntext by default I think
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
            .HasMaxLength(100);
            ;

            entity.Property(p => p.GoogleClientId)
            .HasMaxLength(100);
            ;

            entity.Property(p => p.GoogleClientSecret)
            .HasMaxLength(100);
            ;

            entity.Property(p => p.TwitterConsumerKey)
            .HasMaxLength(100);
            ;

            entity.Property(p => p.TwitterConsumerSecret)
            .HasMaxLength(100);
            ;

            entity.Property(p => p.MicrosoftClientId)
            .HasMaxLength(100);
            ;

            entity.Property(p => p.MicrosoftClientSecret)
            .HasMaxLength(100);
            ;

            entity.Property(p => p.PreferredHostName)
            .HasMaxLength(250);
            ;

            entity.Property(p => p.SiteFolderName)
            .HasMaxLength(50);
            ;

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
            .HasSqlServerColumnType("int")
            .HasSqlServerDefaultValue(25)
            ;

            entity.Property(p => p.SmtpUser)
            .HasMaxLength(500);
            ;

            entity.Property(p => p.SmtpPassword)
            .HasMaxLength(500);
            ;

            entity.Property(p => p.SmtpPreferredEncoding)
            .HasMaxLength(20);
            ;

            entity.Property(p => p.SmtpRequiresAuth)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

            entity.Property(p => p.SmtpUseSsl)
            .IsRequired()
            .HasSqlServerColumnType("bit")
            .HasSqlServerDefaultValue(0)
            ;

        }

        public void Map(EntityTypeBuilder<SiteHost> entity)
        {
            entity.ToTable("mp_SiteHosts");
            entity.HasKey(p => p.HostId);

            entity.Property(p => p.HostId)
            .UseSqlServerIdentityColumn()
            .HasColumnName("HostID")
            .Metadata.SentinelValue = -1
            ;

            entity.Property(p => p.SiteId)
            .HasColumnName("SiteID")
            .HasSqlServerColumnType("int")
            .IsRequired()
            ;

            entity.Property(p => p.SiteGuid)
            .HasColumnName("SiteGuid")
            .HasSqlServerColumnType("uniqueidentifier")
            .IsRequired()
            ;


            entity.Property(p => p.HostName)
            .IsRequired()
            .HasMaxLength(255);
            ;

        }

        public void Map(EntityTypeBuilder<SiteFolder> entity)
        {
            entity.ToTable("mp_SiteFolders");
            entity.HasKey(p => p.Guid);

            entity.Property(p => p.Guid)
               .HasSqlServerColumnType("uniqueidentifier")
               .HasSqlServerDefaultValueSql("newid()")
               .IsRequired()
               ;

            entity.Property(p => p.SiteGuid)
                .HasColumnName("SiteGuid")
                .HasSqlServerColumnType("uniqueidentifier")
                .IsRequired()
                ;

            entity.Property(p => p.FolderName)
            .IsRequired()
            .HasMaxLength(255);
            ;

        }

        public void Map(EntityTypeBuilder<SiteUser> entity)
        {
            entity.ToTable("mp_Users");
            entity.HasKey(p => p.UserId);

            entity.Property(p => p.UserId)
            .UseSqlServerIdentityColumn()
            .HasColumnName("UserID")
            .Metadata.SentinelValue = -1
            ;

            entity.Property(p => p.UserGuid)
               .HasSqlServerColumnType("uniqueidentifier")
               .HasSqlServerDefaultValueSql("newid()")
               .IsRequired()
               ;

            entity.Index(p => p.UserGuid)
            .Unique();

            entity.Property(p => p.SiteId)
            .HasColumnName("SiteID")
            .HasSqlServerColumnType("int")
            .IsRequired()
            ;

            entity.Index(p => p.SiteId);

            entity.Property(p => p.SiteGuid)
                .HasColumnName("SiteGuid")
                .HasSqlServerColumnType("uniqueidentifier")
                .IsRequired()
                ;

            entity.Index(p => p.SiteGuid);


        }

        public void Map(EntityTypeBuilder<SiteRole> entity)
        {

        }

        public void Map(EntityTypeBuilder<UserClaim> entity)
        {

        }

        public void Map(EntityTypeBuilder<UserLogin> entity)
        {

        }

        public void Map(EntityTypeBuilder<GeoCountry> entity)
        {

        }

        public void Map(EntityTypeBuilder<GeoZone> entity)
        {

        }

        public void Map(EntityTypeBuilder<Currency> entity)
        {

        }

        public void Map(EntityTypeBuilder<Language> entity)
        {


        }

    }
}
