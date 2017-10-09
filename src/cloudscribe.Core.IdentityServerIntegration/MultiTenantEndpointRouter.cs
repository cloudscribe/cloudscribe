
using cloudscribe.Core.Models;
using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using static cloudscribe.Core.IdentityServerIntegration.CustomConstants;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class MultiTenantEndpointRouter : IEndpointRouter
    {
        //private readonly Dictionary<string, EndpointName> _pathToNameMap;
        private readonly IdentityServerOptions _options;
        //private readonly IEnumerable<EndpointMapping> _mappings;
        private readonly IEnumerable<Endpoint> _endpoints;
        
        private readonly ILogger<MultiTenantEndpointRouter> _logger;
        private MultiTenantOptions _multiTenantOptions;

        public MultiTenantEndpointRouter(
            IEnumerable<Endpoint> endpoints, 
            IdentityServerOptions options,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            ILogger<MultiTenantEndpointRouter> logger
            )
        {
            //_pathToNameMap = pathToNameMap;
            _endpoints = endpoints;
            _options = options;
            //_mappings = mappings;
            _logger = logger;
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
        }

        public IEndpointHandler Find(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach (var endpoint in _endpoints)
            {
                var path = endpoint.Path;
                //if (context.Request.Path.StartsWithSegments(path))
                if (IsMatch(context, path))
                {
                    var endpointName = endpoint.Name;
                    _logger.LogDebug("Request path {path} matched to endpoint type {endpoint}", context.Request.Path, endpointName);

                    return GetEndpointHandler(endpoint, context);
                }
            }

            _logger.LogTrace("No endpoint entry found for request path: {path}", context.Request.Path);

            return null;
        }



        //public IEndpoint Find(HttpContext context)
        //{
        //    if (context == null) throw new ArgumentNullException(nameof(context));

        //   // _logger.LogInformation("hey this is the custom endpointrouter find method");

        //    foreach (var key in _pathToNameMap.Keys)
        //    {
        //        var path = key.EnsureLeadingSlash();
        //        if (IsMatch(context, path))
        //        {
        //            var endpointName = _pathToNameMap[key];
        //            _logger.LogDebug("Request path {0} matched to endpoint type {1}", context.Request.Path, endpointName);

        //            return GetEndpoint(endpointName, context);
        //        }
        //    }

        //    _logger.LogTrace("No endpoint entry found for request path: {0}", context.Request.Path);

        //    return null;
        //}

        private bool IsMatch(HttpContext context, string path)
        {
            if (context.Request.Path.StartsWithSegments(path)) return true;
            if(_multiTenantOptions.Mode == MultiTenantMode.FolderName && !_multiTenantOptions.UseRelatedSitesMode)
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

        private IEndpointHandler GetEndpointHandler(Endpoint endpoint, HttpContext context)
        {
            if (_options.Endpoints.IsEndpointEnabled(endpoint))
            {
                var handler = context.RequestServices.GetService(endpoint.Handler) as IEndpointHandler;
                if (handler != null)
                {
                    _logger.LogDebug("Endpoint enabled: {endpoint}, successfully created handler: {endpointHandler}", endpoint.Name, endpoint.Handler.FullName);
                    return handler;
                }
                else
                {
                    _logger.LogDebug("Endpoint enabled: {endpoint}, failed to create handler: {endpointHandler}", endpoint.Name, endpoint.Handler.FullName);
                }
            }
            else
            {
                _logger.LogWarning("Endpoint disabled: {endpoint}", endpoint.Name);
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

        public static bool IsEndpointEnabled(this EndpointsOptions options, Endpoint endpoint)
        {
            switch (endpoint?.Name)
            {
                case EndpointNames.Authorize:
                    return options.EnableAuthorizeEndpoint;
                case EndpointNames.CheckSession:
                    return options.EnableCheckSessionEndpoint;
                case EndpointNames.Discovery:
                    return options.EnableDiscoveryEndpoint;
                case EndpointNames.EndSession:
                    return options.EnableEndSessionEndpoint;
                case EndpointNames.Introspection:
                    return options.EnableIntrospectionEndpoint;
                case EndpointNames.Revocation:
                    return options.EnableTokenRevocationEndpoint;
                case EndpointNames.Token:
                    return options.EnableTokenEndpoint;
                case EndpointNames.UserInfo:
                    return options.EnableUserInfoEndpoint;
                default:
                    // fall thru to true to allow custom endpoints
                    return true;
            }
        }

       
    }
}
