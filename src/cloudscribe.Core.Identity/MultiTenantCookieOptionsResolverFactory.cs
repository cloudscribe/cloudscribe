// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-01
// Last Modified:			2016-02-04
// 

using cloudscribe.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;

namespace cloudscribe.Core.Identity
{
    public class MultiTenantCookieOptionsResolverFactory
    {

        public MultiTenantCookieOptionsResolverFactory(
            //ISiteResolver siteResolver,
            SiteSettings currentSite,
            IOptions<MultiTenantOptions> multiTenantOptions,
            ILoggerFactory loggerFactory)
        {
            //this.siteResolver = siteResolver;
            this.currentSite = currentSite;
            this.multiTenantOptions = multiTenantOptions;
            this.loggerFactory = loggerFactory;
        }

        private IOptions<MultiTenantOptions> multiTenantOptions;
        private SiteSettings currentSite;
        //private ISiteResolver siteResolver;
        private ILoggerFactory loggerFactory;

        public MultiTenantCookieOptionsResolver GetResolver()
        {
            return new MultiTenantCookieOptionsResolver(currentSite, multiTenantOptions, loggerFactory);
        }

    }
}
