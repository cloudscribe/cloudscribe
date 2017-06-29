// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2017-06-29
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.ViewModels.SiteSettings;
using cloudscribe.Messaging.Email;
using cloudscribe.Web.Common;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Common.Razor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers.Mvc
{
    [Authorize(Policy = "AdminPolicy")]
    public class SiteAdminController : Controller
    {
        public SiteAdminController(
            SiteManager siteManager,
            GeoDataManager geoDataManager,
            ISmtpOptionsProvider smtpOptionsProvider,
            IOptions<MultiTenantOptions> multiTenantOptions,
            IOptions<UIOptions> uiOptionsAccessor,
            IThemeListBuilder layoutListBuilder,
            IStringLocalizer<CloudscribeCore> localizer,
            ITimeZoneHelper timeZoneHelper,
            IOptions<RequestLocalizationOptions> localizationOptions
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
            sr = localizer;
            tzHelper = timeZoneHelper;
            this.smtpOptionsProvider = smtpOptionsProvider;
            localization = localizationOptions.Value;
        }

        private SiteManager siteManager;
        private GeoDataManager geoDataManager;
        private MultiTenantOptions multiTenantOptions;
        private ISmtpOptionsProvider smtpOptionsProvider;
        //private ITriggerStartup startup
        private IStringLocalizer sr;
        private IThemeListBuilder layoutListBuilder;
        private UIOptions uiOptions;
        private ITimeZoneHelper tzHelper;
        private RequestLocalizationOptions localization;

        // GET: /SiteAdmin
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Title"] = sr["Site Administration"];
            // this view just has navigation map
            return View();
        }

        // GET: /SiteAdmin
        [HttpGet]
        public IActionResult Security()
        {
            ViewData["Title"] = sr["Security Settings"];
            // this view just has navigation map
            return View("Index");
        }


        [HttpGet]
        [Authorize(Policy = "ServerAdminPolicy")]
        public async Task<IActionResult> SiteList(int pageNumber = 1, int pageSize = -1)
        {
            ViewData["Title"] = sr["Site List"];

            int itemsPerPage = uiOptions.DefaultPageSize_SiteList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            
            var filteredSiteId = Guid.Empty; //nothing filtered
            var sites = await siteManager.GetPageOtherSites(
                filteredSiteId,
                pageNumber,
                itemsPerPage);

            var count = await siteManager.CountOtherSites(filteredSiteId);
            var model = new SiteListViewModel();
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

            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Settings"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["Site Settings"];
            }
            
            var model = new SiteBasicSettingsViewModel();
            model.ReturnPageNumber = slp; // site list page number to return to
            model.TimeZoneId = selectedSite.TimeZoneId;
            model.AllTimeZones = tzHelper.GetTimeZoneList().Select(x =>
                               new SelectListItem
                               {
                                   Text = x,
                                   Value = x,
                                   Selected = model.TimeZoneId == x
                               });

            model.SiteId = selectedSite.Id;
            model.SiteName = selectedSite.SiteName;
            model.AliasId = selectedSite.AliasId;
            model.GoogleAnalyticsProfileId = selectedSite.GoogleAnalyticsProfileId;
            
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
            model.AvailableThemes = layoutListBuilder.GetAvailableThemes(selectedSite.AliasId);
            
            // can only delete from server admin site/cannot delete server admin site
            if (siteManager.CurrentSite.IsServerAdminSite)
            {
                if (model.SiteId != siteManager.CurrentSite.Id)
                {
                    model.ShowDelete = uiOptions.AllowDeleteChildSites;
                }
            }

            model.ForcedUICulture = selectedSite.ForcedUICulture;
            model.AvailableUICultures = localization.SupportedUICultures
                                      .Select(c => new SelectListItem { Value = c.Name, Text = c.Name , Selected = model.ForcedUICulture == c.Name  })
                                      .ToList();
            model.AvailableUICultures.Insert(0, new SelectListItem { Value = "", Text = sr["Any"] });

            model.ForcedCulture = selectedSite.ForcedCulture;
            model.AvailableCultures = localization.SupportedCultures
                                      .Select(c => new SelectListItem { Value = c.Name, Text = c.Name, Selected = model.ForcedCulture == c.Name })
                                      .ToList();
            model.AvailableCultures.Insert(0, new SelectListItem { Value = "", Text = sr["Any"] });



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
                this.AlertDanger(sr["oops something went wrong, site was not found."], true);

                return RedirectToAction("Index");
            }

            ISiteSettings selectedSite = null;
            if (model.SiteId == siteManager.CurrentSite.Id)
            {
                selectedSite = await siteManager.GetCurrentSiteSettings();
                ViewData["Title"] = sr["Site Settings"];
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                selectedSite = await siteManager.Fetch(model.SiteId);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Settings"], selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger(sr["oops something went wrong."], true);

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
                    ModelState.AddModelError("foldererror", sr["Folder name is required."]);

                    return View(model);
                }
                
                var folderAvailable = await siteManager.FolderNameIsAvailable(selectedSite.Id, model.SiteFolderName);
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
                            ModelState.AddModelError("hosterror", sr["The selected host/domain name is already in use on another site."]);
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

            //if (model.AliasId.Length > 0) selectedSite.AliasId = model.AliasId;
            selectedSite.SiteName = model.SiteName;
            selectedSite.TimeZoneId = model.TimeZoneId;
            selectedSite.SiteFolderName = model.SiteFolderName;
            selectedSite.SiteIsClosed = model.IsClosed;
            selectedSite.SiteIsClosedMessage = model.ClosedMessage;
            selectedSite.Theme = model.Theme;
            selectedSite.GoogleAnalyticsProfileId = model.GoogleAnalyticsProfileId;

            selectedSite.ForcedCulture = model.ForcedCulture;
            selectedSite.ForcedUICulture = model.ForcedUICulture;


            await siteManager.Update(selectedSite);
            
            this.AlertSuccess(string.Format(sr["Basic site settings for {0} were successfully updated."],
                        selectedSite.SiteName), true);
            
            if (siteManager.CurrentSite.IsServerAdminSite)
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
            ViewData["Title"] = sr["Create New Site"];

            var model = new NewSiteViewModel();
            model.ReturnPageNumber = slp; //site list return page
            model.SiteId = Guid.Empty;
            model.TimeZoneId = siteManager.CurrentSite.TimeZoneId;
            model.AllTimeZones = tzHelper.GetTimeZoneList().Select(x =>
                               new SelectListItem
                               {
                                   Text = x,
                                   Value = x,
                                   Selected = model.TimeZoneId == x
                               });
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ServerAdminPolicy")]
        public async Task<ActionResult> NewSite(NewSiteViewModel model)
        {
            ViewData["Title"] = "Create New Site";

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            bool addHostName = false;
            var newSite = new SiteSettings();
            newSite.Id = Guid.NewGuid();

            

            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                //https://github.com/joeaudette/cloudscribe.StarterKits/issues/27
                model.SiteFolderName = model.SiteFolderName.ToLowerInvariant();

                if (string.IsNullOrEmpty(model.SiteFolderName))
                {
                    model.AllTimeZones = tzHelper.GetTimeZoneList().Select(x =>
                               new SelectListItem
                               {
                                   Text = x,
                                   Value = x,
                                   Selected = model.TimeZoneId == x
                               });
                    ModelState.AddModelError("foldererror", sr["Folder name is required."]);
                    return View(model);
                }

                bool folderAvailable = await siteManager.FolderNameIsAvailable(newSite.Id, model.SiteFolderName);
                if (!folderAvailable)
                {
                    model.AllTimeZones = tzHelper.GetTimeZoneList().Select(x =>
                               new SelectListItem
                               {
                                   Text = x,
                                   Value = x,
                                   Selected = model.TimeZoneId == x
                               });
                    ModelState.AddModelError("foldererror", sr["The selected folder name is already in use on another site."]);
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
                        model.AllTimeZones = tzHelper.GetTimeZoneList().Select(x =>
                               new SelectListItem
                               {
                                   Text = x,
                                   Value = x,
                                   Selected = model.TimeZoneId == x
                               });
                        ModelState.AddModelError("hosterror", sr["The selected host/domain name is already in use on another site."]);
                        return View(model);
                    }
                    addHostName = true;
                }

            }
            
            // only the first site created by setup page should be a server admin site
            newSite.IsServerAdminSite = false;
            newSite.SiteName = model.SiteName;
            
            var siteNumber = 1 + await siteManager.CountOtherSites(Guid.Empty);
            newSite.AliasId = $"s{siteNumber}";
            

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
            
            await siteManager.CreateNewSite(newSite);
            await siteManager.CreateRequiredRolesAndAdminUser(
                newSite,
                model.Email,
                model.LoginName,
                model.DisplayName,
                model.Password
                );
            
            if (addHostName)
            {
                await siteManager.AddHost(newSite.Id, model.HostName);
            }
            
            this.AlertSuccess(string.Format(sr["Basic site settings for {0} were successfully created."],
                        newSite.SiteName), true);
            
            return RedirectToAction("SiteList", new { pageNumber = model.ReturnPageNumber });

        }

        [HttpPost]
        public async Task<JsonResult> AliasIdAvailable(Guid? siteId, string aliasId)
        {
            var selectedSiteId = Guid.Empty;
            if (siteId.HasValue) { selectedSiteId = siteId.Value; }
            bool available = await siteManager.AliasIdIsAvailable(selectedSiteId, aliasId);
            return Json(available);
        }

        [HttpPost]
        public async Task<JsonResult> FolderNameAvailable(Guid? siteId, string folderName)
        {            
            var selectedSiteId = Guid.Empty;
            if (siteId.HasValue) { selectedSiteId = siteId.Value; }
            bool available = await siteManager.FolderNameIsAvailable(selectedSiteId, folderName);
            return Json(available);
        }

        [HttpPost]
        public async Task<JsonResult> HostNameAvailable(Guid? siteId, string hostName)
        {
            var selectedSiteId = Guid.Empty;
            if (siteId.HasValue) { selectedSiteId = siteId.Value; }
            bool available = await siteManager.HostNameIsAvailable(selectedSiteId, hostName);
            return Json(available);
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CompanyInfo(
            Guid? siteId,
            int slp = 1)
        {
            var selectedSite = await siteManager.GetSiteForEdit(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Company Info"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["Company Info"];
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

            model.AvailableCountries.Add(new SelectListItem { Text = sr["-Please select-"], Value = "" });
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
            var selectedSite = await siteManager.GetSiteForEdit(model.SiteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id == siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = sr["Company Info"];
            }
            else 
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Company Info"], selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger(sr["oops something went wrong."], true);

                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger(sr["oops something went wrong, site was not found."], true);

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

            await siteManager.Update(selectedSite);
            
            this.AlertSuccess(string.Format(sr["Company Info for {0} wwas successfully updated."],
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
            var selectedSite = await siteManager.GetSiteForEdit(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Email Settings"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["Email Settings"];
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
            var selectedSite = await siteManager.GetSiteForEdit(model.SiteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id == siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = sr["Email Settings"];
            }
            else
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Email Settings"], selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger(sr["oops something went wrong."], true);
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger(sr["oops something went wrong, site was not found."], true);
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
            
            await siteManager.Update(selectedSite);
            
            this.AlertSuccess(string.Format(sr["Email Settings for {0} were successfully updated."],
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

            var selectedSite = await siteManager.GetSiteForEdit(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - SMS Settings"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["SMS Settings"];
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
            var selectedSite = await siteManager.GetSiteForEdit(model.SiteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id == siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = sr["SMS Settings"];
            }
            else 
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - SMS Settings"], selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger(sr["oops something went wrong."], true);
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger(sr["oops something went wrong, site was not found."], true);
                return RedirectToAction("Index");
            }
            
            selectedSite.SmsFrom = model.SmsFrom;
            selectedSite.SmsClientId = model.SmsClientId;
            selectedSite.SmsSecureToken = model.SmsSecureToken;
            
            await siteManager.Update(selectedSite);
            
            this.AlertSuccess(string.Format(sr["SMS Settings for {0} were successfully updated."],
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
            var selectedSite = await siteManager.GetSiteForEdit(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Security Settings"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["Security Settings"];
            }

            var smtpOptions = await smtpOptionsProvider.GetSmtpOptions().ConfigureAwait(false);

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

            model.SmtpIsConfigured = !string.IsNullOrEmpty(smtpOptions.Server);
            model.SmsIsConfigured = selectedSite.SmsIsConfigured();
            model.HasAnySocialAuthEnabled = selectedSite.HasAnySocialAuthEnabled();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> SecuritySettings(SecuritySettingsViewModel model)
        {
            var selectedSite = await siteManager.GetSiteForEdit(model.SiteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id == siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = "Security Settings";
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Security Settings", selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger(sr["oops something went wrong."], true);
                return RedirectToAction("Index");
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger(sr["oops something went wrong, site was not found."], true);
                return RedirectToAction("Index");
            }

            var smtpOptions = await smtpOptionsProvider.GetSmtpOptions().ConfigureAwait(false);

            if (!ModelState.IsValid)
            {
                model.SmtpIsConfigured = !string.IsNullOrEmpty(smtpOptions.Server);
                model.SmsIsConfigured = selectedSite.SmsIsConfigured();
                model.HasAnySocialAuthEnabled = selectedSite.HasAnySocialAuthEnabled();
                return View(model);
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
            
            await siteManager.Update(selectedSite);
            
            this.AlertSuccess(string.Format(sr["Security Settings for {0} wwas successfully updated."],
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
            var selectedSite = await siteManager.GetSiteForEdit(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Captcha Settings"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["Captcha Settings"];
            }
            
            var model = new CaptchaSettingsViewModel();
            model.SiteId = selectedSite.Id;
            model.RecaptchaPrivateKey = selectedSite.RecaptchaPrivateKey;
            model.RecaptchaPublicKey = selectedSite.RecaptchaPublicKey;
            model.UseInvisibleCaptcha = selectedSite.UseInvisibleRecaptcha;
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
            var selectedSite = await siteManager.GetSiteForEdit(model.SiteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id == siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = sr["Captcha Settings"];
            }
            else 
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Captcha Settings"], selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger(sr["oops something went wrong."], true);
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger(sr["oops something went wrong, site was not found."], true);
                return RedirectToAction("Index");
            }

            selectedSite.RecaptchaPublicKey = model.RecaptchaPublicKey;
            selectedSite.RecaptchaPrivateKey = model.RecaptchaPrivateKey;
            selectedSite.UseInvisibleRecaptcha = model.UseInvisibleCaptcha;
            selectedSite.CaptchaOnRegistration = model.RequireCaptchaOnRegistration;
            selectedSite.CaptchaOnLogin = model.RequireCaptchaOnLogin;

            await siteManager.Update(selectedSite);
            
            this.AlertSuccess(string.Format(sr["Captcha Settings for {0} wwas successfully updated."],
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
            var selectedSite = await siteManager.GetSiteForEdit(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Social Login Settings"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["Social Login Settings"];
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
            model.OidConnectDisplayName = selectedSite.OidConnectDisplayName;
            model.OidConnectAppId = selectedSite.OidConnectAppId;
            model.OidConnectAppSecret = selectedSite.OidConnectAppSecret;
            model.OidConnectAuthority = selectedSite.OidConnectAuthority;

            return View(model);

        }

        // Post: /SiteAdmin/SocialLogins
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> SocialLogins(SocialLoginSettingsViewModel model)
        {
            var selectedSite = await siteManager.GetSiteForEdit(model.SiteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id == siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = sr["Social Login Settings"];
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Social Login Settings"], selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger(sr["oops something went wrong."], true);
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger(sr["oops something went wrong, site was not found."], true);
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
            selectedSite.OidConnectDisplayName = model.OidConnectDisplayName;
            selectedSite.OidConnectAppId = model.OidConnectAppId;
            selectedSite.OidConnectAppSecret = model.OidConnectAppSecret;
            selectedSite.OidConnectAuthority = model.OidConnectAuthority;

            await siteManager.Update(selectedSite);
            //TODO: need to wrap ICache into something more abstract and/or move it into sitemanager
            // also need to clear using the folder name if it isn't root site or hostname if using tenants per host
           // cache.Remove("root");
            
            this.AlertSuccess(string.Format(sr["Social Login Settings for {0} was successfully updated."],
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
            var selectedSite = await siteManager.GetSiteForEdit(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Login Page Content"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["Login Page Content"];
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
            var selectedSite = await siteManager.GetSiteForEdit(model.SiteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id == siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = sr["Login Page Content"];
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Login Page Content"], selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger(sr["oops something went wrong."], true);
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger(sr["oops something went wrong, site was not found."], true);
                return RedirectToAction("Index");
            }
            
            selectedSite.LoginInfoTop = model.LoginInfoTop;
            selectedSite.LoginInfoBottom = model.LoginInfoBottom;
            
            await siteManager.Update(selectedSite);
            
            this.AlertSuccess(string.Format(sr["Login Page Info for {0} was successfully updated."],
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
            var selectedSite = await siteManager.GetSiteForEdit(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Registration Page Content"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["Registration Page Content"];
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
            var selectedSite = await siteManager.GetSiteForEdit(model.SiteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id == siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = sr["Registration Page Content"];
            }
            else if (siteManager.CurrentSite.IsServerAdminSite)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Registration Page Content"], selectedSite.SiteName);
            }

            if (selectedSite == null)
            {
                this.AlertDanger(sr["oops something went wrong."], true);
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SiteId == Guid.Empty)
            {
                this.AlertDanger(sr["oops something went wrong, site was not found."], true);
                return RedirectToAction("Index");
            }

            if(selectedSite.RegistrationAgreement != model.RegistrationAgreement)
            {
                if(model.RequireUsersToAcceptChangedAgreement)
                {
                    // changing the terms date will force non admin users to re-accept the terms
                    selectedSite.TermsUpdatedUtc = DateTime.UtcNow;
                }
                
            }
            
            selectedSite.RegistrationPreamble = model.RegistrationPreamble;
            selectedSite.RegistrationAgreement = model.RegistrationAgreement;
            
            await siteManager.Update(selectedSite);
            
            this.AlertSuccess(string.Format(sr["Registration Page Content for {0} was successfully updated."],
                        selectedSite.SiteName), true);
            
            if ((siteManager.CurrentSite.IsServerAdminSite)
                && (siteManager.CurrentSite.Id != selectedSite.Id)
                )
            {
                return RedirectToAction("RegisterPageInfo", new { siteId = model.SiteId });
            }

            return RedirectToAction("RegisterPageInfo");
        }


        // probably should hide delete by config by default?

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ServerAdminPolicy")]
        public async Task<ActionResult> SiteDelete(Guid siteId, int returnPageNumber = 1)
        {
            var selectedSite = await siteManager.Fetch(siteId);

            if (selectedSite != null)
            {

                if (selectedSite.IsServerAdminSite)
                {
                    this.AlertWarning(string.Format(
                            sr["The site {0} was not deleted because it is a server admin site."],
                            selectedSite.SiteName)
                            , true);

                    return RedirectToAction("SiteList", new { pageNumber = returnPageNumber });
                }

                await siteManager.Delete(selectedSite);

                this.AlertWarning(string.Format(
                            sr["The site {0} was successfully deleted."],
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
            var selectedSite = await siteManager.GetSiteForEdit(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.InvariantCulture,
                sr["Domain/Host Name Mappings for {0}"],
                selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["Domain/Host Name Mappings"];
            }

            var model = new SiteHostMappingsViewModel();
            model.SiteId = selectedSite.Id;
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
                        sr["failed to add the requested host name mapping becuase it is already mapped to another site."],
                        true);
                    }
                }
                else
                {
                    await siteManager.AddHost(selectedSite.Id, hostName);
                    
                    this.AlertSuccess(string.Format(sr["Host/domain mapping for {0} was successfully created."],
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
                        await siteManager.Update(selectedSite);
                    }

                    await siteManager.DeleteHost(host.SiteId, host.Id);
                    
                    this.AlertSuccess(string.Format(sr["Host/domain mapping for {0} was successfully removed."],
                                selectedSite.SiteName), true);
                }
            }

            return RedirectToAction("SiteHostMappings", new { siteId = siteId, slp = slp });
        }
  
    }
}
