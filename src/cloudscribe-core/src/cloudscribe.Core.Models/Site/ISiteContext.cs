// Author:					Joe Audette
// Created:					2014-09-01
// Last Modified:			2014-09-06
// 

using System;
//using System.Collections.Generic;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin.Security.DataProtection;

namespace cloudscribe.Core.Models
{
    public interface ISiteContext : IDisposable
    {
        ISiteSettings SiteSettings { get; }
        ISiteRepository SiteRepository { get; }
        IUserRepository UserRepository { get; }
        UserManager<SiteUser> SiteUserManager { get; }
        SignInManager<SiteUser> SiteSignInManager { get; }
        //IDataProtectionProvider DataProtectionProvider { get; }

    }
}
