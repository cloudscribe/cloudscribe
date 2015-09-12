// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2015-09-12
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Helpers;
using cloudscribe.Core.Web.ViewModels.SiteSettings;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Localization;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.OptionsModel;
using System;
using System.Globalization;
using System.Threading.Tasks;


namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Roles = "Admins,Content Administrators")]
    public class SiteAdminController : CloudscribeBaseController
    {
        public SiteAdminController(
            SiteManager siteManager,
            GeoDataManager geoDataManager,
            IOptions<MultiTenantOptions> multiTenantOptions,
            IOptions<UIOptions> uiOptionsAccessor
            //ConfigHelper configuration
            //, ITriggerStartup startupTrigger
            )
        {
            //if (siteResolver == null) { throw new ArgumentNullException(nameof(siteResolver)); }
            if (geoDataManager == null) { throw new ArgumentNullException(nameof(geoDataManager)); }
            //if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }

            //config = configuration;
            this.multiTenantOptions = multiTenantOptions.Options;
            //Site = siteResolver.Resolve();

            this.siteManager = siteManager;
            this.geoDataManager = geoDataManager;
            uiOptions = uiOptionsAccessor.Options;

            //startup = startupTrigger;
        }

        //private ISiteSettings Site;
        private SiteManager siteManager;
        private GeoDataManager geoDataManager;
        //private ConfigHelper config;
        private MultiTenantOptions multiTenantOptions;
        //private ITriggerStartup startup;

        private UIOptions uiOptions;

        //disable warning about not really being async
        // we know it is not, it is not needed to hit the db in these
#pragma warning disable 1998

        // GET: /SiteAdmin
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Site Administration";
            ViewData["Heading"] = "Site Administration";
            //ViewModels.SiteMapTreeBuilder builder = new ViewModels.SiteMapTreeBuilder();
            //object o = builder.GetTree();
            return View();


        }

