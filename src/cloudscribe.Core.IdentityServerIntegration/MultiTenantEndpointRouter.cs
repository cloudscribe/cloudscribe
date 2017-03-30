
using cloudscribe.Core.Models;
using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class MultiTenantEndpointRouter : IEndpointRouter
    {
        private readonly Dictionary<string, EndpointName> _pathToNameMap;
        private readonly IdentityServerOptions _options;
        private readonly IEnumerable<EndpointMapping> _mappings;
        private readonly ILogger<MultiTenantEndpointRouter> _logger;
        private MultiTenantOptions multiTenantOptions;

        public MultiTenantEndpointRouter(
            Dictionary<string, EndpointName> pathToNameMap, 
            IdentityServerOptions options, 
            IEnumerable<EndpointMapping> mappings, 
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            ILogger<MultiTenantEndpointRouter> logger
            )
        {
            _pathToNameMap = pathToNameMap;
            _options = options;
            _mappings = mappings;
            _logger = logger;
            multiTenantOptions = multiTenantOptionsAccessor.Value;
        }

        public IEndpoint Find(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

           // _logger.LogInformation("hey this is the custom endpointrouter find method");

            foreach (var key in _pathToNameMap.Keys)
            {
                var path = key.EnsureLeadingSlash();
                if (IsMatch(context, path))
                {
                    var endpointName = _pathToNameMap[key];
                    _logger.LogDebug("Request path {0} matched to endpoint type {1}", context.Request.Path, endpointName);

                    return GetEndpoint(endpointName, context);
                }
            }

            _logger.LogTrace("No endpoint entry found for request path: {0}", context.Request.Path);

            return null;
        }

        private bool IsMatch(HttpContext context, string path)
        {
            if (context.Request.Path.StartsWithSegments(path)) return true;
            if(multiTenantOptions.Mode == MultiTenantMode.FolderName && !multiTenantOptions.UseRelatedSitesMode)
            {
                var site = context.GetTenant<SiteContext>();
                if(site != null && (!string.IsNullOrEmpty(site.SiteFolderName)))
                {
                    var folderPath = "/" + site.SiteFolderName + path;
                    if (context.Request.Path.StartsWithSegments(folderPath))
                    {
                        var idBasePath = context.GetIdentityServerBasePath().EnsureTrailingSlash();
                        context.SetIdentityServerBasePath(idBasePath + site.SiteFolderName + "/");
                        return true;
                    }
                }
            }

            return false;
        }

        private IEndpoint GetEndpoint(EndpointName endpointName, HttpContext context)
        {
            if (_options.Endpoints.IsEndpointEnabled(endpointName))
            {
                var mapping = _mappings.Where(x => x.Endpoint == endpointName).LastOrDefault();
                if (mapping != null)
                {
                    _logger.LogDebug("Mapping found for endpoint: {0}, creating handler: {1}", endpointName, mapping.Handler.FullName);
                    return context.RequestServices.GetService(mapping.Handler) as IEndpoint;
                }
                else
                {
                    _logger.LogError("No mapping found for endpoint: {0}", endpointName);
                }
            }
            else
            {
                _logger.LogWarning("{0} endpoint requested, but is diabled in endpoint options.", endpointName);
            }

            return null;
        }
    }

    internal static class EndpointExtensions
    {
        public static string EnsureLeadingSlash(this string url)
        {
            if (!url.StartsWith("/"))
            {
                return "/" + url;
            }

            return url;
        }

        public static string EnsureTrailingSlash(this string url)
        {
            if (!url.EndsWith("/"))
            {
                return url + "/";
            }

            return url;
        }

        public static bool IsEndpointEnabled(this EndpointsOptions options, EndpointName endpointName)
        {
            switch (endpointName)
            {
                case EndpointName.Authorize:
                    return options.EnableAuthorizeEndpoint;
                case EndpointName.CheckSession:
                    return options.EnableCheckSessionEndpoint;
                case EndpointName.Discovery:
                    return options.EnableDiscoveryEndpoint;
                case EndpointName.EndSession:
                    return options.EnableEndSessionEndpoint;
                case EndpointName.Introspection:
                    return options.EnableIntrospectionEndpoint;
                case EndpointName.Revocation:
                    return options.EnableTokenRevocationEndpoint;
                case EndpointName.Token:
                    return options.EnableTokenEndpoint;
                case EndpointName.UserInfo:
                    return options.EnableUserInfoEndpoint;
                default:
                    return false;
            }
        }
    }
}
