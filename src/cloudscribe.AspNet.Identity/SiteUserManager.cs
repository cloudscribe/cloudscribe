// Author:					Joe Audette
// Created:				    2014-07-22
// Last Modified:		    2014-09-06
// 
//
// You must not remove this notice, or any other, from this software.


using cloudscribe.Core.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;


namespace cloudscribe.AspNet.Identity
{
   /// <summary>
   /// an instance of this class is created and configured in cloudscribe.Core.Web.SiteContext.cs
   /// </summary>
    public class SiteUserManager : UserManager<SiteUser>
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(SiteUserManager));

        //private IUserStore<SiteUser> _store = null;

        //public IUserStore<SiteUser> Store
        //{
        //    get { return _store; }
        //}

        public SiteUserManager(IUserStore<SiteUser> store)
            : base(store)
        {
           // _store = store;
        }

        public override Task<ClaimsIdentity> CreateIdentityAsync(SiteUser user, string authenticationType)
        {

            return base.CreateIdentityAsync(user, authenticationType);
        }
    }

    

    
}
