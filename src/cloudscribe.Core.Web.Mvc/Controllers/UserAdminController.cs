// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-08
// Last Modified:			2017-07-13
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Components.Messaging;
using cloudscribe.Core.Web.ExtensionPoints;
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
using System.Security.Claims;
using System.Threading.Tasks;


namespace cloudscribe.Core.Web.Controllers.Mvc
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
            ITimeZoneHelper timeZoneHelper,
            IHandleCustomUserInfoAdmin customUserEdit
            )
        {
           
            UserManager = userManager;
            this.siteManager = siteManager;
            this.emailSender = emailSender;
            uiOptions = uiOptionsAccessor.Value;
            sr = localizer;
            this.timeZoneIdResolver = timeZoneIdResolver;
            tzHelper = timeZoneHelper;
            this.customUserInfo = customUserEdit;
        }

        private SiteManager siteManager;
        public SiteUserManager<SiteUser> UserManager { get; private set; }
        private ISiteMessageEmailSender emailSender;
        private UIOptions uiOptions;
        private IStringLocalizer sr; // string resources
        private ITimeZoneIdResolver timeZoneIdResolver;
        private ITimeZoneHelper tzHelper;
        private IHandleCustomUserInfoAdmin customUserInfo;

        [HttpGet]
        public async Task<IActionResult> Index(
            Guid? siteId,
            string query = "",
            int sortMode = 0,  //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First
            int pageNumber = 1,
            int pageSize = -1
            )
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - User Management"], selectedSite.SiteName);
            }
            else
            {
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
            model.SortMode = sortMode;
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
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - User Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["User Management"];
            }
            
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
            model.SortMode = sortMode;
            model.ActionName = "Search";
            model.TimeZoneId = await timeZoneIdResolver.GetUserTimeZoneId();

            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> IpSearch(
            Guid? siteId,
            string ipQuery = "")
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - User Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["User Management"];
            }
            
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
            model.ActionName = "IpSearch";
            return View("Index", model);

        }

        [HttpGet]
        public async Task<IActionResult> LockedUsers(
            Guid? siteId,
            int pageNumber = 1,
            int pageSize = -1)
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Locked Out User Accounts"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["Locked Out User Accounts"];
            }
            
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
            model.ActionName = "LockedUsers";
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> UnApprovedUsers(
            Guid? siteId,
            int pageNumber = 1,
            int pageSize = -1)
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - User Accounts Pending Approval"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["User Accounts Pending Approval"];
            }

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
            model.ActionName = "UnApprovedUsers";
            return View(model);
        }



        [HttpGet]
        public async Task<ActionResult> NewUser(
            Guid? siteId)
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - New User"], selectedSite.SiteName);
            }
            else
            {
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
            var selectedSite = await siteManager.GetSiteForDataOperations(model.SiteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - New User"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["New User"];
            }

            if (ModelState.IsValid)
            { 
                    var user = new SiteUser()
                    {
                        SiteId = selectedSite.Id,
                        UserName = model.Username,
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
            if(userId == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            ViewData["ReturnUrl"] = Request.Path + Request.QueryString;
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Manage User"], selectedSite.SiteName);
            }
            else
            {
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
                model.Username = user.UserName;
                model.DisplayName = user.DisplayName;
                model.AccountApproved = user.AccountApproved;
                model.Comment = user.Comment;
                model.EmailConfirmed = user.EmailConfirmed;
                model.IsLockedOut = user.IsLockedOut;
                model.LastLoginDate = user.LastLoginUtc;
                model.TimeZoneId = user.TimeZoneId;
                model.WebSiteUrl = user.WebSiteUrl;

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

                model.UserClaims = await UserManager.GetClaimsAsync((SiteUser)user);


                var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext);
                currentCrumbAdjuster.KeyToAdjust = "UserEdit";
                currentCrumbAdjuster.AdjustedText = user.DisplayName;
                currentCrumbAdjuster.ViewFilterName = NamedNavigationFilters.Breadcrumbs; // this is default but showing here for readers of code 
                currentCrumbAdjuster.AddToContext();
                
            }

            var viewName = await customUserInfo.GetUserEditViewName(UserManager.Site, HttpContext);
            await customUserInfo.HandleUserEditGet(
                UserManager.Site,
                model,
                HttpContext,
                ViewData);

            return View(viewName, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit(EditUserViewModel model)
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(model.SiteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Manage User"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["Manage User"];
            }
            
            bool isValid = ModelState.IsValid && (model.UserId != Guid.Empty);
            bool customDataIsValid = await customUserInfo.HandleUserEditValidation(
                UserManager.Site,
                model,
                HttpContext,
                ViewData,
                ModelState);
            var viewName = await customUserInfo.GetUserEditViewName(UserManager.Site, HttpContext);

            if (!isValid || !customDataIsValid)
            {
                return View(viewName, model);
            }

            //editing an existing user
            var user = await UserManager.Fetch(selectedSite.Id, model.UserId);
            if (user != null)
            {
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.Username;
                user.DisplayName = model.DisplayName;
                //user.AccountApproved = model.AccountApproved;
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
                    user.DateOfBirth = null;
                }
                user.WebSiteUrl = model.WebSiteUrl;

                await customUserInfo.HandleUserEditPostSuccess(
                        UserManager.Site,
                        user,
                        model,
                        HttpContext
                        );

                await UserManager.UpdateAsync((SiteUser)user);

                

                this.AlertSuccess(string.Format(sr["user account for {0} was successfully updated."],
                        user.DisplayName), true);
                        
                    
            }
            else
            {
                //to do log it?
            }


            return RedirectToAction("Index", "UserAdmin", new { siteId = selectedSite.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> ApproveUserAccount(
            Guid siteId, 
            Guid userId, 
            bool sendApprovalEmail,
            string returnUrl = null)
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            if (selectedSite != null)
            {
                var user = await UserManager.Fetch(selectedSite.Id, userId);
                if(user != null)
                {
                    user.AccountApproved = true;
                    await UserManager.UpdateAsync((SiteUser)user);

                    this.AlertSuccess(string.Format(sr["user account for {0} was successfully approved."],
                            user.DisplayName), true);
                    
                    if(sendApprovalEmail)
                    {
                        var loginUrl = Url.Action("Login", "Account",
                            null,
                            protocol: HttpContext.Request.Scheme);

                        emailSender.SendAccountApprovalNotificationAsync(
                        selectedSite,
                        user.Email,
                        sr["Account Approved"],
                        loginUrl).Forget();
                    }
                }   
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "UserAdmin", new { siteId = selectedSite.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult> UserDelete(Guid siteId, Guid userId, int returnPageNumber = 1)
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            if (selectedSite != null)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClaim(
            //Guid siteId,
            Guid  userId, 
            string claimType, 
            string claimValue)
        {
            var selectedSite = siteManager.CurrentSite;
            var user = await UserManager.Fetch(selectedSite.Id, userId);

            if(user != null)
            {
                var claim = new Claim(claimType, claimValue);
                var result = await UserManager.AddClaimAsync((SiteUser)user, claim);
                if(result.Succeeded)
                {
                    this.AlertSuccess(string.Format(sr["The claim {0} was successfully added."],
                             claimType), true);
                }
            }
            
            return RedirectToAction("UserEdit", "UserAdmin", new { siteId = selectedSite.Id, userId = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveClaim(
            //Guid siteId,
            Guid userId,
            string claimType,
            string claimValue)
        {
            var selectedSite = siteManager.CurrentSite;
            var user = await UserManager.Fetch(selectedSite.Id, userId);

            if (user != null)
            {
                var claim = new Claim(claimType, claimValue);
                var result = await UserManager.RemoveClaimAsync((SiteUser)user, claim);
                if (result.Succeeded)
                {
                    this.AlertSuccess(string.Format(sr["The claim {0} was successfully removed."],
                             claimType), true);
                }
            }

            return RedirectToAction("UserEdit", "UserAdmin", new { siteId = selectedSite.Id, userId = userId });
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
