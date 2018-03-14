// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-08
// Last Modified:			2018-03-14
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Components.Messaging;
using cloudscribe.Core.Web.ExtensionPoints;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.UserAdmin;
using cloudscribe.Pagination.Models;
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
            _userManager = userManager;
            _siteManager = siteManager;
            _emailSender = emailSender;
            _uiOptions = uiOptionsAccessor.Value;
            _sr = localizer;
            _timeZoneIdResolver = timeZoneIdResolver;
            _tzHelper = timeZoneHelper;
            _customUserInfo = customUserEdit;
        }

        private SiteManager _siteManager;
        private SiteUserManager<SiteUser> _userManager;
        private ISiteMessageEmailSender _emailSender;
        private UIOptions _uiOptions;
        private IStringLocalizer _sr; // string resources
        private ITimeZoneIdResolver _timeZoneIdResolver;
        private ITimeZoneHelper _tzHelper;
        private IHandleCustomUserInfoAdmin _customUserInfo;

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public async Task<IActionResult> Index(
            Guid? siteId,
            string query = "",
            int sortMode = 1,  //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First
            int pageNumber = 1,
            int pageSize = -1
            )
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - User Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["User Management"];
            }
            
            var itemsPerPage = _uiOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            
            var siteMembers = await _userManager.GetPage(
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
                TimeZoneId = await _timeZoneIdResolver.GetUserTimeZoneId()
            };

            return View(model);

        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public async Task<IActionResult> Search(
            Guid? siteId,
            string query = "",
            int sortMode = 2,
            int pageNumber = 1,
            int pageSize = -1,
            bool ajaxGrid = false
            )
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - User Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["User Management"];
            }
            
            var itemsPerPage = _uiOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            if(query == null) { query = string.Empty; }
            
            var siteMembers = await _userManager.GetUserAdminSearchPage(
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
                TimeZoneId = await _timeZoneIdResolver.GetUserTimeZoneId()
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
        public async Task<IActionResult> SearchModal(
            Guid? siteId,
            string query = "",
            int sortMode = 2,
            int pageNumber = 1,
            int pageSize = -1,
            bool ajaxGrid = false
            )
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);
            
            var itemsPerPage = _uiOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            if (query == null) { query = string.Empty; }

            var siteMembers = await _userManager.GetUserAdminSearchPage(
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
                TimeZoneId = await _timeZoneIdResolver.GetUserTimeZoneId()
            };


            if (ajaxGrid)
            {
                return PartialView("UserModalGridPartial", model);
            }
            
            return PartialView("UserLookupModal", model);
            
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public async Task<IActionResult> IpSearch(
            Guid? siteId,
            string ipQuery = "")
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - User Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["User Management"];
            }
            
            var siteMembers = await _userManager.GetByIPAddress(
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
                TimeZoneId = await _timeZoneIdResolver.GetUserTimeZoneId(),
                ActionName = "IpSearch"
            };
            return View("Index", model);

        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public async Task<IActionResult> LockedUsers(
            Guid? siteId,
            int pageNumber = 1,
            int pageSize = -1)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - Locked Out User Accounts"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["Locked Out User Accounts"];
            }
            
            var itemsPerPage = _uiOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            
            var siteMembers = await _userManager.GetPageLockedUsers(
                selectedSite.Id,
                pageNumber,
                itemsPerPage);

            var model = new UserListViewModel
            {
                SiteId = selectedSite.Id,
                UserList = siteMembers,
                ShowAlphaPager = false,
                TimeZoneId = await _timeZoneIdResolver.GetUserTimeZoneId(),
                ActionName = "LockedUsers"
            };
            return View(model);

        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public async Task<IActionResult> UnApprovedUsers(
            Guid? siteId,
            int pageNumber = 1,
            int pageSize = -1)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - User Accounts Pending Approval"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["User Accounts Pending Approval"];
            }

            var itemsPerPage = _uiOptions.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            
            var siteMembers = await _userManager.GetNotApprovedUsers(
                selectedSite.Id,
                pageNumber,
                itemsPerPage);

            var model = new UserListViewModel
            {
                SiteId = selectedSite.Id,
                UserList = siteMembers,
                ShowAlphaPager = false,
                TimeZoneId = await _timeZoneIdResolver.GetUserTimeZoneId(),
                ActionName = "UnApprovedUsers"
            };
            return View(model);
        }


        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public async Task<ActionResult> NewUser(
            Guid? siteId)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - New User"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["New User"];
            }

            var model = new NewUserViewModel
            {
                SiteId = selectedSite.Id
            };

            var viewName = await _customUserInfo.GetNewUserViewName(_userManager.Site, HttpContext);
            await _customUserInfo.HandleNewUserGet(
                _userManager.Site,
                model,
                HttpContext,
                ViewData);

            return View(viewName, model);
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewUser(NewUserViewModel model)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(model.SiteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - New User"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["New User"];
            }

            bool isValid = ModelState.IsValid;

            bool userNameAvailable = await _userManager.LoginIsAvailable(model.UserId, model.Username);
            if (!userNameAvailable)
            {
                ModelState.AddModelError("usernameerror", _sr["Username is already in use"]);
                isValid = false;
            }

            bool customDataIsValid = await _customUserInfo.HandleNewUserValidation(
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

                await _customUserInfo.ProcessUserBeforeCreate(user, HttpContext);

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _customUserInfo.HandleNewUserPostSuccess(
                        selectedSite,
                        user,
                        model,
                        HttpContext);

                    if (model.MustChangePwd)
                    {
                        user.MustChangePwd = true;
                        await _userManager.UpdateAsync(user);
                    }

                    this.AlertSuccess(string.Format(_sr["user account for {0} was successfully created."],
                        user.DisplayName), true);

                    return RedirectToAction("Index", "UserAdmin", new { siteId = selectedSite.Id });
                }
                AddErrors(result);
            }

            var viewName = await _customUserInfo.GetNewUserViewName(_userManager.Site, HttpContext);

            // If we got this far, something failed, redisplay form
            return View(viewName, model);
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public async Task<ActionResult> UserActivity(
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

            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            var user = await _userManager.Fetch(selectedSite.Id, userId);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - User Activity - {1}"], selectedSite.SiteName, user.Email);
            }
            else
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["User Activity - {0}"], user.Email);
            }

            var model = new UserActivityViewModel
            {
                SiteId = selectedSite.Id,
                UserId = user.Id,
                CreatedUtc = user.CreatedUtc,
                DisplayName = user.DisplayName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastLoginUtc = user.LastLoginUtc,
                LastName = user.LastName,
                LastPassswordChangenUtc = user.LastPasswordChangeUtc,
                TimeZoneId = await _timeZoneIdResolver.GetUserTimeZoneId(),
                Locations = await _userManager.GetUserLocations(
                selectedSite.Id,
                userId,
                pageNumber,
                pageSize
                ),

                TwoFactor = user.TwoFactorEnabled,
                Logins = await _userManager.GetLoginsAsync(user as SiteUser)
            };
            if (!string.IsNullOrWhiteSpace(user.TimeZoneId))
            {
                model.UserTimeZone = user.TimeZoneId;
            }
            else
            {
                model.UserTimeZone = _userManager.Site.TimeZoneId;
            }


            var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
            {
                KeyToAdjust = "UserActivity",
                AdjustedText = string.Format(CultureInfo.CurrentUICulture, _sr["Activity - {0}"], user.Email),
                ViewFilterName = NamedNavigationFilters.Breadcrumbs // this is default but showing here for readers of code 
            };
            currentCrumbAdjuster.AddToContext();

            return View(model);
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
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
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - Manage User"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["Manage User"];
            }



            var model = new EditUserViewModel
            {
                SiteId = selectedSite.Id
            };

            var user = await _userManager.Fetch(selectedSite.Id, userId);
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
                    model.TimeZoneId = await _timeZoneIdResolver.GetSiteTimeZoneId();
                }
                model.AllTimeZones = _tzHelper.GetTimeZoneList().Select(x =>
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

                model.UserClaims = await _userManager.GetClaimsAsync((SiteUser)user);
                model.UserRoles = await _userManager.GetRolesAsync((SiteUser)user);


                var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
                {
                    KeyToAdjust = "UserEdit",
                    AdjustedText = user.DisplayName,
                    ViewFilterName = NamedNavigationFilters.Breadcrumbs // this is default but showing here for readers of code 
                };
                currentCrumbAdjuster.AddToContext();
                
            }

            var viewName = await _customUserInfo.GetUserEditViewName(_userManager.Site, HttpContext);
            await _customUserInfo.HandleUserEditGet(
                _userManager.Site,
                model,
                HttpContext,
                ViewData);

            return View(viewName, model);
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit(EditUserViewModel model)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(model.SiteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - Manage User"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["Manage User"];
            }

            

            bool isValid = ModelState.IsValid && (model.UserId != Guid.Empty);
            bool userNameAvailable = await _userManager.LoginIsAvailable(model.UserId, model.Username);
            if (!userNameAvailable)
            {
                ModelState.AddModelError("usernameerror", _sr["Username is already in use"]);
                isValid = false;
            }

            bool customDataIsValid = await _customUserInfo.HandleUserEditValidation(
                _userManager.Site,
                model,
                HttpContext,
                ViewData,
                ModelState);

            var viewName = await _customUserInfo.GetUserEditViewName(_userManager.Site, HttpContext);
            var user = await _userManager.Fetch(selectedSite.Id, model.UserId);

            if (!isValid || !customDataIsValid)
            {
                model.AccountApproved = user.AccountApproved;
                model.UserClaims = await _userManager.GetClaimsAsync((SiteUser)user);
                model.UserRoles = await _userManager.GetRolesAsync((SiteUser)user);
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

                await _customUserInfo.HandleUserEditPostSuccess(
                        _userManager.Site,
                        user,
                        model,
                        HttpContext
                        );

                await _userManager.UpdateAsync((SiteUser)user);

                

                this.AlertSuccess(string.Format(_sr["user account for {0} was successfully updated."],
                        user.DisplayName), true);
                        
                    
            }
            else
            {
                //to do log it?
            }


            return RedirectToAction("Index", "UserAdmin", new { siteId = selectedSite.Id });
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpGet]
        public async Task<ActionResult> ChangeUserPassword(
            Guid userId,
            Guid? siteId
            )
        {
            if(!_uiOptions.AllowAdminsToChangeUserPasswords)
            {
                return RedirectToAction("Index");
            }
            if (userId == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites users
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - Change User Password"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["Change User Password"];
            }

            var user = await _userManager.Fetch(selectedSite.Id, userId);

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
        public async Task<IActionResult> ChangeUserPassword(ChangeUserPasswordViewModel model)
        {
            if (!_uiOptions.AllowAdminsToChangeUserPasswords)
            {
                return RedirectToAction("Index");
            }

            if (model.UserId == Guid.Empty)
            {
                return RedirectToAction("Index");
            }


            var selectedSite = await _siteManager.GetSiteForDataOperations(model.SiteId);
            // only server admin site can edit other sites users
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - Change User Password"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["Change User Password"];
            }

            var user = await _userManager.Fetch(selectedSite.Id, model.UserId);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            user.MustChangePwd = model.MustChangePwd;

            var result = await _userManager.ChangeUserPassword(user as SiteUser, model.NewPassword, true);
            if (result.Succeeded)
            {
               

                this.AlertSuccess(_sr["The user password has been changed."]);
                return RedirectToAction("Index");
            }
            else
            {
                this.AlertDanger(_sr["oops something went wrong please try again"]);
            }
            AddErrors(result);

            return View(model);

        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApproveUserAccount(
            Guid siteId, 
            Guid userId, 
            bool sendApprovalEmail,
            string returnUrl = null)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            if (selectedSite != null)
            {
                var user = await _userManager.Fetch(selectedSite.Id, userId);
                if(user != null)
                {
                    user.AccountApproved = true;
                    await _userManager.UpdateAsync((SiteUser)user);

                    this.AlertSuccess(string.Format(_sr["user account for {0} was successfully approved."],
                            user.DisplayName), true);
                    
                    if(sendApprovalEmail)
                    {
                        var loginUrl = Url.Action("Login", "Account",
                            null,
                            protocol: HttpContext.Request.Scheme);

                        _emailSender.SendAccountApprovalNotificationAsync(
                        selectedSite,
                        user.Email,
                        _sr["Account Approved"],
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

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserDelete(Guid siteId, Guid userId, int returnPageNumber = 1)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            if (selectedSite != null)
            {
                var user = await _userManager.Fetch(selectedSite.Id, userId);
                var result = await _userManager.DeleteAsync((SiteUser)user);
                if (result.Succeeded)
                {
                    this.AlertSuccess(string.Format(_sr["user account for {0} was successfully deleted."],
                        user.DisplayName), true);
 
                }        
            }

            return RedirectToAction("Index", "UserAdmin", new { siteId = selectedSite.Id, pageNumber = returnPageNumber });
        }

        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClaim(
            //Guid siteId,
            Guid  userId, 
            string claimType, 
            string claimValue)
        {
            var selectedSite = _siteManager.CurrentSite;
            var user = await _userManager.Fetch(selectedSite.Id, userId);

            if(user != null && !string.IsNullOrWhiteSpace(claimType) && !string.IsNullOrWhiteSpace(claimValue))
            {
                var claim = new Claim(claimType, claimValue);
                var result = await _userManager.AddClaimAsync((SiteUser)user, claim);
                if(result.Succeeded)
                {
                    this.AlertSuccess(string.Format(_sr["The claim {0} was successfully added."],
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
            var selectedSite = _siteManager.CurrentSite;
            var user = await _userManager.Fetch(selectedSite.Id, userId);

            if (user != null)
            {
                var claim = new Claim(claimType, claimValue);
                var result = await _userManager.RemoveClaimAsync((SiteUser)user, claim);
                if (result.Succeeded)
                {
                    this.AlertSuccess(string.Format(_sr["The claim {0} was successfully removed."],
                             claimType), true);
                }
            }

            return RedirectToAction("UserEdit", "UserAdmin", new { siteId = selectedSite.Id, userId });
        }

        [Authorize(Policy = PolicyConstants.RoleAdminPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(
            //Guid siteId,
            Guid userId,
            string roleName)
        {
            
            var selectedSite = _siteManager.CurrentSite;

            var result = await _userManager.RemoveUserFromRole(selectedSite.Id, userId, roleName);
            
            if (result.Succeeded)
            {
                this.AlertSuccess(string.Format(_sr["The role {0} was successfully removed."], roleName), true);
            }
           
            return RedirectToAction("UserEdit", "UserAdmin", new { siteId = selectedSite.Id, userId });
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
