using IdentityServer4.Extensions;

namespace IdentityServer4.Hosting
{
    public interface IMatchAuthorizeProtocolRoutePaths
    {
        bool IsAuthorizePath(string requestPath);

        bool IsAuthorizeAfterLoginPath(string requestPath);

        bool IsAuthorizeAfterConsentPath(string requestPath);
    }

    public class DefaultAuthorizeProtocolRouteMatcher : IMatchAuthorizeProtocolRoutePaths
    {
        public bool IsAuthorizePath(string requestPath)
        {
            return (requestPath == Constants.ProtocolRoutePaths.Authorize.EnsureLeadingSlash());
        }

        public bool IsAuthorizeAfterLoginPath(string requestPath)
        {
            return (requestPath == Constants.ProtocolRoutePaths.AuthorizeAfterLogin.EnsureLeadingSlash());
        }

        public bool IsAuthorizeAfterConsentPath(string requestPath)
        {
            return (requestPath == Constants.ProtocolRoutePaths.AuthorizeAfterConsent.EnsureLeadingSlash());
        }
    }
}
