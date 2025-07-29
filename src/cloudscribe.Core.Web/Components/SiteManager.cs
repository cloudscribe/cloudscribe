﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-22
// Last Modified:			2019-02-17
// 

using cloudscribe.Core.DataProtection;
using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.EventHandlers;
using cloudscribe.Pagination.Models;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.Navigation.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteManager
    {

        public SiteManager(
            SiteContext currentSite,
            SiteEvents siteEventHandlers,
            ISiteCommands siteCommands,
            ISiteQueries siteQueries,
            IUserCommands userCommands,
            IUserQueries userQueries,
            SiteDataProtector dataProtector,
            IHttpContextAccessor contextAccessor,
            ILogger<SiteManager> logger,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<SiteConfigOptions> siteConfigOptionsAccessor,
            CacheHelper cacheHelper,
            ITreeCache treeCache,
            SiteRoleManager<SiteRole> roleManager
            )
        {

            _commands           = siteCommands;
            _queries            = siteQueries;
            _userCommands       = userCommands;
            _userQueries        = userQueries;
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _siteConfigOptions  = siteConfigOptionsAccessor.Value;
            _context            = contextAccessor?.HttpContext;
            _dataProtector      = dataProtector;
            _log                = logger;

            //resolver = siteResolver;
            _currentSite     = currentSite;
            _cacheHelper     = cacheHelper;
            _eventHandlers   = siteEventHandlers;
            _navigationCache = treeCache;
            _roleManager     = roleManager;
            //_navigationTreeBuilderService = navigationTreeBuilderService;
        }

        private readonly HttpContext _context;
        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;
        private ILogger _log;
        private SiteDataProtector _dataProtector;
        private CacheHelper _cacheHelper;
        private MultiTenantOptions _multiTenantOptions;
        private SiteConfigOptions _siteConfigOptions;
        private ISiteCommands _commands;
        private ISiteQueries _queries;
        private IUserQueries _userQueries;
        private IUserCommands _userCommands;
        private ISiteContext _currentSite = null;
        private SiteEvents _eventHandlers;
        private readonly ITreeCache _navigationCache;
        private readonly SiteRoleManager<SiteRole> _roleManager;

        //private readonly NavigationTreeBuilderService _navigationTreeBuilderService;



        public ISiteContext CurrentSite
        {
            get { return _currentSite; }
        }

        public async Task<ISiteSettings> GetCurrentSiteSettings()
        {
            return await _queries.Fetch(CurrentSite.Id, CancellationToken);
        }

        public async Task<ISiteContext> GetSiteForDataOperations(Guid? siteId, bool useRelatedSiteId = false)
        {
            if(_multiTenantOptions.UseRelatedSitesMode)
            {
                if (useRelatedSiteId)
                {
                    return await Fetch(_multiTenantOptions.RelatedSiteId) as ISiteContext;
                }
            }

            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) 
                && (siteId.Value != CurrentSite.Id) 
                && (CurrentSite.IsServerAdminSite))
            {
                return await Fetch(siteId.Value) as ISiteContext; 
            }

            return CurrentSite;
        }

        public async Task<ISiteSettings> GetSiteForEdit(Guid? siteId)
        {
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty)
                && (siteId.Value != CurrentSite.Id)
                && (CurrentSite.IsServerAdminSite))
            {
                return await Fetch(siteId.Value);
            }

            return await Fetch(CurrentSite.Id);
        }

        public Task<PagedResult<ISiteInfo>> GetPageOtherSites(
            Guid currentSiteId,
            int pageNumber,
            int pageSize)
        {
            return _queries.GetPageOtherSites(currentSiteId, pageNumber, pageSize, CancellationToken);

        }

        public Task<int> CountOtherSites(Guid currentSiteId)
        {
            return _queries.CountOtherSites(currentSiteId, CancellationToken);
        }
        
        public async Task<ISiteSettings> Fetch(Guid siteId)
        {
            var site = await _queries.Fetch(siteId, CancellationToken);
            _dataProtector.UnProtect(site);
            return site;
        }
        
        public async Task<ISiteSettings> Fetch(string hostname)
        {
            var site = await _queries.Fetch(hostname, CancellationToken);
            _dataProtector.UnProtect(site);
            return site;
        }

        /// <summary>
        /// returns true if the folder is not in use or is in use only on the passed in ISiteSettings
        /// </summary>
        /// <param name=""></param>
        /// <param name="requestedFolderName"></param>
        /// <returns></returns>
        public async Task<bool> FolderNameIsAvailable(Guid requestingSiteId, string requestedFolderName)
        {
            var matchingSite = await _queries.FetchByFolderName(requestedFolderName, CancellationToken).ConfigureAwait(false) ;
            if(matchingSite == null) { return true; }
            if(matchingSite.SiteFolderName != requestedFolderName) { return true; }
            if(matchingSite.Id == requestingSiteId) { return true; }

            return false;

        }

        public async Task<bool> HostNameIsAvailable(Guid requestingSiteId, string requestedHostName)
        {
            return await _queries.HostNameIsAvailable(requestingSiteId, requestedHostName, CancellationToken).ConfigureAwait(false);

        }

        public async Task<bool> AliasIdIsAvailable(Guid requestingSiteId, string requestedAliasId)
        {
            if (_multiTenantOptions.AllowSharedAliasId) return true;

            if (string.IsNullOrWhiteSpace(requestedAliasId)) return false;
            if (requestedAliasId.Length > 36) return false;
            return await _queries.AliasIdIsAvailable(requestingSiteId, requestedAliasId, CancellationToken).ConfigureAwait(false);
            
        }

        public async Task Update(ISiteSettings site, string oldFolderName = null)
        {
            await _eventHandlers.HandleSitePreUpdate(site.Id).ConfigureAwait(false);

            _dataProtector.Protect(site);
            if(site.Id == Guid.Empty)
            {
                site.Id = Guid.NewGuid();
                await _commands.Update(site, CancellationToken.None);
            }
            else
            {
                await _commands.Update(site, CancellationToken.None);
            }

            await _cacheHelper.SetDistributedCacheTimestamp(site.Id, site.LastModifiedUtc).ConfigureAwait(false);

            if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if(!string.IsNullOrEmpty(oldFolderName))
                {
                    _cacheHelper.ClearLocalCache(oldFolderName);
                    await _cacheHelper.ClearSiteFolderListCache();
                    await _navigationCache.ClearTreeCache();
                }

                if(string.IsNullOrEmpty(site.SiteFolderName))
                {
                    _cacheHelper.ClearLocalCache("root");
                }
                else
                {
                    _cacheHelper.ClearLocalCache(site.SiteFolderName);
                }
            }
            else
            {
                if (_context != null && !string.IsNullOrEmpty(_context.Request.Host.Value))
                {
                    _cacheHelper.ClearLocalCache(_context.Request.Host.Value);
                }
                    

                if (!string.IsNullOrWhiteSpace(site.PreferredHostName))
                {
                    _cacheHelper.ClearLocalCache(site.PreferredHostName);
                }
                
                var siteHosts = await GetSiteHosts(site.Id);
                if (siteHosts != null && siteHosts.Count > 0)
                {
                    foreach(ISiteHost siteHostName in siteHosts)
                    {
                        _cacheHelper.ClearLocalCache(siteHostName.HostName);
                    }
                }
            }
            _cacheHelper.ClearLocalCache("site-" + site.Id.ToString());

            await _eventHandlers.HandleSiteUpdated(site).ConfigureAwait(false);
        }

        public async Task Delete(ISiteSettings site)
        {
            await _eventHandlers.HandleSitePreDelete(site.Id).ConfigureAwait(false);

            // delete users
            await _userCommands.DeleteUsersBySite(site.Id, CancellationToken.None); // this also deletes userroles claims logins
            await _userCommands.DeleteRolesBySite(site.Id, CancellationToken.None);
            await _commands.DeleteHostsBySite(site.Id, CancellationToken.None);
            
            await _commands.Delete(site.Id, CancellationToken.None);

            await _cacheHelper.ClearSiteFolderListCache();
        }

        public async Task<SiteSettings> CreateNewSite(bool isServerAdminSite)
        {
            var newSite = InitialData.BuildInitialSite();
            newSite.Theme = _siteConfigOptions.FirstSiteTheme;
            newSite.IsServerAdminSite = isServerAdminSite;
            var siteNumber = 1 + await _queries.CountOtherSites(Guid.Empty);
            newSite.AliasId = $"s{siteNumber}";

            await CreateNewSite(newSite);
            
            return newSite;
        }

        public async Task CreateNewSite(ISiteSettings newSite)
        {
            if (newSite == null) { throw new ArgumentNullException("you must pass in an instance of ISiteSettings"); }
            
            if(string.IsNullOrEmpty(newSite.Theme))
            {
                newSite.Theme = _siteConfigOptions.DefaultTheme;
            }
           
            await _commands.Create(newSite, CancellationToken.None);

            if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                await _cacheHelper.ClearSiteFolderListCache();
            }
                

            await _eventHandlers.HandleSiteCreated(newSite).ConfigureAwait(false);

        }

        public async Task CreateNewSiteFromClone(ISiteSettings newSite, ISiteSettings sourceSite)
        {
            if (newSite == null) { throw new ArgumentNullException("you must pass in an instance of ISiteSettings"); }

            if (string.IsNullOrEmpty(newSite.Theme))
            {
                if (string.IsNullOrEmpty(sourceSite.Theme))
                    newSite.Theme = _siteConfigOptions.DefaultTheme;
                else
                    newSite.Theme = sourceSite.Theme;
            }

            /// Clone lots of things
            /// Maybe separate out into UI options
            newSite.RequireCookieConsent       = sourceSite.RequireCookieConsent;
            newSite.CookiePolicySummary        = sourceSite.CookiePolicySummary;
            newSite.AllowNewRegistration       = sourceSite.AllowNewRegistration;
            newSite.RequireConfirmedEmail      = sourceSite.RequireConfirmedEmail;
            newSite.RequireConfirmedPhone      = sourceSite.RequireConfirmedPhone;
            newSite.LdapUseSsl                 = sourceSite.LdapUseSsl;
            newSite.LdapServer                 = sourceSite.LdapServer;
            newSite.LdapDomain                 = sourceSite.LdapDomain;
            newSite.LdapPort                   = sourceSite.LdapPort;
            newSite.LdapRootDN                 = sourceSite.LdapRootDN;
            newSite.LdapUserDNKey              = sourceSite.LdapUserDNKey;
            newSite.UseEmailForLogin           = sourceSite.UseEmailForLogin;
            newSite.DisableDbAuth              = sourceSite.DisableDbAuth;
            newSite.RequiresQuestionAndAnswer  = sourceSite.RequiresQuestionAndAnswer;
            newSite.RequireApprovalBeforeLogin = sourceSite.RequireApprovalBeforeLogin;
            newSite.AccountApprovalEmailCsv    = sourceSite.AccountApprovalEmailCsv;
            newSite.MaxInvalidPasswordAttempts = sourceSite.MaxInvalidPasswordAttempts;
            newSite.MinRequiredPasswordLength  = sourceSite.MinRequiredPasswordLength;
            newSite.PwdRequireNonAlpha         = sourceSite.PwdRequireNonAlpha;
            newSite.PwdRequireLowercase        = sourceSite.PwdRequireLowercase;
            newSite.PwdRequireUppercase        = sourceSite.PwdRequireUppercase;
            newSite.PwdRequireDigit            = sourceSite.PwdRequireDigit;
            newSite.AllowPersistentLogin       = sourceSite.AllowPersistentLogin;
            newSite.CaptchaOnRegistration      = sourceSite.CaptchaOnRegistration;
            newSite.CaptchaOnLogin             = sourceSite.CaptchaOnLogin;
            newSite.RecaptchaPrivateKey        = sourceSite.RecaptchaPrivateKey;
            newSite.RecaptchaPublicKey         = sourceSite.RecaptchaPublicKey;
            newSite.UseInvisibleRecaptcha      = sourceSite.UseInvisibleRecaptcha;
            newSite.FacebookAppId              = sourceSite.FacebookAppId;
            newSite.FacebookAppSecret          = sourceSite.FacebookAppSecret;
            newSite.MicrosoftClientId          = sourceSite.MicrosoftClientId;
            newSite.MicrosoftClientSecret      = sourceSite.MicrosoftClientSecret;
            newSite.GoogleClientId             = sourceSite.GoogleClientId;
            newSite.GoogleClientSecret         = sourceSite.GoogleClientSecret;
            newSite.TwitterConsumerKey         = sourceSite.TwitterConsumerKey;
            newSite.TwitterConsumerSecret      = sourceSite.TwitterConsumerSecret;
            newSite.OidConnectAppId            = sourceSite.OidConnectAppId;
            newSite.OidConnectAppSecret        = sourceSite.OidConnectAppSecret;
            newSite.OidConnectAuthority        = sourceSite.OidConnectAuthority;
            newSite.OidConnectDisplayName      = sourceSite.OidConnectDisplayName;
            newSite.TimeZoneId                 = sourceSite.TimeZoneId;
            newSite.RegRestrictionTld          = sourceSite.RegRestrictionTld;
            
            // We might want to leave out all the company stuff since that's likely to be differrent?
            newSite.CompanyName                = sourceSite.CompanyName;
            newSite.CompanyStreetAddress       = sourceSite.CompanyStreetAddress;
            newSite.CompanyStreetAddress2      = sourceSite.CompanyStreetAddress2;
            newSite.CompanyLocality            = sourceSite.CompanyLocality;
            newSite.CompanyRegion              = sourceSite.CompanyRegion;
            newSite.CompanyPostalCode          = sourceSite.CompanyPostalCode;
            newSite.CompanyCountry             = sourceSite.CompanyCountry;
            newSite.CompanyPhone               = sourceSite.CompanyPhone;
            newSite.CompanyFax                 = sourceSite.CompanyFax;
            newSite.CompanyPublicEmail         = sourceSite.CompanyPublicEmail;
            newSite.CompanyWebsite             = sourceSite.CompanyWebsite;


            newSite.DefaultEmailFromAddress    = sourceSite.DefaultEmailFromAddress;
            newSite.DefaultEmailFromAlias      = sourceSite.DefaultEmailFromAlias;
            newSite.SmtpUser                   = sourceSite.SmtpUser;
            newSite.SmtpPassword               = sourceSite.SmtpPassword;
            newSite.SmtpPort                   = sourceSite.SmtpPort;
            newSite.SmtpPreferredEncoding      = sourceSite.SmtpPreferredEncoding;
            newSite.SmtpServer                 = sourceSite.SmtpServer;
            newSite.SmtpRequiresAuth           = sourceSite.SmtpRequiresAuth;
            newSite.SmtpUseSsl                 = sourceSite.SmtpUseSsl;
            newSite.DkimPrivateKey             = sourceSite.DkimPrivateKey;
            newSite.DkimDomain                 = sourceSite.DkimDomain;
            newSite.DkimSelector               = sourceSite.DkimSelector;
            newSite.SignEmailWithDkim          = sourceSite.SignEmailWithDkim;
            newSite.EmailSenderName            = sourceSite.EmailSenderName;
            newSite.EmailApiKey                = sourceSite.EmailApiKey;
            newSite.EmailApiEndpoint           = sourceSite.EmailApiEndpoint;
            newSite.SmsClientId                = sourceSite.SmsClientId;
            newSite.SmsSecureToken             = sourceSite.SmsSecureToken;
            newSite.SmsFrom                    = sourceSite.SmsFrom;
            newSite.GoogleAnalyticsProfileId   = sourceSite.GoogleAnalyticsProfileId;
            newSite.RegistrationAgreement      = sourceSite.RegistrationAgreement;
            newSite.RegistrationPreamble       = sourceSite.RegistrationPreamble;
            newSite.LoginInfoTop               = sourceSite.LoginInfoTop;
            newSite.LoginInfoBottom            = sourceSite.LoginInfoBottom;
            newSite.SiteIsClosed               = sourceSite.SiteIsClosed;
            newSite.SiteIsClosedMessage        = sourceSite.SiteIsClosedMessage;
            newSite.PrivacyPolicy              = sourceSite.PrivacyPolicy;
            newSite.IsDataProtected            = sourceSite.IsDataProtected;
            newSite.ForcedCulture              = sourceSite.ForcedCulture;
            newSite.ForcedUICulture            = sourceSite.ForcedUICulture;
            newSite.FooterContent              = sourceSite.FooterContent;
            newSite.HeaderContent              = sourceSite.HeaderContent;
            newSite.Require2FA                 = sourceSite.Require2FA;
            newSite.ShowSiteNameLink           = sourceSite.ShowSiteNameLink;
            newSite.LogoUrl                    = sourceSite.LogoUrl;
            newSite.LdapUserDNFormat           = sourceSite.LdapUserDNFormat;
            newSite.OidConnectScopesCsv        = sourceSite.OidConnectScopesCsv;
            newSite.SingleBrowserSessions      = sourceSite.SingleBrowserSessions;
            newSite.AllowUserToChangeEmail     = sourceSite.AllowUserToChangeEmail;
            newSite.PasswordExpiryWarningDays = sourceSite.PasswordExpiryWarningDays;
            newSite.PasswordExpiresDays = sourceSite.PasswordExpiresDays;

            /// end clone 

            await _commands.Create(newSite, CancellationToken.None);

            if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                await _cacheHelper.ClearSiteFolderListCache();
            }

            await _eventHandlers.HandleSiteCreated(newSite).ConfigureAwait(false);
        }


        public async Task CreateRequiredRolesAndAdminUser(
            SiteSettings site,
            string adminEmail,
            string adminLoginName,
            string adminDisplayName,
            string adminPassword
            )
        {
            // New admin user for a tenant site should belong to the parent site,
            // if this related sites mode is used
            if (_multiTenantOptions.UseRelatedSitesMode && _multiTenantOptions.RelatedSiteId != Guid.Empty)
            {
                site.Id = _multiTenantOptions.RelatedSiteId;
            }

            await EnsureRequiredRoles(site);
            var adminRole = await _userQueries.FetchRole(site.Id, "Administrators", CancellationToken);

            if (adminRole == null)
            {
                throw new InvalidOperationException("Administrators role could nto be found so cannot create admin user");
            }

            var adminUser = new SiteUser
            {
                Id = Guid.NewGuid(),
                SiteId = site.Id,
                Email = adminEmail
            };
            adminUser.NormalizedEmail = adminUser.Email.ToUpperInvariant();
            adminUser.DisplayName = adminDisplayName;
            adminUser.UserName = adminLoginName;
            adminUser.NormalizedUserName = adminUser.UserName.ToUpperInvariant();
            adminUser.EmailConfirmed = true;
            adminUser.AccountApproved = true;
           
            // clear text password will be hashed upon login
            adminUser.PasswordHash =  adminPassword +"||0"; //pwd/salt/format 
            adminUser.MustChangePwd = true; // TODO: implement logic to enforce this

            await _userCommands.Create(adminUser, CancellationToken.None);

            await _userCommands.AddUserToRole(
                site.Id,
                adminRole.Id,
                adminUser.Id,
                CancellationToken.None);

        }

        public async Task CreateRequiredRolesAndAdminUser(SiteSettings site)
        {
            await EnsureRequiredRoles(site);
            await CreateAdminUser(site);
        }

        public async Task CreateRolesFromClone(
           SiteSettings newSite,
           SiteSettings sourceSite,
           string adminUserName
            )
        {
            var rolesToClone = await _roleManager.GetAllRolesBySite(sourceSite.Id);

            foreach(var role in rolesToClone)
            {
                // beware here this RoleExists query operates on normalised name
                bool exists = await _userQueries.RoleExists(newSite.Id, role.NormalizedRoleName, CancellationToken);

                // Add admin to these roles also - query operates on normalised name
                var adminUser = await _userQueries.FetchByLoginName(newSite.Id, adminUserName.ToUpperInvariant(), false, CancellationToken);

                if (!exists)
                {
                    var clonedRole = new SiteRole
                    {
                        RoleName           = role.RoleName,
                        NormalizedRoleName = role.RoleName.ToUpperInvariant(),
                        SiteId             = newSite.Id
                    };
                    await _userCommands.CreateRole(clonedRole, CancellationToken.None);

                    if(adminUser != null)
                    { 
                        await _userCommands.AddUserToRole(
                        newSite.Id,
                        clonedRole.Id,
                        adminUser.Id,
                        CancellationToken.None);
                    }
                }
            }
        }

        public async Task PostProcessClonedSite(SiteSettings newSite, SiteSettings sourceSite)
        {
            await _eventHandlers.HandleSiteCloned(newSite, sourceSite).ConfigureAwait(false);
        }

        public async Task CreateAdminUser(ISiteSettings site)
        {

            var adminRole = await _userQueries.FetchRole(site.Id, "Administrators", CancellationToken);

            if(adminRole == null)
            {
                throw new InvalidOperationException("Administrators role could nto be found so cannot create admin user");
            }
            

            // if using related sites mode there is a problem if we already have user admin@admin.com
            // and we create another one in the child site with the same email and login so we need to make it different
            // we could just skip creating this user since in related sites mode all users come from the first site
            // but then if the config were changed to not related sites mode there would be no admin user
            // so in related sites mode we create one only as a backup in case settings are changed later
            var countOfSites = await _queries.GetCount(CancellationToken);
            var siteDifferentiator = string.Empty;
            if (
                (countOfSites >= 1)
                && (_multiTenantOptions.UseRelatedSitesMode)
                )
            {
                //TODO: do we need to replace this logic now that we don't have an integer id?
                //if (site.SiteId > 1)
                //{
                //    siteDifferentiator = site.SiteId.ToInvariantString();
                //}
            }


            var adminUser = InitialData.BuildInitialAdmin();
            adminUser.SiteId = site.Id;
            adminUser.Email = "admin" + siteDifferentiator + "@admin.com";
            adminUser.NormalizedEmail = adminUser.Email.ToUpperInvariant();
            adminUser.UserName = "admin" + siteDifferentiator;
            adminUser.NormalizedUserName = adminUser.UserName.ToUpperInvariant();
            
            await _userCommands.Create(adminUser, CancellationToken.None);
            
            await _userCommands.AddUserToRole(
                site.Id,
                adminRole.Id,
                adminUser.Id,
                CancellationToken.None);

            
        }

        public async Task EnsureRequiredRoles(ISiteSettings site)
        {
            bool exists = await _userQueries.RoleExists(site.Id, "Administrators", CancellationToken);

            if(!exists)
            {
                var adminRole = InitialData.BuildAdminRole();
                adminRole.SiteId = site.Id;
                await _userCommands.CreateRole(adminRole, CancellationToken.None);
            }

            exists = await _userQueries.RoleExists(site.Id, "Role Administrators", CancellationToken);

            if (!exists)
            {
                var roleAdminRole = InitialData.BuildRoleAdminRole();
                roleAdminRole.SiteId = site.Id;
                await _userCommands.CreateRole(roleAdminRole, CancellationToken.None);
                
            }

            exists = await _userQueries.RoleExists(site.Id, "Content Administrators", CancellationToken);

            if (!exists)
            {
                var contentAdminRole = InitialData.BuildContentAdminsRole();
                contentAdminRole.SiteId = site.Id;
                await _userCommands.CreateRole(contentAdminRole, CancellationToken.None);
            }

            exists = await _userQueries.RoleExists(site.Id, "Authenticated Users", CancellationToken);

            if (!exists)
            {
                var authenticatedUserRole = InitialData.BuildAuthenticatedRole();
                authenticatedUserRole.SiteId = site.Id;
                await _userCommands.CreateRole(authenticatedUserRole, CancellationToken.None);
            }
            

        }

        

        public Task<ISiteHost> GetSiteHost(string hostName)
        {
            return _queries.GetSiteHost(hostName, CancellationToken);
        }

        public Task<List<ISiteHost>> GetSiteHosts(Guid siteId)
        {
            return _queries.GetSiteHosts(siteId, CancellationToken);
        }

        public async Task AddHost(Guid siteId, string hostName)
        {
            await _commands.AddHost(siteId, hostName, CancellationToken);
        }

        public async Task DeleteHost(Guid siteId, Guid hostId)
        {
            await _commands.DeleteHost(siteId, hostId, CancellationToken);
        }

        public Task<int> GetUserCount(Guid siteId)
        {
            // this is only used on setup controller
            // to make sure admin user was created
            return _userQueries.CountUsers(siteId, string.Empty, CancellationToken);
        }

        public Task<int> GetRoleCount(Guid siteId)
        {
            // this is only used on setup controller
            // to make sure admin user and role was created
            return _userQueries.CountOfRoles(siteId, string.Empty, CancellationToken);
        }

        public async Task<int> ExistingSiteCount()
        {
            try
            {
                return await _queries.GetCount(CancellationToken);
            }
            catch { }
            // errors are expected here before the db is initialized
            // so just return 0 if error here
            return 0;
            
        }

        public bool CheckSiteNameIsAllowed(string siteName)
        {
            string ReservedSiteNames = _siteConfigOptions.ReservedSiteNames;

            if (ReservedSiteNames.Contains(siteName.ToLower()))
            {
                return false;
            }

            return true;
        }
    }
}
