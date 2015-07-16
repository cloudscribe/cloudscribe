//using cloudscribe.Core.Models;
//using Microsoft.AspNet.Identity;
////using Microsoft.AspNet.Identity.Owin;
////using Microsoft.Owin;
////using Microsoft.Owin.Security;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace cloudscribe.Core.Identity
//{
//    public class SiteSignInManager<TUser> : SignInManager<TUser> where TUser : SiteUser
//    {
//        public SiteSignInManager(SiteUserManager userManager, IAuthenticationManager authenticationManager)
//            : base(userManager, authenticationManager)
//        {
//        }

//        public override Task<ClaimsIdentity> CreateUserIdentityAsync(SiteUser user)
//        {
//            return user.GenerateUserIdentityAsync((SiteUserManager)UserManager);
//        }

//        //public static SiteSignInManager Create(IdentityFactoryOptions<SiteSignInManager> options, IOwinContext context)
//        //{
//        //    return new SiteSignInManager(context.GetUserManager<SiteUserManager>(), context.Authentication);
//        //}
//    }
//}
