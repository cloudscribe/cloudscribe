// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-06
// Last Modified:			2016-05-19
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.ViewModels.RoleAdmin;
using cloudscribe.Web.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Policy = "RoleAdminPolicy")]
    public class RoleAdminController : Controller
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
            Guid? siteId,
            string searchInput = "", 
            int pageNumber = 1, 
            int pageSize = -1)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Role Management", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewBag.Title = "Role Management";
            }

            
            //ViewBag.Heading = "Role Management";

            RoleListViewModel model = new RoleListViewModel();
            model.SiteId = selectedSite.Id;
           // model.Heading = "Role Management";

            int itemsPerPage = uiOptions.DefaultPageSize_RoleList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            int totalItems = await RoleManager.CountOfRoles(
                selectedSite.Id,
                searchInput);

            model.SiteRoles = await RoleManager.GetRolesBySite(
                selectedSite.Id,
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
            Guid? siteId,
            Guid? roleId)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - New Role", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewBag.Title = "New Role";
            }
            
            var model = new RoleViewModel();
            model.SiteId = selectedSite.Id;
            model.Heading = "New Role";

            if (roleId.HasValue)
            {
                var role = await RoleManager.FindByIdAsync(roleId.Value.ToString());
                if ((role != null) && (role.SiteId == selectedSite.Id))
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
            if ((model.SiteId != Guid.Empty) && (model.SiteId != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(model.SiteId);
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
            var role = SiteRole.FromISiteRole(model);
            if (model.Id == Guid.Empty)
            {
                role.SiteId = selectedSite.Id;
                successFormat = "The role <b>{0}</b> was successfully created.";
                await RoleManager.CreateAsync(role);
            }
            else
            {
                successFormat = "The role <b>{0}</b> was successfully updated.";
                await RoleManager.UpdateAsync(role);
            }
            
            this.AlertSuccess(string.Format(successFormat,
                        role.RoleName), true);
            

            return RedirectToAction("Index", new { siteId = selectedSite.Id, pageNumber = returnPageNumber });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RoleAdminPolicy")]
        public async Task<IActionResult> RoleDelete(
            Guid? siteId,
            Guid roleId, 
            int returnPageNumber = 1)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                
            }

            var role = await RoleManager.FindByIdAsync(roleId.ToString());
            if (role != null && role.SiteId == selectedSite.Id && role.IsDeletable(setupOptions.RolesThatCannotBeDeleted))
            {
                await RoleManager.DeleteUserRolesByRole(roleId);
                await RoleManager.DeleteRole(roleId);
                
                string successFormat = "The role <b>{0}</b> was successfully deleted.";

                this.AlertWarning(string.Format(
                            successFormat,
                            role.RoleName)
                            , true);
                
            }

            return RedirectToAction("Index", new { siteId = selectedSite.Id, pageNumber = returnPageNumber });
        }


        [HttpGet]
        [Authorize(Policy = "RoleAdminPolicy")]
        public async Task<IActionResult> RoleMembers(
            Guid? siteId,
            Guid roleId,
            string searchInput = "",
            int pageNumber = 1,
            int pageSize = -1)
        {

            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Role Members", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewBag.Title = "Role Members";
            }
            
            var role = await RoleManager.FindByIdAsync(roleId.ToString());
            if ((role == null) || (role.SiteId != selectedSite.Id))
            {
                return RedirectToAction("Index");
            }

            int itemsPerPage = uiOptions.DefaultPageSize_RoleMemberList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            RoleMemberListViewModel model = new RoleMemberListViewModel();
            
            model.SiteId = selectedSite.Id;
            model.UseEmailForLogin = selectedSite.UseEmailForLogin;
      

            model.Role = RoleViewModel.FromISiteRole(role);
            if(selectedSite.Id != siteManager.CurrentSite.Id)
            {
                model.Heading1 = string.Format(CultureInfo.CurrentUICulture, "{0} - {1}", selectedSite.SiteName, role.RoleName);
            }
            else
            {
                model.Heading1 = role.RoleName;
            }
            
            model.Heading2 = "Role Members";
            model.SearchQuery = searchInput;

            model.Members = await RoleManager.GetUsersInRole(
                role.SiteId,
                role.Id,
                searchInput,
                pageNumber,
                itemsPerPage);

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = await RoleManager.CountUsersInRole(role.SiteId, role.Id, searchInput);

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
            Guid? siteId,
            Guid roleId,
            string searchInput = "",
            int pageNumber = 1,
            int pageSize = 0,
            bool ajaxGrid = false)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, "{0} - Non Role Members", selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewBag.Title = "Non Role Members";
            }
            
            var role = await RoleManager.FindByIdAsync(roleId.ToString());
            if ((role == null) || (role.SiteId != selectedSite.Id))
            {
                return RedirectToAction("Index");
            }

            int itemsPerPage = uiOptions.DefaultPageSize_RoleMemberList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = new RoleMemberListViewModel();

            model.Role = RoleViewModel.FromISiteRole(role);
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                model.Heading1 = string.Format(CultureInfo.CurrentUICulture, "{0} - {1}", selectedSite.SiteName, role.RoleName);
            }
            else
            {
                model.Heading1 = role.RoleName;
            }

            model.Heading2 = "Non Role Members";
            model.SearchQuery = searchInput; // unsafe input

            model.Members = await RoleManager.GetUsersNotInRole(
                role.SiteId,
                role.Id,
                searchInput,
                pageNumber,
                itemsPerPage);

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = await RoleManager.CountUsersNotInRole(
                role.SiteId,
                role.Id,
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
            Guid? siteId, 
            Guid roleId, 
            Guid userId)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                
            }

            var user = await UserManager.Fetch(selectedSite.Id, userId);

            if (user != null)
            {
                var role = await RoleManager.FindByIdAsync(roleId.ToString());
                if ((role != null) && (role.SiteId == selectedSite.Id))
                {
                    await RoleManager.AddUserToRole(user, role);
                    
                    this.AlertSuccess(string.Format("<b>{0}</b> was successfully added to the role {1}.",
                    user.DisplayName, role.RoleName), true);
                       
                }

            }

            return RedirectToAction("RoleMembers", new { siteId = selectedSite.Id, roleId = roleId });
        }

        [HttpPost]
        [Authorize(Policy = "RoleAdminPolicy")]
        public async Task<IActionResult> RemoveUser(
            Guid? siteId,
            Guid roleId, 
            Guid userId)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);

            }
            else
            {
                selectedSite = siteManager.CurrentSite;

            }

            var user = await UserManager.Fetch(selectedSite.Id, userId);
            if (user != null)
            {
                var role = await RoleManager.FindByIdAsync(roleId.ToString());
                if ((role != null) && (role.SiteId == selectedSite.Id))
                {
                    await RoleManager.RemoveUserFromRole(user, role);
                    this.AlertWarning(string.Format(
                        "<b>{0}</b> was successfully removed from the role {1}.",
                        user.DisplayName, role.RoleName)
                        , true);
                       
                }

            }

            return RedirectToAction("RoleMembers", new { siteId = selectedSite.Id, roleId = roleId });
        }

    }
}
