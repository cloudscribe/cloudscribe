// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-17
// Last Modified:			2015-11-17
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using cloudscribe.Core.Models;

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

        public void DoMapping(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SiteSettings>(entity =>
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


            });


        }
    }
}
