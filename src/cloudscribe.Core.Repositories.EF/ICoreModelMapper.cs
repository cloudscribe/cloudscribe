// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-17
// Last Modified:			2015-11-18
// 

using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Models.Logging;

namespace cloudscribe.Core.Repositories.EF
{
    public interface ICoreModelMapper
    {
        void Map(EntityTypeBuilder<SiteSettings> entity);
        void Map(EntityTypeBuilder<SiteHost> entity);
        void Map(EntityTypeBuilder<SiteFolder> entity);

        void Map(EntityTypeBuilder<SiteUser> entity);
        void Map(EntityTypeBuilder<SiteRole> entity);
        void Map(EntityTypeBuilder<UserClaim> entity);
        void Map(EntityTypeBuilder<UserLogin> entity);

        void Map(EntityTypeBuilder<GeoCountry> entity);
        void Map(EntityTypeBuilder<GeoZone> entity);
        void Map(EntityTypeBuilder<Currency> entity);
        void Map(EntityTypeBuilder<Language> entity);

        void Map(EntityTypeBuilder<LogItem> entity);

        void Map(EntityTypeBuilder<UserRole> entity);


    }
}
