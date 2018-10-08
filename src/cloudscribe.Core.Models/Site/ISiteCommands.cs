// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2018-10-08
// 

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    // a marker interface so we can inject as singleton
    public interface ISiteCommandsSingleton : ISiteCommands
    {

    }
    public interface ISiteCommands 
    {
        Task Create(ISiteSettings site, CancellationToken cancellationToken = default(CancellationToken));
        Task Update(ISiteSettings site, CancellationToken cancellationToken = default(CancellationToken));
        Task Delete(Guid siteId, CancellationToken cancellationToken = default(CancellationToken));
        Task AddHost(Guid siteId, string hostName, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteHost(Guid siteId, Guid hostId, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteHostsBySite(Guid siteId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
