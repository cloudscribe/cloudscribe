// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-11
// Last Modified:			2016-06-11
// 

using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class CacheHelper
    {
        public CacheHelper(
            IMemoryCache cache
            )
        {
            this.cache = cache;
        }

        private IMemoryCache cache;

        public void ClearCache(string cacheKey)
        {
            cache.Remove(cacheKey);
        }
    }
}
