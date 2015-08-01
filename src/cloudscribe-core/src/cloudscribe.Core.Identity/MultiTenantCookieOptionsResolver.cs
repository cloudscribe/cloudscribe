// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-01
// Last Modified:			2015-08-01
// 

using cloudscribe.Core.Models;
using Microsoft.Framework.OptionsModel;
using Microsoft.AspNet.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class MultiTenantCookieOptionsResolver
    {
        public MultiTenantCookieOptionsResolver(
            ISiteResolver siteResolver,
            IOptions<MultiTenantOptions> multiTenantOptions)
        {

            this.siteResolver = siteResolver;
            this.multiTenantOptions = multiTenantOptions.Options;

        }

        private MultiTenantOptions multiTenantOptions;
        private ISiteResolver siteResolver;
        private ISiteSettings site = null;
        private ISiteSettings Site
        {
            get
            {
                if(site == null) { site = siteResolver.Resolve(); }
                return site;
            }
        }

        public string ResolveCookieName(string suppliedCookieName)
        {
            if(multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if(!multiTenantOptions.UseRelatedSitesMode)
                {
                    if(Site.SiteFolderName.Length > 0)
                    {
                        return suppliedCookieName + "-" + Site.SiteFolderName;
                    }
                }
            }

            return suppliedCookieName;
        }

        public string ResolveAuthScheme(string suppliedAuthScheme)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if (!multiTenantOptions.UseRelatedSitesMode)
                {
                    if (Site.SiteFolderName.Length > 0)
                    {
                        return suppliedAuthScheme + "-" + Site.SiteFolderName;
                    }

                }
            }

            return suppliedAuthScheme;
        }

        public string ResolveLoginPath(string suppliedLoginPath)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if (!multiTenantOptions.UseRelatedSitesMode)
                {
                    if (Site.SiteFolderName.Length > 0)
                    {
                        return "/" + Site.SiteFolderName + suppliedLoginPath;
                    }
                }
            }

            return suppliedLoginPath;
        }

        public string ResolveLogoutPath(string suppliedLogoutPath)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if (!multiTenantOptions.UseRelatedSitesMode)
                {
                    if (Site.SiteFolderName.Length > 0)
                    {
                        return "/" + Site.SiteFolderName + suppliedLogoutPath;
                    }
                }
            }

            return suppliedLogoutPath;
        }

        public string ResolveReturnUrlParameter(string suppliedReturnUrlParameter)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if (!multiTenantOptions.UseRelatedSitesMode)
                {
                    if (Site.SiteFolderName.Length > 0)
                    {

                    }
                }
            }

            return suppliedReturnUrlParameter;
        }

        //public ISecureDataFormat<AuthenticationTicket> TicketDataFormat { get; set; }

    }
}
