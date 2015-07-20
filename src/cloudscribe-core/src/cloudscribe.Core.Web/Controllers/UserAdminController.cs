// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-08
// Last Modified:			2015-07-20
// 

using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.UserAdmin;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Configuration;
//using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Roles = "Admins")]
    public class UserAdminController : CloudscribeBaseController
    {
        public UserAdminController(
            ISiteResolver siteResolver,
            ISiteRepository siteRepository,
            SiteUserManager<SiteUser> userManager,
            IConfiguration configuration
            )
        {
            Site = siteResolver.Resolve();
            UserManager = userManager;
            siteRepo = siteRepository;
            config = configuration;
        }

        private IConfiguration config;
        private ISiteSettings Site;
        private ISiteRepository siteRepo;
        public SiteUserManager<SiteUser> UserManager { get; private set; }

        [HttpGet]
        public async Task<IActionResult> Index(
            string query = "",
            int sortMode = 2,
            int pageNumber = 1,
            int pageSize = -1,
            int siteId = -1)
        {
            ViewData["SiteName"] = Site.SiteName;
            ViewBag.Title = "User Management";
            //ViewBag.Heading = "Role Management";

            int itemsPerPage = config.DefaultPageSize_UserList();
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            if (siteId != -1)
            {
                if (!Site.IsServerAdminSite)
                {
                    siteId = Site.SiteId;
                }
            }
            else
            {
                siteId = Site.SiteId;
            }

            var siteMembers = await UserManager.GetPage(
                siteId,
                pageNumber,
                itemsPerPage,
                query,
                sortMode);

            var count = await UserManager.CountUsers(siteId, query);

            UserListViewModel model = new UserListViewModel();
            model.Heading = "User Management";
            model.UserList = siteMembers;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = count;
            model.AlphaQuery = query; //TODO: sanitize

            return View(model);


        }

        [HttpGet]
        public async Task<IActionResult> Search(
            string query = "",
            int sortMode = 2,
            int pageNumber = 1,
            int pageSize = -1,
            int siteId = -1)
        {
            ViewData["SiteName"] = Site.SiteName;
            ViewBag.Title = "User Management";
            //ViewBag.Heading = "Role Management";

            int itemsPerPage = config.DefaultPageSize_UserList();
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            if (siteId != -1)
            {
                if (!Site.IsServerAdminSite)
                {
                    siteId = Site.SiteId;
                }
            }
            else
            {
                siteId = Site.SiteId;
            }

            var siteMembers = await UserManager.GetUserAdminSearchPage(
                siteId,
                pageNumber,
                itemsPerPage,
                query,
                sortMode);

            var count = await UserManager.CountUsersForAdminSearch(siteId, query);

            UserListViewModel model = new UserListViewModel();
            model.Heading = "User Management";
            model.UserList = siteMembers;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = count;
            model.SearchQuery = query; //TODO: sanitize


            return View("Index", model);

        }

        [HttpGet]
        public async Task<IActionResult> IpSearch(string ipQuery = "", int siteId = -1)
        {
            ViewData["SiteName"] = Site.SiteName;
            ViewBag.Title = "User Management";
            //ViewBag.Heading = "Role Management";


            Guid siteGuid = Site.SiteGuid;

            if (siteId != -1)
            {
                if (Site.IsServerAdminSite)
                {
                    ISiteSettings otherSite = await siteRepo.Fetch(siteId);
                    if (otherSite != null)
                    {
                        siteGuid = otherSite.SiteGuid;
                    }
                }
            }


            List<IUserInfo> siteMembers;
            //if(searchExp.Length > 0)
            //{
            //    siteMembers = Site.UserRepository.GetUserAdminSearchPage(
            //    siteId,
            //    pageNumber,
            //    pageSize,
            //    searchExp,
            //    sortMode, out totalPages);
            //}
            //else
            //{

            //}

            siteMembers = await UserManager.GetByIPAddress(
                siteGuid,
                ipQuery);


            UserListViewModel model = new UserListViewModel();
            model.Heading = "User Management";
            model.UserList = siteMembers;
            //model.Paging.CurrentPage = pageNumber;
            //model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = 1;
            model.IpQuery = ipQuery; //TODO: sanitize
            model.ShowAlphaPager = false;

            return View("Index", model);


        }


        [HttpGet]
        //[Authorize(Roles = "Admins")]
        //[MvcSiteMapNode(Title = "New User", ParentKey = "UserAdmin", Key = "UserEdit")]
        public async Task<ActionResult> UserEdit(int? userId)
        {
            ViewData["SiteName"] = Site.SiteName;
            ViewBag.Title = "New User";

            EditUserViewModel model = new EditUserViewModel();
            model.SiteGuid = Site.SiteGuid;

            if (userId.HasValue)
            {
                ISiteUser user = await UserManager.Fetch(Site.SiteId, userId.Value);
                if (user != null)
                {
                    model.UserId = user.UserId;
                    model.Email = user.Email;
                    model.FirstName = user.FirstName;
                    model.LastName = user.LastName;
                    model.LoginName = user.UserName;
                    model.DisplayName = user.DisplayName;

                    if (user.DateOfBirth > DateTime.MinValue)
                    {
                        model.DateOfBirth = user.DateOfBirth;
                    }

                    ViewBag.Title = "Manage User";

                    //var node = SiteMaps.Current.FindSiteMapNodeFromKey("UserEdit");
                    //if (node != null)
                    //{
                    //    node.Title = "Manage User";
                    //}
                }


            }

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit(EditUserViewModel model)
        {
            ViewData["SiteName"] = Site.SiteName;
            ViewBag.Title = "New User";

            if (ModelState.IsValid)
            {
                if (model.UserId > -1)
                {
                    //editing an existing user
                    ISiteUser user = await UserManager.Fetch(Site.SiteId, model.UserId);
                    if (user != null)
                    {
                        user.Email = model.Email;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.UserName = model.LoginName;
                        user.DisplayName = model.DisplayName;
                        if (model.DateOfBirth.HasValue)
                        {
                            user.DateOfBirth = model.DateOfBirth.Value;
                        }
                        else
                        {
                            user.DateOfBirth = DateTime.MinValue;
                        }

                        bool result = await UserManager.Save(user);
                        if (result)
                        {
                            this.AlertSuccess(string.Format("user account for <b>{0}</b> was successfully updated.",
                            user.DisplayName), true);
                        }


                        return RedirectToAction("Index", "UserAdmin");
                    }
                }
                else
                {
                    var user = new SiteUser
                    {
                        SiteId = Site.SiteId,
                        SiteGuid = Site.SiteGuid,
                        UserName = model.LoginName,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DisplayName = model.DisplayName
                    };

                    if (model.DateOfBirth.HasValue)
                    {
                        user.DateOfBirth = model.DateOfBirth.Value;
                    }

                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        this.AlertSuccess(string.Format("user account for <b>{0}</b> was successfully created.",
                            user.DisplayName), true);

                        return RedirectToAction("Index", "UserAdmin");
                    }
                    AddErrors(result);
                }


            }

            // If we got this far, something failed, redisplay form
            return View(model);

        }



        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

    }
}
