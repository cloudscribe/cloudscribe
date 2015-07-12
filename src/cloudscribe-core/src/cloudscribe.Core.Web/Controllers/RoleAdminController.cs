// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-06
// Last Modified:			2015-07-04
// 

using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.RoleAdmin;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Configuration;
//using MvcSiteMapProvider;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Roles = "Admins,Role Admins")]
    public class RoleAdminController : CloudscribeBaseController
    {

        public RoleAdminController(
            ISiteResolver siteResolver,
            IUserRepository userRepository,
            IConfiguration configuration
            )
        {
            Site = siteResolver.Resolve();
            UserRepository = userRepository;
            config = configuration;
        }

        private ISiteSettings Site;
        private IUserRepository UserRepository;
        private IConfiguration config;

        [HttpGet]
        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<IActionResult> Index(string searchInput = "", int pageNumber = 1, int pageSize = -1)
        {
            ViewBag.SiteName = Site.SiteName;
            ViewBag.Title = "Role Management";
            //ViewBag.Heading = "Role Management";

            RoleListViewModel model = new RoleListViewModel();
            model.Heading = "Role Management";

            int itemsPerPage = config.DefaultPageSize_RoleList();
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            int totalItems = await UserRepository.CountOfRoles(
                Site.SiteId,
                searchInput);

            model.SiteRoles = await UserRepository.GetRolesBySite(
                Site.SiteId,
                searchInput,
                pageNumber,
                itemsPerPage);

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = totalItems;

            return View(model);


        }


        [HttpGet]
        [Authorize(Roles = "Admins,Role Admins")]
        //[MvcSiteMapNode(Title = "New Role", ParentKey = "Roles", Key = "RoleEdit")]
        public async Task<IActionResult> RoleEdit(int? roleId)
        {
            ViewBag.SiteName = Site.SiteName;
            ViewBag.Title = "New Role";

            RoleViewModel model = new RoleViewModel();
            model.Heading = "New Role";

            if (roleId.HasValue)
            {
                ISiteRole role = await UserRepository.FetchRole(roleId.Value);
                if ((role != null) && (role.SiteId == Site.SiteId || Site.IsServerAdminSite))
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
            else
            {
                model.SiteGuid = Site.SiteGuid;
                model.SiteId = Site.SiteId;
            }


            return View(model);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<IActionResult> RoleEdit(RoleViewModel model, int returnPageNumber = 1)
        {
            ViewBag.SiteName = Site.SiteName;
            ViewBag.Title = "Edit Role";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string successFormat;

            if ((model.SiteId == -1) || (model.SiteGuid == Guid.Empty))
            {
                model.SiteId = Site.SiteId;
                model.SiteGuid = Site.SiteGuid;
                successFormat = "The role <b>{0}</b> was successfully created.";
            }
            else
            {
                successFormat = "The role <b>{0}</b> was successfully updated.";
            }

            bool result = await UserRepository.SaveRole(model);

            if (result)
            {
                this.AlertSuccess(string.Format(successFormat,
                            model.DisplayName), true);
            }

            return RedirectToAction("Index", new { pageNumber = returnPageNumber });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<IActionResult> RoleDelete(int roleId, int returnPageNumber = 1)
        {
            ISiteRole role = await UserRepository.FetchRole(roleId);
            if (role != null && role.IsDeletable(config.RolesThatCannotBeDeleted()))
            {
                bool result = await UserRepository.DeleteUserRolesByRole(roleId);

                result = await UserRepository.DeleteRole(roleId);

                if (result)
                {
                    string successFormat = "The role <b>{0}</b> was successfully deleted.";

                    this.AlertWarning(string.Format(
                                successFormat,
                                role.DisplayName)
                                , true);
                }

            }


            return RedirectToAction("Index", new { pageNumber = returnPageNumber });
        }


        [HttpGet]
        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<IActionResult> RoleMembers(
            int roleId,
            string searchInput = "",
            int pageNumber = 1,
            int pageSize = -1)
        {
            ViewBag.SiteName = Site.SiteName;
            ViewBag.Title = "Role Members";

            ISiteRole role = await UserRepository.FetchRole(roleId);
            if ((role == null) || (role.SiteId != Site.SiteId && !Site.IsServerAdminSite))
            {
                return RedirectToAction("Index");
            }

            int itemsPerPage = config.DefaultPageSize_RoleMemberList();
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            RoleMemberListViewModel model = new RoleMemberListViewModel();

            model.Role = RoleViewModel.FromISiteRole(role);
            model.Heading = "Role Members";
            model.SearchQuery = searchInput;

            model.Members = await UserRepository.GetUsersInRole(
                role.SiteId,
                role.RoleId,
                searchInput,
                pageNumber,
                itemsPerPage);

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = await UserRepository.CountUsersInRole(role.SiteId, role.RoleId, searchInput);

            //var node = SiteMaps.Current.FindSiteMapNodeFromKey("RoleMembers");
            //if (node != null)
            //{
            //    node.Title = model.Role.DisplayName + " Role Members";
            //}

            return View(model);

        }

        [HttpGet]
        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<IActionResult> RoleNonMembers(
            int roleId,
            string searchInput = "",
            int pageNumber = 1,
            int pageSize = 2,
            bool ajaxGrid = false)
        {
            ViewBag.SiteName = Site.SiteName;
            ViewBag.Title = "Non Role Members";

            ISiteRole role = await UserRepository.FetchRole(roleId);
            if ((role == null) || (role.SiteId != Site.SiteId && !Site.IsServerAdminSite))
            {
                return RedirectToAction("Index");
            }

            int itemsPerPage = config.DefaultPageSize_RoleMemberList();
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            RoleMemberListViewModel model = new RoleMemberListViewModel();

            model.Role = RoleViewModel.FromISiteRole(role);
            model.Heading = "Non Role Members";
            model.SearchQuery = searchInput; // unsafe input

            model.Members = await UserRepository.GetUsersNotInRole(
                role.SiteId,
                role.RoleId,
                searchInput,
                pageNumber,
                itemsPerPage);

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = await UserRepository.CountUsersNotInRole(
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
        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<IActionResult> AddUser(int roleId, Guid roleGuid, int userId, Guid userGuid)
        {
            ISiteUser user = await UserRepository.Fetch(Site.SiteId, userId);

            if (user != null)
            {
                ISiteRole role = await UserRepository.FetchRole(roleId);
                if (role != null)
                {
                    bool result = await UserRepository.AddUserToRole(roleId, roleGuid, userId, userGuid);
                    if (result)
                    {
                        user.RolesChanged = true;
                        result = await UserRepository.Save(user);
                        if (result)
                        {
                            this.AlertSuccess(string.Format("<b>{0}</b> was successfully added to the role {1}.",
                            user.DisplayName, role.DisplayName), true);
                        }

                    }
                }

            }

            return RedirectToAction("RoleMembers", new { roleId = roleId });
        }

        [HttpPost]
        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<IActionResult> RemoveUser(int roleId, int userId)
        {

            ISiteUser user = await UserRepository.Fetch(Site.SiteId, userId);
            if (user != null)
            {
                ISiteRole role = await UserRepository.FetchRole(roleId);
                if (role != null)
                {
                    bool result = await UserRepository.RemoveUserFromRole(roleId, userId);
                    if (result)
                    {
                        user.RolesChanged = true;

                        result = await UserRepository.Save(user);

                        if (result)
                        {
                            this.AlertWarning(string.Format(
                            "<b>{0}</b> was successfully removed from the role {1}.",
                            user.DisplayName, role.DisplayName)
                            , true);
                        }

                    }

                }

            }

            return RedirectToAction("RoleMembers", new { roleId = roleId });
        }

    }
}
