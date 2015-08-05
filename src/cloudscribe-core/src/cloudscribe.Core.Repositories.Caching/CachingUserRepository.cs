// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-14
// Last Modified:			2015-08-05
// 


using cloudscribe.Core.Models;
using Microsoft.Framework.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.Caching
{
    
    public class CachingUserRepository
    {
        public CachingUserRepository(
            IDistributedCache cache,
            IUserRepository implementation)
        {
            this.cache = cache;
            this.implementation = implementation;

        }

        private IDistributedCache cache;
        private IUserRepository implementation;

    }
}
