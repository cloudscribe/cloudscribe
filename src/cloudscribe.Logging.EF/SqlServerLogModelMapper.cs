// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-26
// Last Modified:			2015-12-26
// 

using cloudscribe.Logging.Web;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;

namespace cloudscribe.Logging.EF
{
    public class SqlServerLogModelMapper : ILogModelMapper
    {
        public void Map(EntityTypeBuilder<LogItem> entity)
        {
            entity.ToTable("mp_SystemLog");
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id)
            .ForSqlServerHasColumnType("int")

            //.UseSqlServerIdentityColumn<int>()
            .ValueGeneratedOnAdd()
            .HasColumnName("ID")
            .HasDefaultValueSql("NEXT VALUE FOR LogIds")
            //.Metadata.SentinelValue = -1
            ;

            entity.Property(p => p.LogDateUtc)
            .HasColumnName("LogDate")
            .ForSqlServerHasColumnType("datetime")
            .ForSqlServerHasDefaultValueSql("getutcdate()")
            ;

            entity.Property(p => p.IpAddress)
            .HasMaxLength(50)
            ;

            entity.Property(p => p.Culture)
            .HasMaxLength(10)
            ;

            entity.Property(p => p.ShortUrl)
            .HasMaxLength(255)
            ;

            entity.Property(p => p.Thread)
            .HasMaxLength(255)
            ;

            entity.Property(p => p.LogLevel)
            .HasMaxLength(20)
            ;

            entity.Property(p => p.Logger)
            .HasMaxLength(255)
            ;

            //Url
            //Message

        }
    }
}
