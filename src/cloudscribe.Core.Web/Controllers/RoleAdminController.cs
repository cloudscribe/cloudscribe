// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-06
// Last Modified:			2016-02-12
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.ViewModels.RoleAdmin;
using cloudscribe.Web.Common.Extensions;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Policy = "RoleAdminPolicy")]
    public class RoleAdminController : CloudscribeBaseController
    {

        public RoleAdminController(
            SiteManager siteManager,
            SiteUserManager<SiteUser> userManager,
            SiteRoleManager<SiteRole> roleManager,
            IOptions<UIOptions> uiOptionsAccessor,
            IOptions<SiteConfigOptions> setupOptionsAccessor
            )
        {
            UserManager = userManager;
            RoleManager = roleManager;
            this.siteManager = siteManager;
            uiOptions = uiOptionsAccessor.Value;
            setupOptions = setupOptionsAccessor.Value;
        }

        private SiteManager siteManager;
        private UIOptions uiOptions;
        private SiteConfigOptions setupOptions;
        public SiteUserManager<SiteUser> UserManager { get; private set; }
        public SiteRoleManager<SiteRole> RoleManager { get; private set; }

        [HttpGet]
        [Authorize(Policy = "RoleAdminPolicy")]
        public async Task<IActionResult> Index(
            Guid? siteGuid,
            string searchInput = "", 
            int pageNumber = 1, 
            int pageSize = -1)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteGuid.HasValue) && (siteGuid.Value != Guid.Empty) && (siteGuid.Value != siteManager.CurrentSite.SiteGuid) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteGuid.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Role Management", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewBag.Title = "Role Management";
            }

            
            //ViewBag.Heading = "Role Management";

            RoleListViewModel model = new RoleListViewModel();
            model.SiteGuid = selectedSite.SiteGuid;
           // model.Heading = "Role Management";

            int itemsPerPage = uiOptions.DefaultPageSize_RoleList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            int totalItems = await RoleManager.CountOfRoles(
                selectedSite.SiteId,
                searchInput);

            model.SiteRoles = await RoleManager.GetRolesBySite(
                selectedSite.SiteId,
                searchInput,
                pageNumber,
                itemsPerPage);

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = totalItems;

            return View(model);


        }


        [HttpGet]
        [Authorize(Policy = "RoleAdminPolicy")]
        //[MvcSiteMapNode(Title = "New Role", ParentKey = "Roles", Key = "RoleEdit")]
        public async Task<IActionResult> RoleEdit(
            Guid? siteGuid,
            int? roleId)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteGuid.HasValue) && (siteGuid.Value != Guid.Empty) && (siteGuid.Value != siteManager.CurrentSite.SiteGuid) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteGuid.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - New Role", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewBag.Title = "New Role";
            }

           

            RoleViewModel model = new RoleViewModel();
            model.SiteGuid = selectedSite.SiteGuid;
            model.SiteId = selectedSite.SiteId;
            model.Heading = "New Role";

            if (roleId.HasValue)
            {
                ISiteRole role = await RoleManager.FetchRole(roleId.Value);
                if ((role != null) && (role.SiteId == selectedSite.SiteId))
                {
                    model = RoleViewModel.FromISiteRole(role);

                    ViewBag.Title = "Edit Role";
                    model.Heading = "Edit Role";

                    // below we are just manipiulating the bread crumbs
                    //var node = SiteMaps.Current.FindSiteMapNodeFromKey("RoleEdit");
                    //var node = SiteMaps.Current.FindSiteMapNodeFromKey("RoleEdit");
                    //if (node != null)
                    //{
                    //    node.Title = "Edit Role";
                    //}
                }
            }
            


            return View(model);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RoleAdminPolicy")]
        public async Task<IActionResult> RoleEdit(RoleViewModel model, int returnPageNumber = 1)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((model.SiteGuid != Guid.Empty) && (model.SiteGuid != siteManager.CurrentSite.SiteGuid) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(model.SiteGuid);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Edit Role", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewBag.Title = "Edit Role";
            }

            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string successFormat;

            if ((model.SiteId == -1) || (model.SiteGuid == Guid.Empty))
            {
                model.SiteId = selectedSite.SiteId;
                model.SiteGuid = selectedSite.SiteGuid;
                successFormat = "The role <b>{0}</b> was successfully created.";
            }
            else
            {
                successFormat = "The role <b>{0}</b> was successfully updated.";
            }

            bool result = await RoleManager.SaveRole(model);

            if (result)
            {
                this.AlertSuccess(string.Format(successFormat,
                            model.DisplayName), true);
            }

            return RedirectToAction("Index", new { siteGuid = selectedSite.SiteGuid, pageNumber = returnPageNumber });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RoleAdminPolicy")]
        public async Task<IActionResult> RoleDelete(
            Guid? siteGuid,
            int roleId, 
            int returnPageNumber = 1)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteGuid.HasValue) && (siteGuid.Value != Guid.Empty) && (siteGuid.Value != siteManager.CurrentSite.SiteGuid) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteGuid.Value);
                
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                
            }

            ISiteRole role = await RoleManager.FetchRole(roleId);
            if (role != null && role.SiteGuid == selectedSite.SiteGuid && role.IsDeletable(setupOptions.RolesThatCannotBeDeleted))
            {
                bool result = await RoleManager.DeleteUserRolesByRole(roleId);

                result = await RoleManager.DeleteRole(roleId);

                if (result)
                {
                    string successFormat = "The role <b>{0}</b> was successfully deleted.";

                    this.AlertWarning(string.Format(
                                successFormat,
                                role.DisplayName)
                                , true);
                }

            }


            return RedirectToAction("Index", new { siteGuid = selectedSite.SiteGuid, pageNumber = returnPageNumber });
        }


        [HttpGet]
        [Authorize(Policy = "RoleAdminPolicy")]
        public async Task<IActionResult> RoleMembers(
            Guid? siteGuid,
            int roleId,
            string searchInput = "",
            int pageNumber = 1,
            int pageSize = -1)
        {

            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteGuid.HasValue) && (siteGuid.Value != Guid.Empty) && (siteGuid.Value != siteManager.CurrentSite.SiteGuid) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteGuid.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Role Members", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewBag.Title = "Role Members";
            }

            

            ISiteRole role = await RoleManager.FetchRole(roleId);
            if ((role == null) || (role.SiteId != selectedSite.SiteId))
            {
                return RedirectToAction("Index");
            }

            int itemsPerPage = uiOptions.DefaultPageSize_RoleMemberList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            RoleMemberListViewModel model = new RoleMemberListViewModel();
            
            model.SiteGuid = selectedSite.SiteGuid;
            model.UseEmailForLogin = selectedSite.UseEmailForLogin;
      

            model.Role = RoleViewModel.FromISiteRole(role);
            if(selectedSite.SiteGuid != siteManager.CurrentSite.SiteGuid)
            {
                model.Heading1 = string.Format(CultureInfo.CurrentUICulture, "{0} - {1}", selectedSite.SiteName, role.DisplayName);
            }
            else
            {
                model.Heading1 = role.DisplayName;
            }
            
            model.Heading2 = "Role Members";
            model.SearchQuery = searchInput;

            model.Members = await RoleManager.GetUsersInRole(
                role.SiteId,
                role.RoleId,
                searchInput,
                pageNumber,
                itemsPerPage);

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = await RoleManager.CountUsersInRole(role.SiteId, role.RoleId, searchInput);

            //var node = SiteMaps.Current.FindSiteMapNodeFromKey("RoleMembers");
            //if (node != null)
            //{
            //    node.Title = model.Role.DisplayName + " Role Members";
            //}

            return View(model);

        }

        [HttpGet]
        [Authorize(Policy = "RoleAdminPolicy")]
        public async Task<IActionResult> RoleNonMembers(
            Guid? siteGuid,
            int roleId,
            string searchInput = "",
            int pageNumber = 1,
            int pageSize = 0,
            bool ajaxGrid = false)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteGuid.HasValue) && (siteGuid.Value != Guid.Empty) && (siteGuid.Value != siteManager.CurrentSite.SiteGuid) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteGuid.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Non Role Members", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewBag.Title = "Non Role Members";
            }

            

            ISiteRole role = await RoleManager.FetchRole(roleId);
            if ((role == null) || (role.SiteId != selectedSite.SiteId))
            {
                return RedirectToAction("Index");
            }

            int itemsPerPage = uiOptions.DefaultPageSize_RoleMemberList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            RoleMemberListViewModel model = new RoleMemberListViewModel();

            model.Role = RoleViewModel.FromISiteRole(role);
            if (selectedSite.SiteGuid != siteManager.CurrentSite.SiteGuid)
            {
                model.Heading1 = string.Format(CultureInfo.CurrentUICulture, "{0} - {1}", selectedSite.SiteName, role.DisplayName);
            }
            else
            {
                model.Heading1 = role.DisplayName;
            }

            model.Heading2 = "Non Role Members";
            model.SearchQuery = searchInput; // unsafe input

            model.Members = await RoleManager.GetUsersNotInRole(
                role.SiteId,
                role.RoleId,
                searchInput,
                pageNumber,
                itemsPerPage);

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = await RoleManager.CountUsersNotInRole(
                role.SiteId,
                role.RoleId,
                searchInput);
            
            if (ajaxGrid)
            {
                return PartialView("NonMembersGridPartial", model);
            }

            return PartialView("NonMembersPartial", model);

        }

        [HttpPost]
        [Authorize(Policy = "RoleAdminPolicy")]
        public async Task<IActionResult> AddUser(
            Guid? siteGuid,
            int roleId, 
            Guid roleGuid, 
            int userId, 
            Guid userGuid)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteGuid.HasValue) && (siteGuid.Value != Guid.Empty) && (siteGuid.Value != siteManager.CurrentSite.SiteGuid) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteGuid.Value);
                
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                
            }

            ISiteUser user = await UserManager.Fetch(selectedSite.SiteId, userId);

            if (user != null)
            {
                ISiteRole role = await RoleManager.FetchRole(roleId);
                if ((role != null) && (role.SiteId == selectedSite.SiteId))
                {
                    bool result = await RoleManager.AddUserToRole(user, role);
                    if (result)
                    {
                        
                        this.AlertSuccess(string.Format("<b>{0}</b> was successfully added to the role {1}.",
                        user.DisplayName, role.DisplayName), true);
                        
                    }
                }

            }

            return RedirectToAction("RoleMembers", new { siteGuid = selectedSite.SiteGuid, roleId = roleId });
        }

        [HttpPost]
        [Authorize(Policy = "RoleAdminPolicy")]
        public async Task<IActionResult> RemoveUser(
            Guid? siteGuid,
            int roleId, 
            int userId)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteGuid.HasValue) && (siteGuid.Value != Guid.Empty) && (siteGuid.Value != siteManager.CurrentSite.SiteGuid) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteGuid.Value);

            }
            else
            {
                selectedSite = siteManager.CurrentSite;

            }

            ISiteUser user = await UserManager.Fetch(selectedSite.SiteId, userId);
            if (user != null)
            {
                ISiteRole role = await RoleManager.FetchRole(roleId);
                if ((role != null) && (role.SiteId == selectedSite.SiteId))
                {
                    bool result = await RoleManager.RemoveUserFromRole(user, role);
                    if (result)
                    {
                        this.AlertWarning(string.Format(
                            "<b>{0}</b> was successfully removed from the role {1}.",
                            user.DisplayName, role.DisplayName)
                            , true);
                       
                    }

                }

            }

            return RedirectToAction("RoleMembers", new { siteGuid = selectedSite.SiteGuid, roleId = roleId });
        }

    }
}
