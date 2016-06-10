// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-08
// Last Modified:			2016-06-10
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Components.Messaging;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.UserAdmin;
using cloudscribe.Web.Common;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Policy = "UserManagementPolicy")]
    public class UserAdminController : Controller
    {
        public UserAdminController(
            SiteManager siteManager,
            SiteUserManager<SiteUser> userManager,
            ISiteMessageEmailSender emailSender,
            IOptions<UIOptions> uiOptionsAccessor,
            IStringLocalizer<CloudscribeCore> localizer,
            ITimeZoneIdResolver timeZoneIdResolver,
            ITimeZoneHelper timeZoneHelper
            )
        {
           
            UserManager = userManager;
            this.siteManager = siteManager;
            this.emailSender = emailSender;
            uiOptions = uiOptionsAccessor.Value;
            sr = localizer;
            this.timeZoneIdResolver = timeZoneIdResolver;
            tzHelper = timeZoneHelper;
        }

        private SiteManager siteManager;
        public SiteUserManager<SiteUser> UserManager { get; private set; }
        private ISiteMessageEmailSender emailSender;
        private UIOptions uiOptions;
        private IStringLocalizer sr; // string resources
        private ITimeZoneIdResolver timeZoneIdResolver;
        private ITimeZoneHelper tzHelper;

        [HttpGet]
        public async Task<IActionResult> Index(
            Guid? siteId,
            string query = "",
            int sortMode = 2,
            int pageNumber = 1,
            int pageSize = -1
            )
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - User Management"], selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = sr["User Management"];
            }
            
            var itemsPerPage = uiOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            
            var siteMembers = await UserManager.GetPage(
                selectedSite.Id,
                pageNumber,
                itemsPerPage,
                query,
                sortMode);

            var count = await UserManager.CountUsers(selectedSite.Id, query);

            var model = new UserListViewModel();
            model.SiteId = selectedSite.Id;
            model.UserList = siteMembers;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = count;
            model.AlphaQuery = query; //TODO: sanitize?
            model.TimeZoneId = await timeZoneIdResolver.GetUserTimeZoneId();

            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Search(
            Guid? siteId,
            string query = "",
            int sortMode = 2,
            int pageNumber = 1,
            int pageSize = -1)
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

            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - User Management"], selectedSite.SiteName);
            
            var itemsPerPage = uiOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            
            var siteMembers = await UserManager.GetUserAdminSearchPage(
                selectedSite.Id,
                pageNumber,
                itemsPerPage,
                query,
                sortMode);

            var count = await UserManager.CountUsersForAdminSearch(selectedSite.Id, query);

            var model = new UserListViewModel();
            model.SiteId = selectedSite.Id;
            model.UserList = siteMembers;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = count;
            model.SearchQuery = query; //TODO: sanitize?
            model.TimeZoneId = await timeZoneIdResolver.GetUserTimeZoneId();

            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> IpSearch(
            Guid? siteId,
            string ipQuery = "")
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

            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - User Management"], selectedSite.SiteName);
            
            var siteMembers = await UserManager.GetByIPAddress(
                selectedSite.Id,
                ipQuery);
            
            var model = new UserListViewModel();
            model.SiteId = selectedSite.Id;
            model.UserList = siteMembers;
            //model.Paging.CurrentPage = pageNumber;
            //model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = 1;
            model.IpQuery = ipQuery; //TODO: sanitize
            model.ShowAlphaPager = false;
            model.TimeZoneId = await timeZoneIdResolver.GetUserTimeZoneId();

            return View("Index", model);

        }

        [HttpGet]
        public async Task<IActionResult> LockedUsers(
            Guid? siteId,
            int pageNumber = 1,
            int pageSize = -1)
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

            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Locked Out User Accounts"], selectedSite.SiteName);

            var itemsPerPage = uiOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            
            var siteMembers = await UserManager.GetPageLockedUsers(
                selectedSite.Id,
                pageNumber,
                itemsPerPage);

            var count = await UserManager.CountLockedOutUsers(selectedSite.Id);

            var model = new UserListViewModel();
            model.SiteId = selectedSite.Id;
            model.UserList = siteMembers;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = count;
            model.ShowAlphaPager = false;
            model.TimeZoneId = await timeZoneIdResolver.GetUserTimeZoneId();

            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> UnApprovedUsers(
            Guid? siteId,
            int pageNumber = 1,
            int pageSize = -1)
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

            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - User Accounts Pending Approval"], selectedSite.SiteName);

            var itemsPerPage = uiOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            
            var siteMembers = await UserManager.GetNotApprovedUsers(
                selectedSite.Id,
                pageNumber,
                itemsPerPage);

            var count = await UserManager.CountNotApprovedUsers(selectedSite.Id);

            var model = new UserListViewModel();
            model.SiteId = selectedSite.Id;
            model.UserList = siteMembers;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = count;
            model.ShowAlphaPager = false;
            model.TimeZoneId = await timeZoneIdResolver.GetUserTimeZoneId();

            return View(model);
        }



        [HttpGet]
        public async Task<ActionResult> NewUser(
            Guid? siteId)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - New User"], selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = sr["New User"];
            }

            var model = new NewUserViewModel();
            model.SiteId = selectedSite.Id;
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewUser(NewUserViewModel model)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((model.SiteId != siteManager.CurrentSite.Id) && (model.SiteId != Guid.Empty) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(model.SiteId);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
            }

            ViewData["Title"] = sr["New User"];

            if (ModelState.IsValid)
            { 
                    var user = new SiteUser()
                    {
                        SiteId = selectedSite.Id,
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
                        this.AlertSuccess(string.Format(sr["user account for {0} was successfully created."],
                            user.DisplayName), true);

                        return RedirectToAction("Index", "UserAdmin", new { siteId = selectedSite.Id });
                    }
                    AddErrors(result);
                }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpGet]
        public async Task<ActionResult> UserEdit(
            Guid userId,
            Guid? siteId
            )
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value);
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Manage User"], selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = sr["Manage User"];
            }
            
            var model = new EditUserViewModel();
            model.SiteId = selectedSite.Id;
            
            var user = await UserManager.Fetch(selectedSite.Id, userId);
            if (user != null)
            {
                model.UserId = user.Id;
                model.Email = user.Email;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.LoginName = user.UserName;
                model.DisplayName = user.DisplayName;
                model.AccountApproved = user.AccountApproved;
                model.Comment = user.Comment;
                model.EmailConfirmed = user.EmailConfirmed;
                model.IsLockedOut = user.IsLockedOut;
                model.LastLoginDate = user.LastLoginDate;
                model.TimeZoneId = user.TimeZoneId;
                if(string.IsNullOrEmpty(model.TimeZoneId))
                {
                    model.TimeZoneId = await timeZoneIdResolver.GetSiteTimeZoneId();
                }
                model.AllTimeZones = tzHelper.GetTimeZoneList().Select(x =>
                               new SelectListItem
                               {
                                   Text = x,
                                   Value = x,
                                   Selected = model.TimeZoneId == x
                               });

                if (user.DateOfBirth > DateTime.MinValue)
                {
                    model.DateOfBirth = user.DateOfBirth;
                }
                
                var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext);
                currentCrumbAdjuster.KeyToAdjust = "UserEdit";
                currentCrumbAdjuster.AdjustedText = user.DisplayName;
                currentCrumbAdjuster.ViewFilterName = NamedNavigationFilters.Breadcrumbs; // this is default but showing here for readers of code 
                currentCrumbAdjuster.AddToContext();
                
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit(EditUserViewModel model)
        {
            ISiteSettings selectedSite;
            // only server admin site can edit other sites settings
            if ((model.SiteId != siteManager.CurrentSite.Id) && (model.SiteId != Guid.Empty) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(model.SiteId);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
            }

            ViewData["Title"] = sr["New User"];

            if (ModelState.IsValid)
            {
                if (model.UserId != Guid.Empty)
                {
                    //editing an existing user
                    var user = await UserManager.Fetch(selectedSite.Id, model.UserId);
                    if (user != null)
                    {
                        user.Email = model.Email;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.UserName = model.LoginName;
                        user.DisplayName = model.DisplayName;
                        user.AccountApproved = model.AccountApproved;
                        user.Comment = model.Comment;
                        user.EmailConfirmed = model.EmailConfirmed;
                        if((user.IsLockedOut)&&(!model.IsLockedOut))
                        {
                            // TODO: notify user
                            // think we need to change this so the admin controls whether
                            // email is sent when approving an account
                        }
                        user.IsLockedOut = model.IsLockedOut;
                        
                        user.TimeZoneId = model.TimeZoneId;

                        if (model.DateOfBirth.HasValue)
                        {
                            user.DateOfBirth = model.DateOfBirth.Value;
                        }
                        else
                        {
                            user.DateOfBirth = DateTime.MinValue;
                        }

                        await UserManager.UpdateAsync((SiteUser)user);
                        
                        this.AlertSuccess(string.Format(sr["user account for {0} was successfully updated."],
                             user.DisplayName), true);
                        
                        return RedirectToAction("Index", "UserAdmin", new { siteId = selectedSite.Id });
                    }
                }
                
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> ApproveUserAccount(
            Guid siteId, 
            Guid userId, 
            bool sendEmailNotification,
            int returnPageNumber = 1)
        {
            var selectedSite = await siteManager.Fetch(siteId);

            if (
                (selectedSite != null)
                && (selectedSite.Id == siteManager.CurrentSite.Id || siteManager.CurrentSite.IsServerAdminSite)
                )
            {
                var user = await UserManager.Fetch(selectedSite.Id, userId);
                if(user != null)
                {
                    user.AccountApproved = true;
                    await UserManager.UpdateAsync((SiteUser)user);

                    this.AlertSuccess(string.Format(sr["user account for {0} was successfully approved."],
                            user.DisplayName), true);
                    
                    if(sendEmailNotification)
                    {
                        var loginUrl = Url.Action("Login", "Account",
                            null,
                            protocol: HttpContext.Request.Scheme);

                        emailSender.SendAccountConfirmationEmailAsync(
                        selectedSite,
                        user.Email,
                        sr["Account Approved"],
                        loginUrl).Forget();
                    }
                }   
            }

            return RedirectToAction("Index", "UserAdmin", new { siteId = selectedSite.Id, pageNumber = returnPageNumber });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> UserDelete(Guid siteId, Guid userId, int returnPageNumber = 1)
        {
            var selectedSite = await siteManager.Fetch(siteId);

            if (
                (selectedSite != null)
                && (selectedSite.Id == siteManager.CurrentSite.Id || siteManager.CurrentSite.IsServerAdminSite)
                )
            {
                var user = await UserManager.Fetch(selectedSite.Id, userId);
                var result = await UserManager.DeleteAsync((SiteUser)user);
                if (result.Succeeded)
                {
                    this.AlertSuccess(string.Format(sr["user account for {0} was successfully deleted."],
                        user.DisplayName), true);
 
                }        
            }

            return RedirectToAction("Index", "UserAdmin", new { siteId = selectedSite.Id, pageNumber = returnPageNumber });
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
