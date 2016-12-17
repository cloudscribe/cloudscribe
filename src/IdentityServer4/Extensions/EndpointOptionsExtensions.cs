// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Hosting;

namespace IdentityServer4.Extensions
{
    internal static class EndpointOptionsExtensions
    {
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