// Author:					Joe Audette
// Created:				    2014-09-08
// Last Modified:		    2014-10-23
// 
// You must not remove this notice, or any other, from this software.


using cloudscribe.Core.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Identity
{
    //http://stackoverflow.com/questions/19763807/how-to-set-a-custom-claimsprincipal-in-mvc-5

    /// <summary>
    /// at first was using this for folder based multi tenants
    /// but not used currently because we figured out how to use a different auth cookie per tenant
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class MultiTenantClaimsIdentityFactory<TUser, TKey> : ClaimsIdentityFactory<TUser, string> 
        where TUser : SiteUser
        , Microsoft.AspNet.Identity.IUser<string>
    {

        public MultiTenantClaimsIdentityFactory()
            : base()
        { }

        public override Task<ClaimsIdentity> CreateAsync(
            UserManager<TUser, string> manager, TUser user, string authenticationType)
        {
            //override Creation of ClaimsIdentity and return it.
            Task<ClaimsIdentity> ci = base.CreateAsync(manager, user, authenticationType);

            MultiTenantClaimsIdentity ci2 = MultiTenantClaimsIdentity.FromClaimsIdentity(ci.Result);
           
            
            return Task.FromResult((ClaimsIdentity)ci2);
        }

    }
}
