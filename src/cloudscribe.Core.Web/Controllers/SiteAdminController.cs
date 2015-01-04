// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2015-01-04
// 

using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Helpers;
using cloudscribe.Core.Web.ViewModels.SiteSettings;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcSiteMapProvider;

namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Roles="Admins,Content Administrators")]
    public class SiteAdminController : CloudscribeBaseController
    {
        private IGeoRepository geoRepo;

        public SiteAdminController(IGeoRepository geoRepository)
        {
            geoRepo = geoRepository;
        }

        // GET: /SiteAdmin
        public async Task<ActionResult> Index(SiteAdminMessageId? message)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Site Administration";
            ViewBag.Heading = "Site Administration";
            return View();

            
        }

        [Authorize(Roles = "ServerAdmins")]
        public async Task<ActionResult> SiteList(int pageNumber = 1, int pageSize = -1)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Site List";
            ViewBag.Heading = "Site List";

            int itemsPerPage = AppSettings.DefaultPageSize_SiteList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            int totalPages = 0;
            int filteredSiteId = -1; //nothing filtered
            List<ISiteInfo> sites = Site.SiteRepository.GetPageOtherSites(
                filteredSiteId,
                pageNumber,
                itemsPerPage,
                out totalPages);

            SiteListViewModel model = new SiteListViewModel();
            model.Heading = "Site List";
            model.Sites = sites;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalPages = totalPages;

            return View(model);


        }

        // GET: /SiteAdmin/SiteInfo
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> SiteInfo(int? siteId, SiteAdminMessageId? message)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Site Basic Settings";

            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if((siteId.HasValue)&&(Site.SiteSettings.IsServerAdminSite))
            {
                selectedSite = Site.SiteRepository.Fetch(siteId.Value);
            }
            else
            {
                selectedSite = Site.SiteSettings;
            }

            SiteBasicSettingsViewModel model = new SiteBasicSettingsViewModel();
            model.AllTimeZones = DateTimeHelper.GetTimeZoneList();

            model.SiteId = selectedSite.SiteId;
            model.SiteGuid = selectedSite.SiteGuid;
            model.SiteName = selectedSite.SiteName;
            model.Slogan = selectedSite.Slogan;
            model.TimeZoneId = selectedSite.TimeZoneId;
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
            model.SiteFolderName = selectedSite.SiteFolderName;

            model.AvailableCountries.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var countries = geoRepo.GetAllCountries();
            Guid selectedCountryGuid = Guid.Empty;
            foreach (var country in countries)
            {
                if(country.ISOCode2 == model.CompanyCountry)
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
                var states = geoRepo.GetGeoZonesByCountry(selectedCountryGuid);
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

        


        // Post: /SiteAdmin/SiteInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> SiteInfo(SiteBasicSettingsViewModel model)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //model.SiteId = Site.SiteSettings.SiteId;
            //model.SiteGuid = Site.SiteSettings.SiteGuid;
            ISiteSettings selectedSite = null;
            if(model.SiteGuid == Site.SiteSettings.SiteGuid)
            {
                selectedSite = Site.SiteSettings;
            }
            else if(Site.SiteSettings.IsServerAdminSite)
            {
                selectedSite = Site.SiteRepository.Fetch(model.SiteGuid);
            }

            if(selectedSite == null)
            {
                this.AlertDanger("oops something went wrong.", true);

                return RedirectToAction("Index");
            }

            selectedSite.SiteName = model.SiteName;
            selectedSite.Slogan = model.Slogan;
            selectedSite.TimeZoneId = model.TimeZoneId;
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
            selectedSite.SiteFolderName = model.SiteFolderName;

            Site.SiteRepository.Save(selectedSite);

            //SiteAdminMessageId? message = SiteAdminMessageId.UpdateSettingsSuccess;

            this.AlertSuccess(string.Format("Basic site settings for <b>{0}</b> were successfully updated.",
                            selectedSite.SiteName), true);

            if((Site.SiteSettings.IsServerAdminSite)
                &&(Site.SiteSettings.SiteGuid != selectedSite.SiteGuid))
            {
                // just edited from site list so redirect there
                return RedirectToAction("SiteList");
            }

            return RedirectToAction("Index");

        }

        // GET: /SiteAdmin/NewSite
        [Authorize(Roles = "ServerAdmins")]
        public async Task<ActionResult> NewSite(SiteAdminMessageId? message)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Create New Site";

            SiteBasicSettingsViewModel model = new SiteBasicSettingsViewModel();
            model.SiteId = -1;
            model.SiteGuid = Guid.Empty;
           // model.SiteName = Site.SiteSettings.SiteName;
            //model.Slogan = Site.SiteSettings.Slogan;
            model.TimeZoneId = Site.SiteSettings.TimeZoneId;
            model.AllTimeZones = DateTimeHelper.GetTimeZoneList();
            //model.CompanyName = Site.SiteSettings.CompanyName;
            //model.CompanyStreetAddress = Site.SiteSettings.CompanyStreetAddress;
            //model.CompanyStreetAddress2 = Site.SiteSettings.CompanyStreetAddress2;
            //model.CompanyLocality = Site.SiteSettings.CompanyLocality;
            //model.CompanyRegion = Site.SiteSettings.CompanyRegion;
            //model.CompanyPostalCode = Site.SiteSettings.CompanyPostalCode;
            //model.CompanyCountry = Site.SiteSettings.CompanyCountry;
            //model.CompanyPhone = Site.SiteSettings.CompanyPhone;
            //model.CompanyFax = Site.SiteSettings.CompanyFax;
            //model.CompanyPublicEmail = Site.SiteSettings.CompanyPublicEmail;
            //.SiteFolderName = Site.SiteSettings.SiteFolderName;

            model.AvailableCountries.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var countries = geoRepo.GetAllCountries();
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
                var states = geoRepo.GetGeoZonesByCountry(selectedCountryGuid);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> NewSite(SiteBasicSettingsViewModel model)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //model.SiteId = Site.SiteSettings.SiteId;
            //model.SiteGuid = Site.SiteSettings.SiteGuid;
            SiteSettings newSite = new SiteSettings();


            newSite.SiteName = model.SiteName;
            newSite.Slogan = model.Slogan;
            newSite.TimeZoneId = model.TimeZoneId;
            newSite.CompanyName = model.CompanyName;
            newSite.CompanyStreetAddress = model.CompanyStreetAddress;
            newSite.CompanyStreetAddress2 = model.CompanyStreetAddress2;
            newSite.CompanyLocality = model.CompanyLocality;
            newSite.CompanyRegion = model.CompanyRegion;
            newSite.CompanyPostalCode = model.CompanyPostalCode;
            newSite.CompanyCountry = model.CompanyCountry;
            newSite.CompanyPhone = model.CompanyPhone;
            newSite.CompanyFax = model.CompanyFax;
            newSite.CompanyPublicEmail = model.CompanyPublicEmail;
            newSite.SiteFolderName = model.SiteFolderName;

            //Site.SiteRepository.Save(newSite);
            NewSiteHelper.CreateNewSite(Site.SiteRepository, newSite);
            NewSiteHelper.CreateRequiredRolesAndAdminUser(newSite, Site.SiteRepository, Site.UserRepository);

            if(AppSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            {
                SiteFolder folder = new SiteFolder();
                folder.FolderName = newSite.SiteFolderName;
                folder.SiteGuid = newSite.SiteGuid;
                Site.SiteRepository.Save(folder);
            }

            this.AlertSuccess(string.Format("Basic site settings for <b>{0}</b> were successfully created.",
                            newSite.SiteName), true);

            //SiteAdminMessageId? message = SiteAdminMessageId.CreateSiteSuccess;

            return RedirectToAction("Index");

        }

        //public async Task<ActionResult> SiteList(SiteAdminMessageId? message)
        //{

        //}

        //public async Task<ActionResult> Roles(SiteAdminMessageId? message)
        //{

        //}

        //[Authorize(Roles = "Admins,Role Admins")]
        //public async Task<ActionResult> Roles()
        //{
        //    ViewBag.SiteName = Site.SiteSettings.SiteName;
        //    ViewBag.Title = "Role Management";
        //    //ViewBag.Heading = "Role Management";

        //    RoleListViewModel model = new RoleListViewModel();
        //    model.Heading = "Role Management";
        //    model.SiteRoles = Site.UserRepository.GetRolesBySite(Site.SiteSettings.SiteId);

        //    return View(model);


        //}

        //// GET: /SiteAdmin/SiteInfo
        //[Authorize(Roles = "Admins,Role Admins")]
        //[MvcSiteMapNode(Title = "Edit Role", ParentKey = "Roles", Key = "RoleEdit")] 
        //public async Task<ActionResult> RoleEdit(int? roleId)
        //{
        //    ViewBag.SiteName = Site.SiteSettings.SiteName;
        //    ViewBag.Title = "Role Edit";

        //    //Site.UserRepository.FetchRole()

        //    RoleViewModel model = new RoleViewModel();

        //    if(roleId.HasValue)
        //    {
        //        ISiteRole role = Site.UserRepository.FetchRole(roleId.Value);
        //        if((role != null) &&(role.SiteId == Site.SiteSettings.SiteId || Site.SiteSettings.IsServerAdminSite))
        //        {
        //            model = RoleViewModel.FromISiteRole(role);
        //        }
        //    }
        //    else
        //    {
        //        model.SiteGuid = Site.SiteSettings.SiteGuid;
        //        model.SiteId = Site.SiteSettings.SiteId;
        //    }

        //    model.Heading = "Role Edit";

        //    return View(model);


        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admins,Role Admins")]
        //public async Task<ActionResult> RoleEdit(RoleViewModel model, int returnPageNumber = 1)
        //{
        //    ViewBag.SiteName = Site.SiteSettings.SiteName;
        //    ViewBag.Title = "Edit Role";

        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    if((model.SiteId == -1)||(model.SiteGuid == Guid.Empty))
        //    {
        //        model.SiteId = Site.SiteSettings.SiteId;
        //        model.SiteGuid = Site.SiteSettings.SiteGuid;
        //    }

        //    Site.UserRepository.SaveRole(model);

        //    return RedirectToAction("Roles", new { pageNumber = returnPageNumber });

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admins,Role Admins")]
        //public async Task<ActionResult> RoleDelete(int roleId, int returnPageNumber =1)
        //{
        //    ISiteRole role = Site.UserRepository.FetchRole(roleId);
        //    if (role != null && role.IsDeletable(AppSettings.RolesThatCannotBeDeleted))
        //    {
        //        Site.UserRepository.DeleteRole(roleId);
        //    }

            
        //    return RedirectToAction("Roles", new { pageNumber = returnPageNumber });
        //}



        public enum SiteAdminMessageId
        {
            CreateSiteSuccess,
            UpdateSettingsSuccess,
            ClosedSiteSuccess,
            Error
        }
    }
}
