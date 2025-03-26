using Microsoft.AspNetCore.Identity;

namespace cloudscribe.Core.Identity
{
    public class DefaultSocialAuthEmailVerfificationPolicy : ISocialAuthEmailVerfificationPolicy
    {
        public DefaultSocialAuthEmailVerfificationPolicy()
        {}

        public bool HasVerifiedEmail(ExternalLoginInfo externalLoginInfo)
        {
            return false;
        }
    }
}