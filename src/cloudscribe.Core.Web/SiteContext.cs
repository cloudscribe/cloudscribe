// Author:					Joe Audette
// Created:				    2014-08-31
// Last Modified:		    2014-10-23
// 
//
// You must not remove this notice, or any other, from this software.

using cloudscribe.AspNet.Identity;
using cloudscribe.Core.Web.Identity;
using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Repositories.Caching;
using cloudscribe.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Web;

namespace cloudscribe.Core.Web
{
    public sealed class SiteContext : ISiteContext
    {
        public SiteContext(
            ISiteRepository siteRepository, 
            IUserRepository userRepository,
            IDataProtectionProvider dataProtection)
        {
            if (siteRepository == null) { throw new ArgumentException("siteRepository cannot be null"); }
            siteRepo = siteRepository;

            if (userRepository == null) { throw new ArgumentException("userRepository cannot be null"); }
            userRepo = userRepository;

            if (dataProtection == null) { throw new ArgumentException("dataProtectionProvider cannot be null"); }
            dataProtectionProvider = dataProtection;
            
        }

        private IOwinContext owinContext = null;

        private ISiteRepository siteRepo;

        public ISiteRepository SiteRepository
        {
            get { return siteRepo; }
        }


        private ISiteSettings siteSettings = null;

        public ISiteSettings SiteSettings
        {
            get 
            {
                if (siteSettings == null) { Resolve(); }
                return siteSettings; 
            }
        }

        private IUserRepository userRepo;

        public IUserRepository UserRepository
        {
            get { return userRepo; }
        }

        private SiteUserManager userManager = null;

        public UserManager<SiteUser> SiteUserManager
        {
            get
            {
                if (siteSettings == null) { Resolve(); }
                if (userManager == null) { userManager = CreateUserManager(); }
                return userManager;
            }
        }

        private SiteSignInManager signInManager = null;

        public SignInManager<SiteUser, string> SiteSignInManager
        {
            get
            {
                if (siteSettings == null) { Resolve(); }
                if (signInManager == null) 
                {
                    signInManager = new SiteSignInManager((SiteUserManager)SiteUserManager, owinContext.Authentication);
                }
                return signInManager;
            }
        }

        private IDataProtectionProvider dataProtectionProvider = null;

        public IDataProtectionProvider DataProtectionProvider
        {
            get { return dataProtectionProvider; }
        }

        private SiteUserManager CreateUserManager()
        {

            IUserStore<SiteUser> userStore = new UserStore<SiteUser>(
                siteSettings,
                userRepo
                );

            SiteUserManager manager = new SiteUserManager(userStore);

            // this turned out to not be needed once we figured out how
            // to use a different auth cookie per folder tenant
            //if (AppSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            //{
            //    if (!AppSettings.UseRelatedSiteMode)
            //    {
            //        manager.ClaimsIdentityFactory = new MultiTenantClaimsIdentityFactory<SiteUser, string>();
            //    }
            //}

            manager.UserValidator = new UserValidator<SiteUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = siteSettings.MinRequiredPasswordLength,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<SiteUser>
            {
                MessageFormat = CommonResources.SecurityCodeMessageFormat
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<SiteUser>
            {
                Subject = CommonResources.SecurityCode,
                BodyFormat = CommonResources.SecurityCodeMessageFormat
            });

            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();

            //IDataProtectionProvider dataProtectionProvider = null;

            //var dataProtectionProvider =  options.DataProtectionProvider;
            //if (dataProtectionProvider != null)
            //{
                manager.UserTokenProvider
                    = new DataProtectorTokenProvider<SiteUser>(DataProtectionProvider.Create("UserToken"));
            //}

            manager.PasswordHasher = new SitePasswordHasher();

            //manager.MaxFailedAccessAttemptsBeforeLockout

            return manager;
        }

        private void Resolve()
        {
            //Resolve(HttpContext.Current.Request.Url);
            owinContext = HttpContext.Current.GetOwinContext();
            Resolve(HttpContext.Current.Request.RawUrl, HttpContext.Current.Request.Url.Host);
        }

        //public void Resolve(Uri uri)
        //{
        //    Resolve(uri.ToString(), uri.Host);
        //}

