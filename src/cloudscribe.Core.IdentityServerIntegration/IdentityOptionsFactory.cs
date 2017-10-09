using cloudscribe.Core.Identity;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class IdentityOptionsFactory : IIdentityOptionsFactory
    {
        public IdentityOptions CreateOptions()
        {
            var options =  new IdentityOptions();

            //https://github.com/IdentityServer/IdentityServer4.AspNetIdentity/blob/dev/src/IdentityServer4.AspNetIdentity/IdentityServerBuilderExtensions.cs

            options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
            options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
            options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;

            return options;
        }
    }
}
