// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-08
// Last Modified:			2019-05-17
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Components.Messaging;
using cloudscribe.Core.Web.ExtensionPoints;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.UserAdmin;
using cloudscribe.Pagination.Models;
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

    public class UserAdminController : Controller
    {
        public UserAdminController(
            SiteManager siteManager,
            SiteUserManager<SiteUser> userManager,
            ISiteMessageEmailSender emailSender,
            IAuthorizationService authorizationService,
            IOptions<UIOptions> uiOptionsAccessor,
            IStringLocalizer<CloudscribeCore> localizer,
            cloudscribe.DateTimeUtils.ITimeZoneIdResolver timeZoneIdResolver,
            cloudscribe.DateTimeUtils.ITimeZoneHelper timeZoneHelper,
            IHandleCustomUserInfoAdmin customUserEdit
            )
        {
            UserManager = userManager;
            SiteManager = siteManager;
            EmailSender = emailSender;
            UIOptions = uiOptionsAccessor.Value;
            StringLocalizer = localizer;
            AuthorizationService = authorizationService;
            TimeZoneIdResolver = timeZoneIdResolver;
            TimeZoneHelper = timeZoneHelper;
            CustomUserInfo = customUserEdit;
        }

        protected SiteManager SiteManager { get; private set; }
        protected SiteUserManager<SiteUser> UserManager { get; private set; }
        protected ISiteMessageEmailSender EmailSender { get; private set; }
        protected IAuthorizationService AuthorizationService { get; private set; }
        protected UIOptions UIOptions { get; private set; }
        protected IStringLocalizer StringLocalizer { get; private set; } // string resources
        protected cloudscribe.DateTimeUtils.ITimeZoneIdResolver TimeZoneIdResolver { get; private set; }
        protected cloudscribe.DateTimeUtils.ITimeZoneHelper TimeZoneHelper { get; private set; }
        protected IHandleCustomUserInfoAdmin CustomUserInfo { get; private set; }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public virtual async Task<IActionResult> Index(
            Guid? siteId,
            string query = "",
            int sortMode = 1,  //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First
            int pageNumber = 1,
            int pageSize = -1
            )
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - User Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["User Management"];
            }
            
            var itemsPerPage = UIOptions.DefaultPageSize_UserList;
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

            var model = new UserListViewModel
            {
                SiteId = selectedSite.Id,
                UserList = siteMembers,
                SortMode = sortMode,
                AlphaQuery = query,
                TimeZoneId = await TimeZoneIdResolver.GetUserTimeZoneId()
            };

            return View(model);

        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public virtual async Task<IActionResult> Search(
            Guid? siteId,
            string query = "",
            int sortMode = 2,
            int pageNumber = 1,
            int pageSize = -1,
            bool ajaxGrid = false
            )
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - User Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["User Management"];
            }
            
            var itemsPerPage = UIOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            if(query == null) { query = string.Empty; }
            
            var siteMembers = await UserManager.GetUserAdminSearchPage(
                selectedSite.Id,
                pageNumber,
                itemsPerPage,
                query,
                sortMode);

            var model = new UserListViewModel
            {
                SiteId = selectedSite.Id,
                UserList = siteMembers,
                SearchQuery = query,
                SortMode = sortMode,
                ActionName = "Search",
                TimeZoneId = await TimeZoneIdResolver.GetUserTimeZoneId()
            };

            if (Request.IsAjaxRequest())
            {
                if(ajaxGrid)
                {
                    return PartialView("UserModalGridPartial", model);
                }
                else
                {
                    return PartialView("UserLookupModal", model);
                }
            }

            return View("Index", model);
        }

        /// <summary>
        /// this method is for use by custom features that need to lookup users to get the userid email etc
        /// It is protected by UserLookupPolicy so that th epolicy could allow looking up users without
        /// providing user management permissions. ie when creating an ecommerce order or other tasks that require looking up a user
        /// to assign to a custom entity such as an order
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="query"></param>
        /// <param name="sortMode"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="ajaxGrid"></param>
        /// <returns></returns>
        [Authorize(Policy = PolicyConstants.UserLookupPolicy)]
        [HttpGet]
        public virtual async Task<IActionResult> SearchModal(
            Guid? siteId,
            string query = "",
            int sortMode = 0, //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First
            int pageNumber = 1,
            int pageSize = -1,
            bool ajaxGrid = false
            )
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId);
            
            var itemsPerPage = UIOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            if (query == null) { query = string.Empty; }

            var siteMembers = await UserManager.GetUserAdminSearchPage(
                selectedSite.Id,
                pageNumber,
                itemsPerPage,
                query,
                sortMode);

            var model = new UserListViewModel
            {
                SiteId = selectedSite.Id,
                UserList = siteMembers,
                SearchQuery = query,
                SortMode = sortMode,
                ActionName = "SearchModal",
                TimeZoneId = await TimeZoneIdResolver.GetUserTimeZoneId()
            };


            if (ajaxGrid)
            {
                return PartialView("UserModalGridPartial", model);
            }
            
            return PartialView("UserLookupModal", model);
            
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public virtual async Task<IActionResult> IpSearch(
            Guid? siteId,
            string ipQuery = "")
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - User Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["User Management"];
            }
            
            var siteMembers = await UserManager.GetByIPAddress(
                selectedSite.Id,
                ipQuery);

            // not really paged in UI but re-using the ViewModel which needs a PagedResult
            var data = new PagedResult<IUserInfo>
            {
                Data = siteMembers,
                PageNumber = 1,
                PageSize = 2000,
                TotalItems = siteMembers.Count()
            };

            var model = new UserListViewModel
            {
                SiteId = selectedSite.Id,
                UserList = data,
                IpQuery = ipQuery, //TODO: sanitize
                ShowAlphaPager = false,
                TimeZoneId = await TimeZoneIdResolver.GetUserTimeZoneId(),
                ActionName = "IpSearch"
            };
            return View("Index", model);

        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public virtual async Task<IActionResult> LockedUsers(
            Guid? siteId,
            int pageNumber = 1,
            int pageSize = -1)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - Locked Out User Accounts"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["Locked Out User Accounts"];
            }
            
            var itemsPerPage = UIOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            
            var siteMembers = await UserManager.GetPageLockedUsers(
                selectedSite.Id,
                pageNumber,
                itemsPerPage);

            var model = new UserListViewModel
            {
                SiteId = selectedSite.Id,
                UserList = siteMembers,
                ShowAlphaPager = false,
                TimeZoneId = await TimeZoneIdResolver.GetUserTimeZoneId(),
                ActionName = "LockedUsers"
            };
            return View(model);

        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public virtual async Task<IActionResult> UnApprovedUsers(
            Guid? siteId,
            int pageNumber = 1,
            int pageSize = -1)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - User Accounts Pending Approval"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["User Accounts Pending Approval"];
            }

            var itemsPerPage = UIOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            
            var siteMembers = await UserManager.GetNotApprovedUsers(
                selectedSite.Id,
                pageNumber,
                itemsPerPage);

            var model = new UserListViewModel
            {
                SiteId = selectedSite.Id,
                UserList = siteMembers,
                ShowAlphaPager = false,
                TimeZoneId = await TimeZoneIdResolver.GetUserTimeZoneId(),
                ActionName = "UnApprovedUsers"
            };
            return View(model);
        }


        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public virtual async Task<ActionResult> NewUser(
            Guid? siteId)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - New User"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["New User"];
            }

            var model = new NewUserViewModel
            {
                SiteId = selectedSite.Id
            };

            var viewName = await CustomUserInfo.GetNewUserViewName(UserManager.Site, HttpContext);
            await CustomUserInfo.HandleNewUserGet(
                UserManager.Site,
                model,
                HttpContext,
                ViewData);

            return View(viewName, model);
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> NewUser(NewUserViewModel model)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(model.SiteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - New User"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["New User"];
            }

            bool isValid = ModelState.IsValid;

            bool userNameAvailable = await UserManager.LoginIsAvailable(model.UserId, model.Username);
            if (!userNameAvailable)
            {
                ModelState.AddModelError("usernameerror", StringLocalizer["Username is already in use"]);
                isValid = false;
            }

            bool customDataIsValid = await CustomUserInfo.HandleNewUserValidation(
                selectedSite,
                model,
                HttpContext,
                ViewData,
                ModelState);


            if (isValid && customDataIsValid)
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

                await CustomUserInfo.ProcessUserBeforeCreate(user, HttpContext);

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await CustomUserInfo.HandleNewUserPostSuccess(
                        selectedSite,
                        user,
                        model,
                        HttpContext);

                    if (model.MustChangePwd)
                    {
                        user.MustChangePwd = true;
                        await UserManager.UpdateAsync(user);
                    }

                    this.AlertSuccess(string.Format(StringLocalizer["user account for {0} was successfully created."],
                        user.DisplayName), true);

                    return RedirectToAction("Index", "UserAdmin", new { siteId = selectedSite.Id });
                }
                AddErrors(result);
            }

            var viewName = await CustomUserInfo.GetNewUserViewName(UserManager.Site, HttpContext);

            // If we got this far, something failed, redisplay form
            return View(viewName, model);
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public virtual async Task<ActionResult> UserActivity(
            Guid userId,
            Guid? siteId,
            int pageNumber = 1, 
            int pageSize = 10
            )
        {
            if (userId == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId);

            var user = await UserManager.Fetch(selectedSite.Id, userId);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - User Activity - {1}"], selectedSite.SiteName, user.Email);
            }
            else
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["User Activity - {0}"], user.Email);
            }

            var model = new UserActivityViewModel
            {
                SiteId = selectedSite.Id,
                UserId = user.Id,
                CreatedUtc = user.CreatedUtc,
                DisplayName = user.DisplayName,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                FirstName = user.FirstName,
                LastLoginUtc = user.LastLoginUtc,
                LastName = user.LastName,
                LastPassswordChangenUtc = user.LastPasswordChangeUtc,
                TimeZoneId = await TimeZoneIdResolver.GetUserTimeZoneId(),
                Locations = await UserManager.GetUserLocations(
                selectedSite.Id,
                userId,
                pageNumber,
                pageSize
                ),

                TwoFactor = user.TwoFactorEnabled,
                Logins = await UserManager.GetLoginsAsync(user as SiteUser)
            };
            if (!string.IsNullOrWhiteSpace(user.TimeZoneId))
            {
                model.UserTimeZone = user.TimeZoneId;
            }
            else
            {
                model.UserTimeZone = UserManager.Site.TimeZoneId;
            }


            var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
            {
                KeyToAdjust = "UserActivity",
                AdjustedText = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["Activity - {0}"], user.Email),
                ViewFilterName = NamedNavigationFilters.Breadcrumbs // this is default but showing here for readers of code 
            };
            currentCrumbAdjuster.AddToContext();

            return View(model);
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public virtual async Task<ActionResult> UserEdit(
            Guid userId,
            Guid? siteId
            )
        {
            if(userId == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            ViewData["ReturnUrl"] = Request.Path + Request.QueryString;
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - Manage User"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["Manage User"];
            }



            var model = new EditUserViewModel
            {
                SiteId = selectedSite.Id
            };

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
                model.AvatarUrl = user.AvatarUrl;

                if(string.IsNullOrEmpty(model.TimeZoneId))
                {
                    model.TimeZoneId = await TimeZoneIdResolver.GetSiteTimeZoneId();
                }
                model.AllTimeZones = TimeZoneHelper.GetTimeZoneList().Select(x =>
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
                model.UserRoles = await UserManager.GetRolesAsync((SiteUser)user);


                var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
                {
                    KeyToAdjust = "UserEdit",
                    AdjustedText = user.DisplayName,
                    ViewFilterName = NamedNavigationFilters.Breadcrumbs // this is default but showing here for readers of code 
                };
                currentCrumbAdjuster.AddToContext();
                
            }

            var viewName = await CustomUserInfo.GetUserEditViewName(UserManager.Site, HttpContext);
            await CustomUserInfo.HandleUserEditGet(
                UserManager.Site,
                model,
                HttpContext,
                ViewData);

            return View(viewName, model);
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> UserEdit(EditUserViewModel model)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(model.SiteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - Manage User"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["Manage User"];
            }

            

            bool isValid = ModelState.IsValid && (model.UserId != Guid.Empty);
            bool userNameAvailable = await UserManager.LoginIsAvailable(model.UserId, model.Username);
            if (!userNameAvailable)
            {
                ModelState.AddModelError("usernameerror", StringLocalizer["Username is already in use"]);
                isValid = false;
            }

            bool customDataIsValid = await CustomUserInfo.HandleUserEditValidation(
                UserManager.Site,
                model,
                HttpContext,
                ViewData,
                ModelState);

            var viewName = await CustomUserInfo.GetUserEditViewName(UserManager.Site, HttpContext);
            var user = await UserManager.Fetch(selectedSite.Id, model.UserId);

            if (!isValid || !customDataIsValid)
            {
                model.AccountApproved = user.AccountApproved;
                model.UserClaims = await UserManager.GetClaimsAsync((SiteUser)user);
                model.UserRoles = await UserManager.GetRolesAsync((SiteUser)user);
                return View(viewName, model);
            }

            //editing an existing user
            
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

                await CustomUserInfo.HandleUserEditPostSuccess(
                        UserManager.Site,
                        user,
                        model,
                        HttpContext
                        );

                await UserManager.UpdateAsync((SiteUser)user);

                

                this.AlertSuccess(string.Format(StringLocalizer["user account for {0} was successfully updated."],
                        user.DisplayName), true);
                        
                    
            }
            else
            {
                //to do log it?
            }


            return RedirectToAction("UserEdit", "UserAdmin", new { siteId = selectedSite.Id, userId = user.Id });
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public virtual async Task<ActionResult> ChangeUserPassword(
            Guid userId,
            Guid? siteId
            )
        {
            if(!UIOptions.AllowAdminsToChangeUserPasswords)
            {
                return RedirectToAction("Index");
            }
            if (userId == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites users
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - Change User Password"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["Change User Password"];
            }

            var user = await UserManager.Fetch(selectedSite.Id, userId);

            if (user == null)
            {
                return RedirectToAction("Index");
            }


            var model = new ChangeUserPasswordViewModel
            {
                SiteId = selectedSite.Id,
                UserId = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email
            };

            var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
            {
                KeyToAdjust = "UserEdit",
                AdjustedText = user.DisplayName
            };
            currentCrumbAdjuster.AddToContext();

            return View(model);

        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> ChangeUserPassword(ChangeUserPasswordViewModel model)
        {
            if (!UIOptions.AllowAdminsToChangeUserPasswords)
            {
                return RedirectToAction("Index");
            }

            if (model.UserId == Guid.Empty)
            {
                return RedirectToAction("Index");
            }


            var selectedSite = await SiteManager.GetSiteForDataOperations(model.SiteId);
            // only server admin site can edit other sites users
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - Change User Password"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["Change User Password"];
            }

            var user = await UserManager.Fetch(selectedSite.Id, model.UserId);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            user.MustChangePwd = model.MustChangePwd;

            var result = await UserManager.ChangeUserPassword(user as SiteUser, model.NewPassword, true);
            if (result.Succeeded)
            {
               

                this.AlertSuccess(StringLocalizer["The user password has been changed."], true);
                return RedirectToAction("Index");
            }
            else
            {
                var message = result.Errors.FirstOrDefault().Description;
                if(!string.IsNullOrWhiteSpace(message))
                {
                    this.AlertDanger(StringLocalizer[message], true);
                }
                else
                {
                    this.AlertDanger(StringLocalizer["oops something went wrong please try again"], true);
                }

                return RedirectToAction("ChangeUserPassword", new { userId = model.UserId });
                
            }
            //AddErrors(result);

            //return View(model);

        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ApproveUserAccount(
            Guid siteId, 
            Guid userId, 
            bool sendApprovalEmail,
            string returnUrl = null)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId);

            if (selectedSite != null)
            {
                var user = await UserManager.Fetch(selectedSite.Id, userId);
                if(user != null)
                {
                    user.AccountApproved = true;
                    await UserManager.UpdateAsync((SiteUser)user);

                    this.AlertSuccess(string.Format(StringLocalizer["user account for {0} was successfully approved."],
                            user.DisplayName), true);
                    
                    if(sendApprovalEmail)
                    {
                        var loginUrl = Url.Action("Login", "Account",
                            null,
                            protocol: HttpContext.Request.Scheme);

                        await EmailSender.SendAccountApprovalNotificationAsync(
                            selectedSite,
                            user.Email,
                            StringLocalizer["Account Approved"],
                            loginUrl);
                    }
                }   
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "UserAdmin", new { siteId = selectedSite.Id });
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> UserDelete(Guid siteId, Guid userId, int returnPageNumber = 1)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId);

            if (selectedSite != null)
            {
                var user = await UserManager.Fetch(selectedSite.Id, userId);
                var result = await UserManager.DeleteAsync((SiteUser)user);
                if (result.Succeeded)
                {
                    this.AlertSuccess(string.Format(StringLocalizer["user account for {0} was successfully deleted."],
                        user.DisplayName), true);
 
                }        
            }

            return RedirectToAction("Index", "UserAdmin", new { siteId = selectedSite.Id, pageNumber = returnPageNumber });
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> AddClaim(
            //Guid siteId,
            Guid  userId, 
            string claimType, 
            string claimValue)
        {
            var selectedSite = SiteManager.CurrentSite;
            var user = await UserManager.Fetch(selectedSite.Id, userId);

            if(user != null && !string.IsNullOrWhiteSpace(claimType) && !string.IsNullOrWhiteSpace(claimValue))
            {
                var claim = new Claim(claimType, claimValue);
                var result = await UserManager.AddClaimAsync((SiteUser)user, claim);
                if(result.Succeeded)
                {
                    this.AlertSuccess(string.Format(StringLocalizer["The claim {0} was successfully added."],
                             claimType), true);
                }
            }
            
            return RedirectToAction("UserEdit", "UserAdmin", new { siteId = selectedSite.Id, userId });
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveClaim(
            //Guid siteId,
            Guid userId,
            string claimType,
            string claimValue)
        {
            var selectedSite = SiteManager.CurrentSite;
            var user = await UserManager.Fetch(selectedSite.Id, userId);

            if (user != null)
            {
                var claim = new Claim(claimType, claimValue);
                var result = await UserManager.RemoveClaimAsync((SiteUser)user, claim);
                if (result.Succeeded)
                {
                    this.AlertSuccess(string.Format(StringLocalizer["The claim {0} was successfully removed."],
                             claimType), true);
                }
            }

            return RedirectToAction("UserEdit", "UserAdmin", new { siteId = selectedSite.Id, userId });
        }

        [Authorize(Policy = PolicyConstants.RoleAdminPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> RemoveRole(
            //Guid siteId,
            Guid userId,
            string roleName)
        {
            
            var selectedSite = SiteManager.CurrentSite;

            var canRemove = true;
            if (roleName == "Administrators")
            {
                var adminAuthResult = await AuthorizationService.AuthorizeAsync(User, "AdminPolicy");
                canRemove = adminAuthResult.Succeeded;
            }

            if(canRemove)
            {
                var result = await UserManager.RemoveUserFromRole(selectedSite.Id, userId, roleName);

                if (result.Succeeded)
                {
                    this.AlertSuccess(string.Format(StringLocalizer["The role {0} was successfully removed."], roleName), true);
                }
            }
            else
            {
                this.AlertDanger(StringLocalizer["Sorry, but only other Administrators can remove users from the Administrators role."], true);
            }

           
           
            return RedirectToAction("UserEdit", "UserAdmin", new { siteId = selectedSite.Id, userId });
        }


        protected virtual void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
