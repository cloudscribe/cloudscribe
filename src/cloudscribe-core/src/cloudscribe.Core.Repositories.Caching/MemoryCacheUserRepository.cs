// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-14
// Last Modified:			2015-10-13
// 


using cloudscribe.Core.Models;
using Microsoft.Framework.Caching.Memory;
using Microsoft.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.Caching
{
    
    public class MemoryCacheUserRepository
    {
        private IUserRepository implementation;
        private IMemoryCache cache;
        private ILogger log;

        public MemoryCacheUserRepository(
            IUserRepository implementation,
            IMemoryCache cache,
            ILogger<MemoryCacheUserRepository> logger)
        {
            if (implementation == null) { throw new ArgumentNullException(nameof(implementation)); }
            if (implementation is MemoryCacheUserRepository) { throw new ArgumentException("implementation cannot be an instance of MemoryCacheUserRepository"); }
            if (cache == null) { throw new ArgumentNullException(nameof(cache)); }
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }

            this.cache = cache;
            this.implementation = implementation;
            log = logger;

        }

        
        

    }
}
