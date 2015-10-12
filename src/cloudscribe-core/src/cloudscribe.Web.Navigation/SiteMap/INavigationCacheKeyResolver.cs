// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-10-12
// Last Modified:			2015-10-12
// 

namespace cloudscribe.Web.Navigation
{
    public interface INavigationCacheKeyResolver
    {
        string ResolveCacheKey(string providedCacheKey);
    }
}
