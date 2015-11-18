// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-01
// Last Modified:			2015-11-18
// 

using cloudscribe.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;

namespace cloudscribe.Core.Identity
{
    public class MultiTenantCookieOptionsResolverFactory
    {

        public MultiTenantCookieOptionsResolverFactory(
            ISiteResolver siteResolver,
            IOptions<MultiTenantOptions> multiTenantOptions,
            ILoggerFactory loggerFactory)
        {
            this.siteResolver = siteResolver;
            this.multiTenantOptions = multiTenantOptions;
            this.loggerFactory = loggerFactory;
        }

        private IOptions<MultiTenantOptions> multiTenantOptions;
        private ISiteResolver siteResolver;
        private ILoggerFactory loggerFactory;

        public MultiTenantCookieOptionsResolver GetResolver()
        {
            return new MultiTenantCookieOptionsResolver(siteResolver, multiTenantOptions, loggerFactory);
        }

    }
}
