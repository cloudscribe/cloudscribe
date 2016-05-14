// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2016-05-11
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Helpers;
using cloudscribe.Core.Web.ViewModels.SiteSettings;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Common.Razor;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Localization;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;


namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class SiteAdminController : CloudscribeBaseController
    {
        public SiteAdminController(
            SiteManager siteManager,
            GeoDataManager geoDataManager,
            IOptions<MultiTenantOptions> multiTenantOptions,
            IOptions<UIOptions> uiOptionsAccessor,
           // IOptions<LayoutSelectorOptions> layoutSeletorOptionsAccessor,
            IThemeListBuilder layoutListBuilder
            )
        {
            if (siteManager == null) { throw new ArgumentNullException(nameof(siteManager)); }
            if (geoDataManager == null) { throw new ArgumentNullException(nameof(geoDataManager)); }
            if (multiTenantOptions == null) { throw new ArgumentNullException(nameof(multiTenantOptions)); }

            this.multiTenantOptions = multiTenantOptions.Value;
            
            this.siteManager = siteManager;
            this.geoDataManager = geoDataManager;
            uiOptions = uiOptionsAccessor.Value;
            this.layoutListBuilder = layoutListBuilder;
            //layoutOptions = layoutSeletorOptionsAccessor.Value;

            //startup = startupTrigger;
        }

        private SiteManager siteManager;
        private GeoDataManager geoDataManager;
        private MultiTenantOptions multiTenantOptions;
        //private ITriggerStartup startup
        //private LayoutSelectorOptions layoutOptions;
        private IThemeListBuilder layoutListBuilder;
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
            // this view just has navigation map
            return View();


        }

        // GET: /SiteAdmin
        [HttpGet]
        public async Task<IActionResult> Security()
        {
            ViewData["Title"] = "Security Settings";
            ViewData["Heading"] = "Security Settings";
            // this view just has navigation map
            return View("Index");


        }

