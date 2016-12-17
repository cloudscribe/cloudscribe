using IdentityServer4.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Hosting
{
    public interface IMatchEndSessionProtocolRoutePaths
    {
        bool IsEndSessionPath(string requestPath);

        bool IsEndSessionCallbackPath(string requestPath);
    }

    public class DefaultEndSessionProtocolRouteMatcher : IMatchEndSessionProtocolRoutePaths
    {
        public bool IsEndSessionPath(string requestPath)
        {
            return (requestPath == Constants.ProtocolRoutePaths.EndSession.EnsureLeadingSlash());
        }

        public bool IsEndSessionCallbackPath(string requestPath)
        {
            return (requestPath == Constants.ProtocolRoutePaths.EndSessionCallback.EnsureLeadingSlash());
        }

       
    }
}
