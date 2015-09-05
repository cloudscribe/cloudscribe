// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-08
// Last Modified:			2015-08-05
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.UserAdmin;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Roles = "Admins")]
    public class UserAdminController : CloudscribeBaseController
    {
        public UserAdminController(
            SiteManager siteManager,
            SiteUserManager<SiteUser> userManager,
            IOptions<UIOptions> uiOptionsAccessor
            )
        {
           
            UserManager = userManager;
            this.siteManager = siteManager;
            uiOptions = uiOptionsAccessor.Options;

        }

        private SiteManager siteManager;
        public SiteUserManager<SiteUser> UserManager { get; private set; }
        private UIOptions uiOptions;

        [HttpGet]
        public async Task<IActionResult> Index(
            string query = "",
            int sortMode = 2,
            int pageNumber = 1,
            int pageSize = -1,
            int siteId = -1)
        {
            ViewData["Title"] = "User Management";
            //ViewBag.Heading = "Role Management";

            int itemsPerPage = uiOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            if (siteId != -1)
            {
                if (!siteManager.CurrentSite.IsServerAdminSite)
                {
                    siteId = siteManager.CurrentSite.SiteId;
                }
            }
            else
            {
                siteId = siteManager.CurrentSite.SiteId;
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
            ViewData["Title"] = "User Management";
            //ViewBag.Heading = "Role Management";

            int itemsPerPage = uiOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            if (siteId != -1)
            {
                if (!siteManager.CurrentSite.IsServerAdminSite)
                {
                    siteId = siteManager.CurrentSite.SiteId;
                }
            }
            else
            {
                siteId = siteManager.CurrentSite.SiteId;
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
            ViewData["Title"] = "User Management";
            //ViewBag.Heading = "Role Management";


            Guid siteGuid = siteManager.CurrentSite.SiteGuid;

            if (siteId != -1)
            {
                if (siteManager.CurrentSite.IsServerAdminSite)
                {
                    ISiteSettings otherSite = await siteManager.Fetch(siteId);
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
            ViewData["Title"] = "New User";

            EditUserViewModel model = new EditUserViewModel();
            model.SiteGuid = siteManager.CurrentSite.SiteGuid;

            if (userId.HasValue)
            {
                ISiteUser user = await UserManager.Fetch(siteManager.CurrentSite.SiteId, userId.Value);
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
            ViewData["Title"] = "New User";

            if (ModelState.IsValid)
            {
                if (model.UserId > -1)
                {
                    //editing an existing user
                    ISiteUser user = await UserManager.Fetch(siteManager.CurrentSite.SiteId, model.UserId);
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
                        SiteId = siteManager.CurrentSite.SiteId,
                        SiteGuid = siteManager.CurrentSite.SiteGuid,
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