#pragma warning restore 1998

        [HttpGet]
        [Authorize(Policy = "ServerAdminPolicy")]
        public async Task<IActionResult> SiteList(int pageNumber = 1, int pageSize = -1)
        {
            ViewData["Title"] = "Site List";
            ViewData["Heading"] = "Site List";

            int itemsPerPage = uiOptions.DefaultPageSize_SiteList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }


            Guid filteredSiteId = Guid.Empty; //nothing filtered
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
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> SiteInfo(
            Guid? siteId,
            int slp = 1)
        {
            
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Settings", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Site Settings";
            }
            
            SiteBasicSettingsViewModel model = new SiteBasicSettingsViewModel();
            model.ReturnPageNumber = slp; // site list page number to return to

            //model.AllTimeZones = DateTimeHelper.GetTimeZoneList();
            model.AllTimeZones = DateTimeHelper.GetTimeZoneList().Select(x =>
                               new SelectListItem
                               {
                                   Text = x.DisplayName,
                                   Value = x.Id
                                   ,
                                   Selected = model.TimeZoneId == x.Id
                               });

            
            model.SiteId = selectedSite.Id;
            model.SiteName = selectedSite.SiteName;
            if(selectedSite.AliasId != selectedSite.Id.ToString())
            {
                model.AliasId = selectedSite.AliasId;
            }
            
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
            
            model.Theme = selectedSite.Theme;
            model.AvailableLayouts = layoutListBuilder.GetAvailableThemes();
            
            // can only delete from server admin site/cannot delete server admin site
            if (siteManager.CurrentSite.IsServerAdminSite)
            {
                if (model.SiteId != siteManager.CurrentSite.Id)
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
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> SiteInfo(SiteBasicSettingsViewModel model)
        {
            // can only delete from server admin site/cannot delete server admin site
            if (siteManager.CurrentSite.IsServerAdminSite)
            {
                if (model.SiteId != siteManager.CurrentSite.Id)
                {
                    model.ShowDelete = uiOptions.AllowDeleteChildSites;
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger("oops something went wrong, site was not found.", true);

                return RedirectToAction("Index");
            }

            //model.SiteId = Site.SiteSettings.SiteId;
            //model.SiteGuid = Site.SiteSettings.SiteGuid;
            ISiteSettings selectedSite = null;
            if (model.SiteId == siteManager.CurrentSite.Id)
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Site Settings";
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                selectedSite = await siteManager.Fetch(model.SiteId);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Settings", selectedSite.SiteName);
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

                //TODO: since we removed the SiteFolders table, now we need to implement looking up the site directly from the
                // Sites table SiteFolderName field

                //ISiteFolder folder = await siteManager.GetSiteFolder(model.SiteFolderName);
                //if ((folder != null) && (folder.SiteGuid != selectedSite.SiteGuid))
                //{
                //    ModelState.AddModelError("foldererror", "The selected folder name is already in use on another site.");

                //    return View(model);
                //}

                bool folderAvailable = await siteManager.FolderNameIsAvailable(selectedSite, model.SiteFolderName);
                if (!folderAvailable)
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
                        if (host.SiteId != selectedSite.Id)
                        {
                            ModelState.AddModelError("hosterror", "The selected host/domain name is already in use on another site.");

                            return View(model);
                        }

                    }
                    else
                    {
                        await siteManager.AddHost(
                            selectedSite.Id,
                            model.HostName);
                    }
                }


                selectedSite.PreferredHostName = model.HostName;

            }

            if (model.AliasId.Length > 0) selectedSite.AliasId = model.AliasId;
            selectedSite.SiteName = model.SiteName;
            selectedSite.TimeZoneId = model.TimeZoneId;
            selectedSite.SiteFolderName = model.SiteFolderName;
            selectedSite.SiteIsClosed = model.IsClosed;
            selectedSite.SiteIsClosedMessage = model.ClosedMessage;
            selectedSite.Theme = model.Theme;
            
            
            await siteManager.Save(selectedSite);

            //if ((result) && (multiTenantOptions.Mode == MultiTenantMode.FolderName))
            //{
            //    if (!string.IsNullOrEmpty(selectedSite.SiteFolderName))
            //    {
            //        bool folderEnsured = await siteManager.EnsureSiteFolder(selectedSite);
            //    }

            //}

            
            this.AlertSuccess(string.Format("Basic site settings for <b>{0}</b> were successfully updated.",
                        selectedSite.SiteName), true);
            


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
        [Authorize(Policy = "ServerAdminPolicy")]
        public ActionResult NewSite(int slp = 1)
        {
            ViewData["Title"] = "Create New Site";

            SiteBasicSettingsViewModel model = new SiteBasicSettingsViewModel();
            model.ReturnPageNumber = slp; //site list return page
            model.SiteId = Guid.Empty;
            // model.SiteName = Site.SiteSettings.SiteName;
            
            model.TimeZoneId = siteManager.CurrentSite.TimeZoneId;
            //model.AllTimeZones = DateTimeHelper.GetTimeZoneList();
            model.AllTimeZones = DateTimeHelper.GetTimeZoneList().Select(x =>
                               new SelectListItem
                               {
                                   Text = x.DisplayName,
                                   Value = x.Id
                                   ,
                                   Selected = model.TimeZoneId == x.Id
                               });



            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ServerAdminPolicy")]
        public async Task<ActionResult> NewSite(SiteBasicSettingsViewModel model)
        {
            ViewData["Title"] = "Create New Site";

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            bool addHostName = false;
            SiteSettings newSite = new SiteSettings();
            newSite.Id = Guid.NewGuid();

            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if (string.IsNullOrEmpty(model.SiteFolderName))
                {
                    ModelState.AddModelError("foldererror", "Folder name is required.");

                    return View(model);
                }

                //TODO: since we removed the sitefolders table now we need to check against the sites table to make sure a folder name is not in use

                //ISiteFolder folder = await siteManager.GetSiteFolder(model.SiteFolderName);
                bool folderAvailable = await siteManager.FolderNameIsAvailable(newSite, model.SiteFolderName);
                if (!folderAvailable)
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

            

            // only the first site created by setup page should be a server admin site
            newSite.IsServerAdminSite = false;
            newSite.SiteName = model.SiteName;
            if (!string.IsNullOrWhiteSpace(model.AliasId))
            {
                newSite.AliasId = model.AliasId;
            }
            else
            {
                var siteNumber = 1 + await siteManager.CountOtherSites(Guid.Empty);
                newSite.AliasId = $"tenant-{siteNumber}";
            }

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
            await siteManager.CreateNewSite(newSite);
            await siteManager.CreateRequiredRolesAndAdminUser(newSite);

            //if ((result) && (multiTenantOptions.Mode == MultiTenantMode.FolderName))
            //{
            //    //bool folderResult = await siteManager.EnsureSiteFolder(newSite);

            //// for folder sites we need routes that match the folder
            //// which are normally created during app startup
            //// can we add routes here? or do we need to force the app to recycle?
            //// this seems to work, but we really do need to restart
            //// so that the per folder authentication gets setup too
            ////cloudscribe.Web.Routing.RouteRegistrar.AddDefaultRouteForNewSiteFolder(folder.FolderName);

            ////startup.TriggerStartup();
            ////http://stackoverflow.com/questions/31339896/replacement-httpruntime-unloadappdomain-in-asp-net-5

            //}

            if (addHostName)
            {
                await siteManager.AddHost(
                            newSite.Id,
                            model.HostName);
            }
            
            this.AlertSuccess(string.Format("Basic site settings for <b>{0}</b> were successfully created.",
                        newSite.SiteName), true);
            
            return RedirectToAction("SiteList", new { pageNumber = model.ReturnPageNumber });

        }

        public async Task<JsonResult> AliasIdAvailable(Guid? siteId, string aliasId)
        {
            var selectedSiteId = Guid.Empty;
            if (siteId.HasValue) { selectedSiteId = siteId.Value; }
            bool available = await siteManager.AliasIdIsAvailable(selectedSiteId, aliasId);


            return Json(available);
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CompanyInfo(
            Guid? siteId,
            int slp = 1)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Company Info", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Company Info";
            }
            
            var model = new CompanyInfoViewModel();
            model.SiteId = selectedSite.Id;
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
            var selectedCountryGuid = Guid.Empty;
            foreach (var country in countries)
            {
                if (country.ISOCode2 == model.CompanyCountry)
                {
                    selectedCountryGuid = country.Id;
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
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> CompanyInfo(CompanyInfoViewModel model)
        {
            ISiteSettings selectedSite = null;
            if (model.SiteId == siteManager.CurrentSite.Id)
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Company Info";
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                selectedSite = await siteManager.Fetch(model.SiteId);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Company Info", selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger("oops something went wrong.", true);

                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger("oops something went wrong, site was not found.", true);

                return RedirectToAction("Index");
            }
            
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

            await siteManager.Save(selectedSite);
            
            this.AlertSuccess(string.Format("Company Info for <b>{0}</b> wwas successfully updated.",
                        selectedSite.SiteName), true);
            
            if ((siteManager.CurrentSite.IsServerAdminSite)
                && (siteManager.CurrentSite.Id != selectedSite.Id)
                )
            {
                
                return RedirectToAction("CompanyInfo", new { siteId = model.SiteId });
            }

            return RedirectToAction("CompanyInfo");

        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> MailSettings(
            Guid? siteId,
            int slp = 1)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Email Settings", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Email Settings";
            }

            var model = new MailSettingsViewModel();
            model.SiteId = selectedSite.Id;
            model.DefaultEmailFromAddress = selectedSite.DefaultEmailFromAddress;
            model.DefaultEmailFromAlias = selectedSite.DefaultEmailFromAlias;
            model.SmtpPassword = selectedSite.SmtpPassword;
            model.SmtpPort = selectedSite.SmtpPort;
            model.SmtpPreferredEncoding = selectedSite.SmtpPreferredEncoding;
            model.SmtpRequiresAuth = selectedSite.SmtpRequiresAuth;
            model.SmtpServer = selectedSite.SmtpServer;
            model.SmtpUser = selectedSite.SmtpUser;
            model.SmtpUseSsl = selectedSite.SmtpUseSsl;
            
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> MailSettings(MailSettingsViewModel model)
        {
            ISiteSettings selectedSite = null;
            if (model.SiteId == siteManager.CurrentSite.Id)
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Email Settings";
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                selectedSite = await siteManager.Fetch(model.SiteId);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Email Settings", selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger("oops something went wrong.", true);

                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger("oops something went wrong, site was not found.", true);

                return RedirectToAction("Index");
            }

            
            selectedSite.DefaultEmailFromAddress = model.DefaultEmailFromAddress;
            selectedSite.DefaultEmailFromAlias = model.DefaultEmailFromAlias;
            selectedSite.SmtpPassword = model.SmtpPassword;
            selectedSite.SmtpPort = model.SmtpPort;
            selectedSite.SmtpPreferredEncoding = model.SmtpPreferredEncoding;
            selectedSite.SmtpRequiresAuth = model.SmtpRequiresAuth;
            selectedSite.SmtpServer = model.SmtpServer;
            selectedSite.SmtpUser = model.SmtpUser;
            selectedSite.SmtpUseSsl = model.SmtpUseSsl;
            
            await siteManager.Save(selectedSite);
            
            this.AlertSuccess(string.Format("Email Settings for <b>{0}</b> were successfully updated.",
                        selectedSite.SiteName), true);
            
            if ((siteManager.CurrentSite.IsServerAdminSite)
                && (siteManager.CurrentSite.Id != selectedSite.Id)
                )
            {

                return RedirectToAction("MailSettings", new { siteId = model.SiteId });
            }

            return RedirectToAction("MailSettings");

        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> SmsSettings(
            Guid? siteId,
            int slp = 1)
        {

            //TODO: we need a way to plugin different sms providers by dependency injection
            // and indicate which one is being used in the UI
            // currently using generic labels but only supports twilio

            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - SMS Settings", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "SMS Settings";
            }

            var model = new SmsSettingsViewModel();
            model.SiteId = selectedSite.Id;
            model.SmsFrom = selectedSite.SmsFrom;
            model.SmsClientId = selectedSite.SmsClientId;
            model.SmsSecureToken = selectedSite.SmsSecureToken;
            
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> SmsSettings(SmsSettingsViewModel model)
        {
            ISiteSettings selectedSite = null;
            if (model.SiteId == siteManager.CurrentSite.Id)
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "SMS Settings";
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                selectedSite = await siteManager.Fetch(model.SiteId);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - SMS Settings", selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger("oops something went wrong.", true);

                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger("oops something went wrong, site was not found.", true);

                return RedirectToAction("Index");
            }
            
            selectedSite.SmsFrom = model.SmsFrom;
            selectedSite.SmsClientId = model.SmsClientId;
            selectedSite.SmsSecureToken = model.SmsSecureToken;
            
            await siteManager.Save(selectedSite);
            
            this.AlertSuccess(string.Format("SMS Settings for <b>{0}</b> were successfully updated.",
                        selectedSite.SiteName), true);
            
            if ((siteManager.CurrentSite.IsServerAdminSite)
                && (siteManager.CurrentSite.Id != selectedSite.Id)
                )
            {

                return RedirectToAction("SmsSettings", new { siteId = model.SiteId });
            }

            return RedirectToAction("SmsSettings");

        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> SecuritySettings(
            Guid? siteId,
            int slp = 1)
        {

            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Security Settings", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Security Settings";
            }

            var model = new SecuritySettingsViewModel();
            model.SiteId = selectedSite.Id;
            model.AllowNewRegistration = selectedSite.AllowNewRegistration;
            model.AllowPersistentLogin = selectedSite.AllowPersistentLogin;
            model.DisableDbAuth = selectedSite.DisableDbAuth;
            model.ReallyDeleteUsers = selectedSite.ReallyDeleteUsers;
            model.RequireApprovalBeforeLogin = selectedSite.RequireApprovalBeforeLogin;
            model.RequireConfirmedEmail = selectedSite.RequireConfirmedEmail;
            model.UseEmailForLogin = selectedSite.UseEmailForLogin;
            model.RequireConfirmedPhone = selectedSite.RequireConfirmedPhone;
            model.AccountApprovalEmailCsv = selectedSite.AccountApprovalEmailCsv;

            return View(model);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> SecuritySettings(SecuritySettingsViewModel model)
        {
            ISiteSettings selectedSite = null;
            if (model.SiteId == siteManager.CurrentSite.Id)
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Security Settings";
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                selectedSite = await siteManager.Fetch(model.SiteId);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Security Settings", selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger("oops something went wrong.", true);

                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger("oops something went wrong, site was not found.", true);

                return RedirectToAction("Index");
            }

            selectedSite.AccountApprovalEmailCsv = model.AccountApprovalEmailCsv;
            selectedSite.AllowNewRegistration = model.AllowNewRegistration;
            selectedSite.AllowPersistentLogin = model.AllowPersistentLogin;
            selectedSite.DisableDbAuth = model.DisableDbAuth;
            selectedSite.ReallyDeleteUsers = model.ReallyDeleteUsers;
            selectedSite.RequireApprovalBeforeLogin = model.RequireApprovalBeforeLogin;
            selectedSite.RequireConfirmedEmail = model.RequireConfirmedEmail;
            selectedSite.RequireConfirmedPhone = model.RequireConfirmedPhone;
            selectedSite.UseEmailForLogin = model.UseEmailForLogin;


            await siteManager.Save(selectedSite);

            
            this.AlertSuccess(string.Format("Security Settings for <b>{0}</b> wwas successfully updated.",
                        selectedSite.SiteName), true);
            


            if ((siteManager.CurrentSite.IsServerAdminSite)
                && (siteManager.CurrentSite.Id != selectedSite.Id)
                )
            {

                return RedirectToAction("SecuritySettings", new { siteId = model.SiteId });
            }

            return RedirectToAction("SecuritySettings");

        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Captcha(
            Guid? siteId,
            int slp = 1)
        {
            
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Captcha Settings", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Captcha Settings";
            }
            
            var model = new CaptchaSettingsViewModel();
            model.SiteId = selectedSite.Id;
            model.RecaptchaPrivateKey = selectedSite.RecaptchaPrivateKey;
            model.RecaptchaPublicKey = selectedSite.RecaptchaPublicKey;
            model.RequireCaptchaOnLogin = selectedSite.CaptchaOnLogin;
            model.RequireCaptchaOnRegistration = selectedSite.CaptchaOnRegistration;

            return View("CaptchaSettings", model);
            

        }

        // Post: /SiteAdmin/Captcha
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> Captcha(CaptchaSettingsViewModel model)
        {
            ISiteSettings selectedSite = null;
            if (model.SiteId == siteManager.CurrentSite.Id)
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Captcha Settings";
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                selectedSite = await siteManager.Fetch(model.SiteId);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Captcha Settings", selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger("oops something went wrong.", true);

                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger("oops something went wrong, site was not found.", true);

                return RedirectToAction("Index");
            }

            //model.SiteId = Site.SiteSettings.SiteId;
            //model.SiteGuid = Site.SiteSettings.SiteGuid;
            

            selectedSite.RecaptchaPublicKey = model.RecaptchaPublicKey;
            selectedSite.RecaptchaPrivateKey = model.RecaptchaPrivateKey;
            selectedSite.CaptchaOnRegistration = model.RequireCaptchaOnRegistration;
            selectedSite.CaptchaOnLogin = model.RequireCaptchaOnLogin;

            await siteManager.Save(selectedSite);
            
            this.AlertSuccess(string.Format("Captcha Settings for <b>{0}</b> wwas successfully updated.",
                        selectedSite.SiteName), true);
            
            if ((siteManager.CurrentSite.IsServerAdminSite)
                &&(siteManager.CurrentSite.Id != selectedSite.Id)
                )
            {
                
                return RedirectToAction("Captcha", new { siteId = model.SiteId });
            }

            return RedirectToAction("Captcha");

        }

        // GET: /SiteAdmin/SocialLogins
        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> SocialLogins(
            Guid? siteId,
            int slp = 1)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Social Login Settings", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Social Login Settings";
            }
            
            var model = new SocialLoginSettingsViewModel();
            model.SiteId = selectedSite.Id;
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
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> SocialLogins(SocialLoginSettingsViewModel model)
        {
            ISiteSettings selectedSite = null;
            if (model.SiteId == siteManager.CurrentSite.Id)
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Social Login Settings";
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                selectedSite = await siteManager.Fetch(model.SiteId);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Social Login Settings", selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger("oops something went wrong.", true);

                return RedirectToAction("Index");
            }


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger("oops something went wrong, site was not found.", true);

                return RedirectToAction("Index");
            }

            //model.SiteId = Site.SiteSettings.SiteId;
            //model.SiteGuid = Site.SiteSettings.SiteGuid;
            

            selectedSite.FacebookAppId = model.FacebookAppId;
            selectedSite.FacebookAppSecret = model.FacebookAppSecret;
            selectedSite.GoogleClientId = model.GoogleClientId;
            selectedSite.GoogleClientSecret = model.GoogleClientSecret;
            selectedSite.MicrosoftClientId = model.MicrosoftClientId;
            selectedSite.MicrosoftClientSecret = model.MicrosoftClientSecret;
            selectedSite.TwitterConsumerKey = model.TwitterConsumerKey;
            selectedSite.TwitterConsumerSecret = model.TwitterConsumerSecret;

            await siteManager.Save(selectedSite);
            
            this.AlertSuccess(string.Format("Social Login Settings for <b>{0}</b> was successfully updated.",
                        selectedSite.SiteName), true);
            
            if ((siteManager.CurrentSite.IsServerAdminSite)
                && (siteManager.CurrentSite.Id != selectedSite.Id)
                )
            {

                return RedirectToAction("SocialLogins", new { siteId = model.SiteId });
            }

            return RedirectToAction("SocialLogins");

        }

        // GET: /SiteAdmin/LoginPageInfo
        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> LoginPageInfo(
            Guid? siteId,
            int slp = 1)
        {

            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Login Page Content", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Login Page Content";
            }
            
            var model = new LoginInfoViewModel();
            model.SiteId = selectedSite.Id;
            model.LoginInfoTop = selectedSite.LoginInfoTop;
            model.LoginInfoBottom = selectedSite.LoginInfoBottom;

            return View(model);


        }

        // Post: /SiteAdmin/LoginPageInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> LoginPageInfo(LoginInfoViewModel model)
        {
            ISiteSettings selectedSite = null;
            if (model.SiteId == siteManager.CurrentSite.Id)
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Login Page Content";
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                selectedSite = await siteManager.Fetch(model.SiteId);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Login Page Content", selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger("oops something went wrong.", true);

                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger("oops something went wrong, site was not found.", true);

                return RedirectToAction("Index");
            }
            
            selectedSite.LoginInfoTop = model.LoginInfoTop;
            selectedSite.LoginInfoBottom = model.LoginInfoBottom;
            
            await siteManager.Save(selectedSite);
            
            this.AlertSuccess(string.Format("Login Page Info for <b>{0}</b> was successfully updated.",
                        selectedSite.SiteName), true);
            
            if ((siteManager.CurrentSite.IsServerAdminSite)
                && (siteManager.CurrentSite.Id != selectedSite.Id)
                )
            {

                return RedirectToAction("LoginPageInfo", new { siteId = model.SiteId });
            }

            return RedirectToAction("LoginPageInfo");

        }

        // GET: /SiteAdmin/RegisterPageInfo
        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> RegisterPageInfo(
            Guid? siteId,
            int slp = 1)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Registration Page Content", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Registration Page Content";
            }
            
            var model = new RegisterInfoViewModel();
            model.SiteId = selectedSite.Id;
            model.RegistrationPreamble = selectedSite.RegistrationPreamble;
            model.RegistrationAgreement = selectedSite.RegistrationAgreement;

            return View(model);

        }


        // Post: /SiteAdmin/RegisterPageInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> RegisterPageInfo(RegisterInfoViewModel model)
        {
            ISiteSettings selectedSite = null;
            if (model.SiteId == siteManager.CurrentSite.Id)
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Registration Page Content";
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                selectedSite = await siteManager.Fetch(model.SiteId);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Registration Page Content", selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger("oops something went wrong.", true);

                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger("oops something went wrong, site was not found.", true);

                return RedirectToAction("Index");
            }
            
            selectedSite.RegistrationPreamble = model.RegistrationPreamble;
            selectedSite.RegistrationAgreement = model.RegistrationAgreement;
            
            await siteManager.Save(selectedSite);
            
            this.AlertSuccess(string.Format("Registration Page Content for <b>{0}</b> was successfully updated.",
                        selectedSite.SiteName), true);
            
            if ((siteManager.CurrentSite.IsServerAdminSite)
                && (siteManager.CurrentSite.Id != selectedSite.Id)
                )
            {

                return RedirectToAction("RegisterPageInfo", new { siteId = model.SiteId });
            }

            return RedirectToAction("RegisterPageInfo");

        }


        // probably should hide by config by default

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ServerAdminPolicy")]
        public async Task<ActionResult> SiteDelete(Guid siteId, int returnPageNumber = 1)
        {
            
            var selectedSite = await siteManager.Fetch(siteId);

            if (
                (selectedSite != null)
                //&& (selectedSite.SiteId == siteId)
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

                await siteManager.Delete(selectedSite);

                this.AlertWarning(string.Format(
                            "The site <b>{0}</b> was successfully deleted.",
                            selectedSite.SiteName)
                            , true);
            }

            

            return RedirectToAction("SiteList", new { pageNumber = returnPageNumber });
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> SiteHostMappings(
            Guid? siteId,
            int slp = -1)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.InvariantCulture,
                "Domain/Host Name Mappings for {0}",
                selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = "Domain/Host Name Mappings";
            }

            var model = new SiteHostMappingsViewModel();
            
            //model.Heading = ViewBag.Title;
            model.HostMappings = await siteManager.GetSiteHosts(selectedSite.Id);
            if (slp > -1)
            {
                model.SiteListReturnPageNumber = slp;
            }

            return View("SiteHosts", model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ServerAdminPolicy")]
        public async Task<ActionResult> HostAdd(
            Guid siteId,
            string hostName,
            int slp = -1)
        {
            var selectedSite = await siteManager.Fetch(siteId);

            if (selectedSite == null)
            {
                return RedirectToAction("Index");
            }

            if(selectedSite.Id != siteManager.CurrentSite.Id && !siteManager.CurrentSite.IsServerAdminSite)
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
                    if (host.SiteId != selectedSite.Id)
                    {
                        this.AlertWarning(
                        "failed to add the requested host name mapping becuase it is already mapped to another site.",
                        true);
                    }
                }
                else
                {
                    await siteManager.AddHost(
                            selectedSite.Id,
                            hostName);
                    
                    this.AlertSuccess(string.Format("Host/domain mapping for <b>{0}</b> was successfully created.",
                                selectedSite.SiteName), true);

                }

            }

            return RedirectToAction("SiteHostMappings", new { siteId = siteId, slp = slp });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ServerAdminPolicy")]
        public async Task<ActionResult> HostDelete(
            Guid siteId,
            string hostName,
            int slp = -1)
        {
            var selectedSite = await siteManager.Fetch(siteId);

            if (selectedSite == null)
            {
                return RedirectToAction("Index");
            }

            if (selectedSite.Id != siteManager.CurrentSite.Id && !siteManager.CurrentSite.IsServerAdminSite)
            {
                return RedirectToAction("Index");
            }

            var host = await siteManager.GetSiteHost(hostName);
            
            if (host != null)
            {
                if (host.SiteId == selectedSite.Id)
                {
                    if (selectedSite.PreferredHostName == host.HostName)
                    {
                        selectedSite.PreferredHostName = string.Empty;
                        await siteManager.Save(selectedSite);
                    }

                    await siteManager.DeleteHost(host.Id);
                    
                    this.AlertSuccess(string.Format("Host/domain mapping for <b>{0}</b> was successfully removed.",
                                selectedSite.SiteName), true);
                }

            }


            return RedirectToAction("SiteHostMappings", new { siteId = siteId, slp = slp });

        }

        
    }
}
