using cloudscribe.Core.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.AspNet.Identity
{
    public class SiteSignInManager: SignInManager<SiteUser, string>
    {
        public SiteSignInManager(SiteUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(SiteUser user)
        {
            return user.GenerateUserIdentityAsync((SiteUserManager)UserManager);
        }

        //public static SiteSignInManager Create(IdentityFactoryOptions<SiteSignInManager> options, IOwinContext context)
        //{
        //    return new SiteSignInManager(context.GetUserManager<SiteUserManager>(), context.Authentication);
        //}
    }
}
