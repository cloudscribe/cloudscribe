// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-28
// Last Modified:		    2015-08-28
// 


using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Authentication.Facebook;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Extensions;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.WebUtilities;
using Microsoft.Framework.Logging;
using Newtonsoft.Json.Linq;

namespace cloudscribe.Core.Identity.OAuth
{
    public class MultiTenantFacebookOptionsResolver
    {
        public MultiTenantFacebookOptionsResolver(
            FacebookAuthenticationOptions originalOptions,
            ISiteResolver siteResolver,
            MultiTenantOptions multiTenantOptions)
        {
            this.originalOptions = originalOptions;
            this.siteResolver = siteResolver;
            this.multiTenantOptions = multiTenantOptions;
        }

        private FacebookAuthenticationOptions originalOptions;
        private ISiteResolver siteResolver;
        private MultiTenantOptions multiTenantOptions;
        private ISiteSettings site = null;
        public ISiteSettings Site
        {
            get
            {
                if(site == null) { site = siteResolver.Resolve(); }
                return site;
            }
        }

        public string AppId
        {
            get
            {
                if (Site != null)
                {
                    if ((Site.FacebookAppId.Length > 0) && (Site.FacebookAppSecret.Length > 0))
                    {
                        return Site.FacebookAppId;
                    }
                }

                return originalOptions.AppId;
            }
        }

        public string AppSecret
        {
            get
            {
                if (Site != null)
                {
                    if ((Site.FacebookAppId.Length > 0) && (Site.FacebookAppSecret.Length > 0))
                    {
                        return Site.FacebookAppSecret;
                    }
                }

                return originalOptions.AppSecret;
            }
        }

        public string ResolveRedirectUrl(string redirectUrl)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if ((Site != null)&&(Site.SiteFolderName.Length > 0))
                {
                    if ((Site.FacebookAppId.Length > 0) && (Site.FacebookAppSecret.Length > 0))
                    {
                        return redirectUrl.Replace("signin-facebook", site.SiteFolderName + "/signin-facebook");
                    }
                }
            }
            

            return redirectUrl;
        }

    }
}