        private void Resolve(string url, string host)
        {
            if (siteSettings != null) { return; }
            
            int siteId = -1;
  
            if (AppSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            {
                string siteFolderName = GetFirstFolderSegment(url);
                if (siteFolderName.Length == 0) siteFolderName = "root";
                siteId = siteRepo.GetSiteIdByFolder(siteFolderName);
                //siteSettings = repo.(siteFolderName);

                if (siteId == -1)
                {
                    // error would be expected here on initial setup
                    // when the db has not been set up and no site exists

                    throw new Exception("could not resolve site id");
                }
                else
                {
                    siteSettings = siteRepo.Fetch(siteId);
                }

                
            }
            else
            {
                //siteId = repo.GetSiteIdByHostName(context.Request.Host.Value);
                siteSettings = siteRepo.Fetch(host);
            }
            
        }

        public static string GetFirstFolderSegment(string url)
        {

            // find first level folder name
            // after site root
            string folderName = string.Empty;

            string requestPath = url.ToString().Replace("https://", string.Empty).Replace("http://", string.Empty);
            //HttpContext.Current.Request.RawUrl.Replace("https://", string.Empty).Replace("http://", string.Empty);

            if (requestPath == "/") return folderName;

            //  cloudscribe/Content/css?v=vSkk4S2yX-JkF2jnutJR9WADNnO1X7e9w005ClDaRCs1

            int indexOfFirstSlash = requestPath.IndexOf("/");
            int indexOfLastSlash = requestPath.LastIndexOf("/");

            if (
                (indexOfFirstSlash > -1)
                && (indexOfLastSlash > (indexOfFirstSlash + 1))
                )
            {
                requestPath = requestPath.Substring(indexOfFirstSlash + 1, requestPath.Length - indexOfFirstSlash - 1);

                if (requestPath.IndexOf("/") > -1)
                {
                    folderName = requestPath.Substring(0, requestPath.IndexOf("/"));

                }
            }

            // else
            //    /en
            if ((0 == indexOfFirstSlash) && (0 == indexOfLastSlash) && (requestPath.Length > 1))
            {
                folderName = requestPath.Substring(1);
            }

            return folderName;
        }

        //public static ISiteRepository GetSiteRepository()
        //{
        //    // TODO : dependency injection

        //    ISiteRepository siteRepository
        //            = new CachingSiteRepository(
        //            new cloudscribe.Core.Repositories.MSSQL.SiteRepository()
        //            );

        //    //ISiteRepository siteRepository
        //    //        = new CachingSiteRepository(
        //    //        new cloudscribe.Core.Repositories.MySql.SiteRepository()
        //    //        );

        //    //ISiteRepository siteRepository
        //    //        = new CachingSiteRepository(
        //    //        new cloudscribe.Core.Repositories.pgsql.SiteRepository()
        //    //        );

        //    //ISiteRepository siteRepository
        //    //        = new CachingSiteRepository(
        //    //        new cloudscribe.Core.Repositories.SqlCe.SiteRepository()
        //    //        );

        //    //ISiteRepository siteRepository
        //    //        = new CachingSiteRepository(
        //    //        new cloudscribe.Core.Repositories.Firebird.SiteRepository()
        //    //        );

        //    //ISiteRepository siteRepository
        //    //        = new CachingSiteRepository(
        //    //        new cloudscribe.Core.Repositories.SQLite.SiteRepository()
        //    //        );


        //    return siteRepository;
        //}

        //public static IUserRepository GetUserRepository()
        //{
        //    IUserRepository userRepository = new CachingUserRepository(
        //        new cloudscribe.Core.Repositories.MSSQL.UserRepository()
        //        );

        //    //IUserRepository userRepository = new CachingUserRepository(
        //    //    new cloudscribe.Core.Repositories.MySql.UserRepository()
        //    //    );

        //    //IUserRepository userRepository = new CachingUserRepository(
        //    //    new cloudscribe.Core.Repositories.pgsql.UserRepository()
        //    //    );

        //    //IUserRepository userRepository = new CachingUserRepository(
        //    //    new cloudscribe.Core.Repositories.SqlCe.UserRepository()
        //    //    );

        //    //IUserRepository userRepository = new CachingUserRepository(
        //    //    new cloudscribe.Core.Repositories.Firebird.UserRepository()
        //    //    );

        //    //IUserRepository userRepository = new CachingUserRepository(
        //    //    new cloudscribe.Core.Repositories.SQLite.UserRepository()
        //    //    );


        //    return userRepository;
        //} 

        //CA1063	Implement IDisposable correctly	Modify 'SiteContext.Dispose()' so that it calls Dispose(true), 
        //then calls GC.SuppressFinalize on the current object instance ('this' or 'Me' in Visual Basic), 
        // and then returns.	cloudscribe.Core.Web	SiteContext.cs	121

        public void Dispose()
        {
            Dispose(true);
            
        }

        protected void Dispose(bool disposeManaged)
        {
            siteRepo.Dispose();
            userRepo.Dispose();
            signInManager.Dispose();
            userManager.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