#pragma warning restore 1998

        [HttpGet]
        [Authorize(Roles = "ServerAdmins")]
        public async Task<IActionResult> SiteList(int pageNumber = 1, int pageSize = -1)
        {
            ViewData["Title"] = "Site List";
            ViewData["Heading"] = "Site List";

            int itemsPerPage = uiOptions.DefaultPageSize_SiteList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }


            int filteredSiteId = -1; //nothing filtered
            var sites = await siteManager.GetPageOtherSites(
                filteredSiteId,
                pageNumber,
                itemsPerPage);

            var count = await siteManager.CountOtherSites(filteredSiteId);

            SiteListViewModel model = new SiteListViewModel();
            model.Heading = "Site List";
            model.Sites = sites;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = count;

            return View(model);


        }

        // GET: /SiteAdmin/SiteInfo
        [HttpGet]
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> SiteInfo(
            Guid? siteGuid,
            int slp = 1)
        {
            ViewData["Title"] = "Site Settings";

            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteGuid.HasValue) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteGuid.Value);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
            }

            SiteBasicSettingsViewModel model = new SiteBasicSettingsViewModel();
            model.ReturnPageNumber = slp; // site list page number to return to
            model.AllTimeZones = DateTimeHelper.GetTimeZoneList();

            model.SiteId = selectedSite.SiteId;
            model.SiteGuid = selectedSite.SiteGuid;
            model.SiteName = selectedSite.SiteName;
            
            model.TimeZoneId = selectedSite.TimeZoneId;
            
            model.IsClosed = selectedSite.SiteIsClosed;
            model.ClosedMessage = selectedSite.SiteIsClosedMessage;
            
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                model.SiteFolderName = selectedSite.SiteFolderName;
            }
            else if (multiTenantOptions.Mode == MultiTenantMode.HostName)
            {
                model.HostName = selectedSite.PreferredHostName;
            }

            
            // can only delete from server admin site/cannot delete server admin site
            if (siteManager.CurrentSite.IsServerAdminSite)
            {
                if (model.SiteGuid != siteManager.CurrentSite.SiteGuid)
                {
                    model.ShowDelete = uiOptions.AllowDeleteChildSites;
                }
            }

           
            //dpBeginDate.Text = blog.StartDate.ToLocalTime(timeZone).ToString("g");
            //TimeZoneInfo timeZone = DateTimeHelper.GetTimeZone(model.AllTimeZones, "Eastern Standard Time");

            //model.TmpDate = DateTime.UtcNow.ToLocalTime(timeZone);

            //string timeFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ToDatePickerWithTimeFormat();
            //string timeFormat = "h:mm:ss TT";
            //ViewData.Add("TimeFormat", timeFormat);

            //ViewBag.TimeFormat = timeFormat;

            //string dateFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ToDatePickerFormat();

            //ViewBag.DateFormat = dateFormat;

            return View(model);
        }




        // Post: /SiteAdmin/SiteInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> SiteInfo(SiteBasicSettingsViewModel model)
        {
            // can only delete from server admin site/cannot delete server admin site
            if (siteManager.CurrentSite.IsServerAdminSite)
            {
                if (model.SiteGuid != siteManager.CurrentSite.SiteGuid)
                {
                    model.ShowDelete = uiOptions.AllowDeleteChildSites;
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteGuid == Guid.Empty)
            {
                this.AlertDanger("oops something went wrong, site was not found.", true);

                return RedirectToAction("Index");
            }

            //model.SiteId = Site.SiteSettings.SiteId;
            //model.SiteGuid = Site.SiteSettings.SiteGuid;
            ISiteSettings selectedSite = null;
            if (model.SiteGuid == siteManager.CurrentSite.SiteGuid)
            {
                selectedSite = siteManager.CurrentSite;
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                selectedSite = await siteManager.Fetch(model.SiteGuid);
            }

            if (selectedSite == null)
            {
                this.AlertDanger("oops something went wrong.", true);

                return RedirectToAction("Index");
            }

            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if (
                    ((model.SiteFolderName == null) || (model.SiteFolderName.Length == 0))
                    && (!selectedSite.IsServerAdminSite)
                    )
                {
                    // only the server admin site can be without a folder
                    ModelState.AddModelError("foldererror", "Folder name is required.");

                    return View(model);
                }

                SiteFolder folder = await siteManager.GetSiteFolder(model.SiteFolderName);
                if ((folder != null) && (folder.SiteGuid != selectedSite.SiteGuid))
                {
                    ModelState.AddModelError("foldererror", "The selected folder name is already in use on another site.");

                    return View(model);
                }

            }
            else if (multiTenantOptions.Mode == MultiTenantMode.HostName)
            {
                ISiteHost host;

                if (!string.IsNullOrEmpty(model.HostName))
                {
                    model.HostName = model.HostName.Replace("https://", string.Empty).Replace("http://", string.Empty);

                    host = await siteManager.GetSiteHost(model.HostName);

                    if (host != null)
                    {
                        if (host.SiteGuid != selectedSite.SiteGuid)
                        {
                            ModelState.AddModelError("hosterror", "The selected host/domain name is already in use on another site.");

                            return View(model);
                        }

                    }
                    else
                    {
                        bool hostResult = await siteManager.AddHost(
                            selectedSite.SiteGuid,
                            selectedSite.SiteId,
                            model.HostName);
                    }
                }


                selectedSite.PreferredHostName = model.HostName;

            }
            
            selectedSite.SiteName = model.SiteName;
            selectedSite.TimeZoneId = model.TimeZoneId;
            selectedSite.SiteFolderName = model.SiteFolderName;
            selectedSite.SiteIsClosed = model.IsClosed;
            selectedSite.SiteIsClosedMessage = model.ClosedMessage;
            
            bool result = await siteManager.Save(selectedSite);

            if ((result) && (multiTenantOptions.Mode == MultiTenantMode.FolderName))
            {
                if (!string.IsNullOrEmpty(selectedSite.SiteFolderName))
                {
                    bool folderEnsured = await siteManager.EnsureSiteFolder(selectedSite);
                }

            }

            if (result)
            {
                this.AlertSuccess(string.Format("Basic site settings for <b>{0}</b> were successfully updated.",
                            selectedSite.SiteName), true);
            }


            if ((siteManager.CurrentSite.IsServerAdminSite)
                //&&(Site.SiteSettings.SiteGuid != selectedSite.SiteGuid)
                )
            {
                // just edited from site list so redirect there
                return RedirectToAction("SiteList", new { pageNumber = model.ReturnPageNumber });
            }

            return RedirectToAction("Index");

        }

        // GET: /SiteAdmin/NewSite
        [HttpGet]
        [Authorize(Roles = "ServerAdmins")]
        public ActionResult NewSite(int slp = 1)
        {
            ViewData["Title"] = "Create New Site";

            SiteBasicSettingsViewModel model = new SiteBasicSettingsViewModel();
            model.ReturnPageNumber = slp; //site list return page
            model.SiteId = -1;
            model.SiteGuid = Guid.Empty;
            // model.SiteName = Site.SiteSettings.SiteName;
            
            model.TimeZoneId = siteManager.CurrentSite.TimeZoneId;
            model.AllTimeZones = DateTimeHelper.GetTimeZoneList();
            
            

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ServerAdmins")]
        public async Task<ActionResult> NewSite(SiteBasicSettingsViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            bool addHostName = false;

            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if (string.IsNullOrEmpty(model.SiteFolderName))
                {
                    ModelState.AddModelError("foldererror", "Folder name is required.");

                    return View(model);
                }

                SiteFolder folder = await siteManager.GetSiteFolder(model.SiteFolderName);
                if (folder != null)
                {
                    ModelState.AddModelError("foldererror", "The selected folder name is already in use on another site.");

                    return View(model);
                }

            }
            else
            {
                ISiteHost host;

                if (!string.IsNullOrEmpty(model.HostName))
                {
                    model.HostName = model.HostName.Replace("https://", string.Empty).Replace("http://", string.Empty);

                    host = await siteManager.GetSiteHost(model.HostName);

                    if (host != null)
                    {

                        ModelState.AddModelError("hosterror", "The selected host/domain name is already in use on another site.");

                        return View(model);

                    }

                    addHostName = true;
                }




            }

            SiteSettings newSite = new SiteSettings();

            // only the first site created by setup page should be a server admin site
            newSite.IsServerAdminSite = false;

            newSite.SiteName = model.SiteName;
            
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                newSite.SiteFolderName = model.SiteFolderName;
            }
            else if (addHostName)
            {
                newSite.PreferredHostName = model.HostName;
            }

            newSite.SiteIsClosed = model.IsClosed;
            newSite.SiteIsClosedMessage = model.ClosedMessage;
            
            //Site.SiteRepository.Save(newSite);
            bool result = await siteManager.CreateNewSite(newSite);
            result = await siteManager.CreateRequiredRolesAndAdminUser(newSite);

            if ((result) && (multiTenantOptions.Mode == MultiTenantMode.FolderName))
            {
                bool folderResult = await siteManager.EnsureSiteFolder(newSite);

            // for folder sites we need routes that match the folder
            // which are normally created during app startup
            // can we add routes here? or do we need to force the app to recycle?
            // this seems to work, but we really do need to restart
            // so that the per folder authentication gets setup too
            //cloudscribe.Web.Routing.RouteRegistrar.AddDefaultRouteForNewSiteFolder(folder.FolderName);

            //startup.TriggerStartup();
            //http://stackoverflow.com/questions/31339896/replacement-httpruntime-unloadappdomain-in-asp-net-5

            }

            if (result && addHostName)
            {
                bool hostResult = await siteManager.AddHost(
                            newSite.SiteGuid,
                            newSite.SiteId,
                            model.HostName);
            }

            if (result)
            {
                this.AlertSuccess(string.Format("Basic site settings for <b>{0}</b> were successfully created.",
                           newSite.SiteName), true);

            }

            return RedirectToAction("SiteList", new { pageNumber = model.ReturnPageNumber });

        }

        [HttpGet]
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> CompanyInfo(
            Guid? siteGuid,
            int slp = 1)
        {
            

            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteGuid.HasValue) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteGuid.Value);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
            }

            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Company Info", selectedSite.SiteName);

            CompanyInfoViewModel model = new CompanyInfoViewModel();
            model.SiteGuid = selectedSite.SiteGuid;
            model.SiteId = selectedSite.SiteId;
            model.Slogan = selectedSite.Slogan;
            model.CompanyName = selectedSite.CompanyName;
            model.CompanyStreetAddress = selectedSite.CompanyStreetAddress;
            model.CompanyStreetAddress2 = selectedSite.CompanyStreetAddress2;
            model.CompanyLocality = selectedSite.CompanyLocality;
            model.CompanyRegion = selectedSite.CompanyRegion;
            model.CompanyPostalCode = selectedSite.CompanyPostalCode;
            model.CompanyCountry = selectedSite.CompanyCountry;
            model.CompanyPhone = selectedSite.CompanyPhone;
            model.CompanyFax = selectedSite.CompanyFax;
            model.CompanyPublicEmail = selectedSite.CompanyPublicEmail;

            model.AvailableCountries.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var countries = await geoDataManager.GetAllCountries();
            Guid selectedCountryGuid = Guid.Empty;
            foreach (var country in countries)
            {
                if (country.ISOCode2 == model.CompanyCountry)
                {
                    selectedCountryGuid = country.Guid;
                }
                model.AvailableCountries.Add(new SelectListItem()
                {
                    Text = country.Name,
                    Value = country.ISOCode2.ToString()

                });
            }

            if (selectedCountryGuid != Guid.Empty)
            {
                var states = await geoDataManager.GetGeoZonesByCountry(selectedCountryGuid);
                foreach (var state in states)
                {
                    model.AvailableStates.Add(new SelectListItem()
                    {
                        Text = state.Name,
                        Value = state.Code
                    });
                }

            }


            return View(model);

        }

        // Post: /SiteAdmin/CompanyInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> CompanyInfo(CompanyInfoViewModel model)
        {
           
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteGuid == Guid.Empty)
            {
                this.AlertDanger("oops something went wrong, site was not found.", true);

                return RedirectToAction("Index");
            }

            //model.SiteId = Site.SiteSettings.SiteId;
            //model.SiteGuid = Site.SiteSettings.SiteGuid;
            ISiteSettings selectedSite = null;
            if (model.SiteGuid == siteManager.CurrentSite.SiteGuid)
            {
                selectedSite = siteManager.CurrentSite;
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                selectedSite = await siteManager.Fetch(model.SiteGuid);
            }

            if (selectedSite == null)
            {
                this.AlertDanger("oops something went wrong.", true);

                return RedirectToAction("Index");
            }

            selectedSite.Slogan = model.Slogan;
            selectedSite.CompanyName = model.CompanyName;
            selectedSite.CompanyStreetAddress = model.CompanyStreetAddress;
            selectedSite.CompanyStreetAddress2 = model.CompanyStreetAddress2;
            selectedSite.CompanyLocality = model.CompanyLocality;
            selectedSite.CompanyRegion = model.CompanyRegion;
            selectedSite.CompanyPostalCode = model.CompanyPostalCode;
            selectedSite.CompanyCountry = model.CompanyCountry;
            selectedSite.CompanyPhone = model.CompanyPhone;
            selectedSite.CompanyFax = model.CompanyFax;
            selectedSite.CompanyPublicEmail = model.CompanyPublicEmail;

            bool result = await siteManager.Save(selectedSite);
            
            if (result)
            {
                this.AlertSuccess(string.Format("Company Info for <b>{0}</b> wwas successfully updated.",
                            selectedSite.SiteName), true);
            }


            if ((siteManager.CurrentSite.IsServerAdminSite)
                && (siteManager.CurrentSite.SiteGuid != selectedSite.SiteGuid)
                )
            {
                
                return RedirectToAction("CompanyInfo", new { siteGuid = model.SiteGuid });
            }

            return RedirectToAction("CompanyInfo");

        }

        [HttpGet]
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> Captcha(
            Guid? siteGuid,
            int slp = 1)
        {
            
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteGuid.HasValue) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteGuid.Value);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
            }

            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Captcha Settings", selectedSite.SiteName);

            CaptchaSettingsViewModel model = new CaptchaSettingsViewModel();
            model.SiteGuid = selectedSite.SiteGuid;
            model.SiteId = selectedSite.SiteId;
            model.RecaptchaPrivateKey = selectedSite.RecaptchaPrivateKey;
            model.RecaptchaPublicKey = selectedSite.RecaptchaPublicKey;
            model.RequireCaptchaOnLogin = selectedSite.RequireCaptchaOnLogin;
            model.RequireCaptchaOnRegistration = selectedSite.RequireCaptchaOnRegistration;

            return View("CaptchaSettings", model);
            

        }

        // Post: /SiteAdmin/Captcha
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Captcha(CaptchaSettingsViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteGuid == Guid.Empty)
            {
                this.AlertDanger("oops something went wrong, site was not found.", true);

                return RedirectToAction("Index");
            }

            //model.SiteId = Site.SiteSettings.SiteId;
            //model.SiteGuid = Site.SiteSettings.SiteGuid;
            ISiteSettings selectedSite = null;
            if (model.SiteGuid == siteManager.CurrentSite.SiteGuid)
            {
                selectedSite = siteManager.CurrentSite;
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                selectedSite = await siteManager.Fetch(model.SiteGuid);
            }

            if (selectedSite == null)
            {
                this.AlertDanger("oops something went wrong.", true);

                return RedirectToAction("Index");
            }

            selectedSite.RecaptchaPublicKey = model.RecaptchaPublicKey;
            selectedSite.RecaptchaPrivateKey = model.RecaptchaPrivateKey;
            selectedSite.RequireCaptchaOnRegistration = model.RequireCaptchaOnRegistration;
            selectedSite.RequireCaptchaOnLogin = model.RequireCaptchaOnLogin;

            bool result = await siteManager.Save(selectedSite);

            if (result)
            {
                this.AlertSuccess(string.Format("Captcha Settings for <b>{0}</b> wwas successfully updated.",
                            selectedSite.SiteName), true);
            }


            if ((siteManager.CurrentSite.IsServerAdminSite)
                &&(siteManager.CurrentSite.SiteGuid != selectedSite.SiteGuid)
                )
            {
                
                return RedirectToAction("Captcha", new { siteGuid = model.SiteGuid });
            }

            return RedirectToAction("Captcha");

        }

        // GET: /SiteAdmin/SocialLogins
        [HttpGet]
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> SocialLogins(
            Guid? siteGuid,
            int slp = 1)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteGuid.HasValue) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteGuid.Value);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
            }

            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Social Login Settings", selectedSite.SiteName);

            SocialLoginSettingsViewModel model = new SocialLoginSettingsViewModel();
            model.SiteGuid = selectedSite.SiteGuid;
            model.SiteId = selectedSite.SiteId;
            model.FacebookAppId = selectedSite.FacebookAppId;
            model.FacebookAppSecret = selectedSite.FacebookAppSecret;
            model.GoogleClientId = selectedSite.GoogleClientId;
            model.GoogleClientSecret = selectedSite.GoogleClientSecret;
            model.MicrosoftClientId = selectedSite.MicrosoftClientId;
            model.MicrosoftClientSecret = selectedSite.MicrosoftClientSecret;
            model.TwitterConsumerKey = selectedSite.TwitterConsumerKey;
            model.TwitterConsumerSecret = selectedSite.TwitterConsumerSecret;

            return View(model);

        }

        // Post: /SiteAdmin/SocialLogins
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> SocialLogins(SocialLoginSettingsViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteGuid == Guid.Empty)
            {
                this.AlertDanger("oops something went wrong, site was not found.", true);

                return RedirectToAction("Index");
            }

            //model.SiteId = Site.SiteSettings.SiteId;
            //model.SiteGuid = Site.SiteSettings.SiteGuid;
            ISiteSettings selectedSite = null;
            if (model.SiteGuid == siteManager.CurrentSite.SiteGuid)
            {
                selectedSite = siteManager.CurrentSite;
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                selectedSite = await siteManager.Fetch(model.SiteGuid);
            }

            if (selectedSite == null)
            {
                this.AlertDanger("oops something went wrong.", true);

                return RedirectToAction("Index");
            }

            selectedSite.FacebookAppId = model.FacebookAppId;
            selectedSite.FacebookAppSecret = model.FacebookAppSecret;
            selectedSite.GoogleClientId = model.GoogleClientId;
            selectedSite.GoogleClientSecret = model.GoogleClientSecret;
            selectedSite.MicrosoftClientId = model.MicrosoftClientId;
            selectedSite.MicrosoftClientSecret = model.MicrosoftClientSecret;
            selectedSite.TwitterConsumerKey = model.TwitterConsumerKey;
            selectedSite.TwitterConsumerSecret = model.TwitterConsumerSecret;

            bool result = await siteManager.Save(selectedSite);

            if (result)
            {
                this.AlertSuccess(string.Format("Social Login Settings for <b>{0}</b> wwas successfully updated.",
                            selectedSite.SiteName), true);
            }


            if ((siteManager.CurrentSite.IsServerAdminSite)
                && (siteManager.CurrentSite.SiteGuid != selectedSite.SiteGuid)
                )
            {

                return RedirectToAction("SocialLogins", new { siteGuid = model.SiteGuid });
            }

            return RedirectToAction("SocialLogins");

        }


        // probably should hide by config by default

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ServerAdmins")]
        public async Task<ActionResult> SiteDelete(Guid siteGuid, int siteId, int returnPageNumber = 1)
        {
            bool result = false;

            ISiteSettings selectedSite = await siteManager.Fetch(siteGuid);

            if (
                (selectedSite != null)
                && (selectedSite.SiteId == siteId)
                )
            {

                if (selectedSite.IsServerAdminSite)
                {
                    this.AlertWarning(string.Format(
                            "The site <b>{0}</b> was not deleted because it is a server admin site.",
                            selectedSite.SiteName)
                            , true);

                    return RedirectToAction("SiteList", new { pageNumber = returnPageNumber });

                }

                result = await siteManager.Delete(selectedSite);
            }

            if (result && (selectedSite != null))
            {
                this.AlertWarning(string.Format(
                            "The site <b>{0}</b> was successfully deleted.",
                            selectedSite.SiteName)
                            , true);
            }

            return RedirectToAction("SiteList", new { pageNumber = returnPageNumber });
        }

        [HttpGet]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> SiteHostMappings(
            Guid? siteGuid,
            int slp = -1)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteGuid.HasValue) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteGuid.Value);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
            }

            SiteHostMappingsViewModel model = new SiteHostMappingsViewModel();

            ViewData["Title"] = string.Format(CultureInfo.InvariantCulture,
                "Domain/Host Name Mappings for {0}",
                selectedSite.SiteName);

            //model.Heading = ViewBag.Title;
            model.HostMappings = await siteManager.GetSiteHosts(selectedSite.SiteId);
            if (slp > -1)
            {
                model.SiteListReturnPageNumber = slp;
            }

            return View("SiteHosts", model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ServerAdmins")]
        public async Task<ActionResult> HostAdd(
            Guid siteGuid,
            int siteId,
            string hostName,
            int slp = -1)
        {
            ISiteSettings selectedSite = await siteManager.Fetch(siteGuid);

            if (selectedSite == null)
            {
                return RedirectToAction("Index");
            }

            ISiteHost host;

            if (!string.IsNullOrEmpty(hostName))
            {
                hostName = hostName.Replace("https://", string.Empty).Replace("http://", string.Empty);

                host = await siteManager.GetSiteHost(hostName);

                if (host != null)
                {
                    if (host.SiteGuid != selectedSite.SiteGuid)
                    {
                        this.AlertWarning(
                        "failed to add the requested host name mapping becuase it is already mapped to another site.",
                        true);
                    }
                }
                else
                {
                    bool hostResult = await siteManager.AddHost(
                            selectedSite.SiteGuid,
                            selectedSite.SiteId,
                            hostName);

                    if (hostResult)
                    {
                        this.AlertSuccess(string.Format("Host/domain mapping for <b>{0}</b> was successfully created.",
                                   selectedSite.SiteName), true);

                    }
                }


            }

            return RedirectToAction("SiteHostMappings", new { siteGuid = siteGuid, slp = slp });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ServerAdmins")]
        public async Task<ActionResult> HostDelete(
            Guid siteGuid,
            string hostName,
            int slp = -1)
        {
            ISiteSettings selectedSite = await siteManager.Fetch(siteGuid);

            if (selectedSite == null)
            {
                return RedirectToAction("Index");
            }

            ISiteHost host = await siteManager.GetSiteHost(hostName);

            bool result = false;

            if (host != null)
            {
                if (host.SiteGuid == selectedSite.SiteGuid)
                {
                    if (selectedSite.PreferredHostName == host.HostName)
                    {
                        selectedSite.PreferredHostName = string.Empty;
                        result = await siteManager.Save(selectedSite);
                    }

                    result = await siteManager.DeleteHost(host.HostId);

                    if (result)
                    {
                        this.AlertSuccess(string.Format("Host/domain mapping for <b>{0}</b> was successfully removed.",
                                   selectedSite.SiteName), true);
                    }

                }

            }


            return RedirectToAction("SiteHostMappings", new { siteGuid = siteGuid, slp = slp });

        }

        
    }
}
