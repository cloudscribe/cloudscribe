using IdentityServer4.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration
{
    internal static class CustomConstants
    {

        public static class ProtocolRoutePaths
        {
            public const string Authorize = "connect/authorize";
            public const string AuthorizeAfterConsent = Authorize + "/consent";
            public const string AuthorizeAfterLogin = Authorize + "/login";
            public const string DiscoveryConfiguration = ".well-known/openid-configuration";
            public const string DiscoveryWebKeys = DiscoveryConfiguration + "/jwks";
            public const string Token = "connect/token";
            public const string Revocation = "connect/revocation";
            public const string UserInfo = "connect/userinfo";
            public const string Introspection = "connect/introspect";
            public const string EndSession = "connect/endsession";
            public const string EndSessionCallback = EndSession + "/callback";
            public const string CheckSession = "connect/checksession";

            public static readonly string[] CorsPaths =
            {
                DiscoveryConfiguration,
                DiscoveryWebKeys,
                Token,
                UserInfo,
                Revocation
            };
        }

        public static readonly Dictionary<string, EndpointName> EndpointPathToNameMap = new Dictionary<string, EndpointName>
        {
            { ProtocolRoutePaths.Authorize, EndpointName.Authorize },
            { ProtocolRoutePaths.CheckSession, EndpointName.CheckSession},
            { ProtocolRoutePaths.DiscoveryConfiguration, EndpointName.Discovery},
            { ProtocolRoutePaths.EndSession, EndpointName.EndSession },
            { ProtocolRoutePaths.Introspection, EndpointName.Introspection },
            { ProtocolRoutePaths.Revocation, EndpointName.Revocation },
            { ProtocolRoutePaths.Token, EndpointName.Token },
            { ProtocolRoutePaths.UserInfo, EndpointName.UserInfo },
        };

    }
}
