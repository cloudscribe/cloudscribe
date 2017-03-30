using cloudscribe.Core.Models;
using IdentityServer4.Configuration;
using IdentityServer4;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class CorsPathValidator : ICorsPathValidator
    {
        public CorsPathValidator(
            //SiteContext currentSite,
            IHttpContextAccessor contextAccessor,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor)
        {
            multiTenantOptions = multiTenantOptionsAccessor.Value;
            this.contextAccessor = contextAccessor;
            //site = currentSite;
        }

        private MultiTenantOptions multiTenantOptions;
        private IHttpContextAccessor contextAccessor;
        //private SiteContext site;

        public bool IsPathAllowed(PathString path, CorsOptions options)
        {
            var result = options.CorsPaths.Any(x => path == x);
            if (result) return result;

            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                var site = contextAccessor.HttpContext.GetTenant<SiteContext>();
                if(site != null && !string.IsNullOrEmpty(site.SiteFolderName))
                {
                    //result = options.CorsPaths.Any(x => path == "/" + site.SiteFolderName + x);
                    foreach (var cp in options.CorsPaths)
                    {
                        var tenantPath = "/" + site.SiteFolderName + cp;
                        if (path == tenantPath) return true;
                    }
                }
                
            }

            return result;
        }
    }
}
