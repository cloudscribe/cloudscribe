// Author:					Joe Audette
// Created:				    2014-07-22
// Last Modified:		    2014-09-06
// 
//
// You must not remove this notice, or any other, from this software.


using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Ninject;
using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
//using log4net;

//http://www.asp.net/identity/overview/migrations/migrating-an-existing-website-from-sql-membership-to-aspnet-identity
//https://aspnetidentity.codeplex.com/workitem/2333


//http://odetocode.com/blogs/scott/archive/2014/01/20/implementing-asp-net-identity.aspx
//http://msdn.microsoft.com/en-us/library/microsoft.aspnet.identity%28v=vs.108%29.aspx
//http://stackoverflow.com/questions/19487322/what-is-asp-net-identitys-iusersecuritystampstoretuser-interface

//http://www.hanselman.com/blog/GlobalizationInternationalizationAndLocalizationInASPNETMVC3JavaScriptAndJQueryPart1.aspx

namespace cloudscribe.AspNet.Identity
{
   /// <summary>
   /// an instance of this class is created and configured in cloudscribe.Core.Web.SiteContext.cs
   /// </summary>
    public class SiteUserManager : UserManager<SiteUser>
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(SiteUserManager));

        public SiteUserManager(IUserStore<SiteUser> store)
            : base(store)
        {
        }

        public override Task<ClaimsIdentity> CreateIdentityAsync(SiteUser user, string authenticationType)
        {

            return base.CreateIdentityAsync(user, authenticationType);
        }
    }

    

    
}
