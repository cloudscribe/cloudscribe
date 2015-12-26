// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-26
// Last Modified:			2015-12-26
// 

using cloudscribe.Logging.Web;
using Microsoft.Data.Entity.Metadata.Builders;

namespace cloudscribe.Logging.EF
{
    public interface ILogModelMapper
    {
        void Map(EntityTypeBuilder<LogItem> entity);
    }
}
