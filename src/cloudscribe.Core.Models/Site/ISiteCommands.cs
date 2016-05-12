// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2016-05-09
// 

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public interface ISiteCommands : IDisposable
    {
        Task Create(ISiteSettings site, CancellationToken cancellationToken = default(CancellationToken));
        Task Update(ISiteSettings site, CancellationToken cancellationToken = default(CancellationToken));
        Task Delete(Guid siteGuid, CancellationToken cancellationToken = default(CancellationToken));
        Task AddHost(Guid siteGuid, string hostName, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteHost(Guid hostGuid, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteHostsBySite(Guid siteGuid, CancellationToken cancellationToken = default(CancellationToken));
    }
}
