// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-06
// Last Modified:			2019-05-17
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.ViewModels.RoleAdmin;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers.Mvc
{
    
    public class RoleAdminController : Controller
    {
        public RoleAdminController(
            SiteManager siteManager,
            SiteUserManager<SiteUser> userManager,
            SiteRoleManager<SiteRole> roleManager,
            IAuthorizationService authorizationService,
            IStringLocalizer<CloudscribeCore> localizer,
            IOptions<UIOptions> uiOptionsAccessor,
            IOptions<SiteConfigOptions> setupOptionsAccessor,
            IEnumerable<IGuardNeededRoles> roleGuards
            )
        {
            UserManager = userManager;
            RoleManager = roleManager;
            SiteManager = siteManager;
            UIOptions = uiOptionsAccessor.Value;
            SetupOptions = setupOptionsAccessor.Value;
            AuthorizationService = authorizationService;
            StringLocalizer = localizer;
            RoleGuards = roleGuards;
        }

        protected SiteManager SiteManager { get; private set; }
        protected UIOptions UIOptions { get; private set; }
        protected IAuthorizationService AuthorizationService { get; private set; }
        protected IStringLocalizer StringLocalizer { get; private set; }
        protected SiteConfigOptions SetupOptions { get; private set; }
        protected SiteUserManager<SiteUser> UserManager { get; private set; }
        protected SiteRoleManager<SiteRole> RoleManager { get; private set; }
        protected IEnumerable<IGuardNeededRoles> RoleGuards { get; private set; }


        [HttpGet]
        [Authorize(Policy = PolicyConstants.RoleAdminPolicy)]
        public virtual async Task<IActionResult> Index(
            Guid? siteId,
            string searchInput = "", 
            int pageNumber = 1, 
            int pageSize = -1)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - Role Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["Role Management"];
            }

            var model = new RoleListViewModel
            {
                SiteId = selectedSite.Id
            };

            int itemsPerPage = UIOptions.DefaultPageSize_RoleList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            
            model.SiteRoles = await RoleManager.GetRolesBySite(
                selectedSite.Id,
                searchInput,
                pageNumber,
                itemsPerPage);
            
            return View(model);

        }


        //http://stackoverflow.com/questions/3575690/how-to-populate-a-series-of-checkboxes-from-a-hidden-csv-field-using-jquery
        //http://stackoverflow.com/questions/4276329/how-to-re-populate-hidden-csv-field-on-form-submit-using-jquery
        //https://blog4rami.wordpress.com/2014/01/09/preserving-check-box-states-pagedlist-mvc3/

        [HttpGet]
        [Authorize(Policy = PolicyConstants.RoleLookupPolicy)]
        public virtual async Task<IActionResult> Modal(
            Guid? siteId,
            string searchInput = "",
            bool ajaxGrid = false,
            int pageNumber = 1,
            int pageSize = -1)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId, true);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            { 
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - Role Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["Role Management"];
            }

            var model = new RoleListViewModel
            {
                SiteId = selectedSite.Id
            };

            int itemsPerPage = UIOptions.DefaultPageSize_RoleList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            if (searchInput == null) searchInput = string.Empty;
            
            model.SiteRoles = await RoleManager.GetRolesBySite(
                selectedSite.Id,
                searchInput,
                pageNumber,
                itemsPerPage);
            
            if(ajaxGrid)
            {
                return PartialView("ModalListPartial", model);
            }
            return PartialView(model);
        }



        [HttpGet]
        [Authorize]
        public virtual async Task<IActionResult> ModalAddUserToRoles(
           Guid siteId,
           Guid userId, 
           string searchInput = "",
           bool ajaxGrid = false,
           int pageNumber = 1,
           int pageSize = -1)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId, true);
            
            var model = new AddUserToRoleListViewModel
            {
                SiteId = selectedSite.Id,
                UserId = userId 
            };

            int itemsPerPage = UIOptions.DefaultPageSize_RoleList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            if (searchInput == null) searchInput = string.Empty;

            model.SiteRoles = await RoleManager.GetRolesBySite(
                selectedSite.Id,
                searchInput,
                pageNumber,
                itemsPerPage);

            var user = await UserManager.Fetch(selectedSite.Id, userId);
            if (user != null)
            {
                model.UserRoles = await UserManager.GetRolesAsync((SiteUser)user);
            }

            if (ajaxGrid)
            {
                return PartialView("ModalAddUserToRolesPartial", model);
            }
            return PartialView("ModalAddUserToRoles", model);
        }


        [HttpPost]
        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [Authorize(Policy = PolicyConstants.RoleLookupPolicy)]
        public virtual async Task<IActionResult> AddUserToRoles(AddUserToRoleListViewModel model)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(model.SiteId, true);
            var user         = await UserManager.Fetch(selectedSite.Id, model.UserId);

            if (user != null && model.SelectedCheckboxesCSV != null)
            {
                // somewhat clumsy workaround for persisting roles checkboxes across paging
                model.SelectedRoles = model.SelectedCheckboxesCSV.Split(',').Where(x=>x.Length > 0).ToList();

                foreach(var selectedRole in model.SelectedRoles)
                {
                    var role = await RoleManager.FindByIdAsync(selectedRole);

                    if(role != null) {

                        var canAdd = true;
                        
                        if (role.NormalizedRoleName == "ADMINISTRATORS")
                        {
                            var adminAuthResult = await AuthorizationService.AuthorizeAsync(User, "AdminPolicy");
                            canAdd = adminAuthResult.Succeeded;
                        }

                        if (canAdd)
                        {
                            if ((role != null) && (role.SiteId == selectedSite.Id))
                            {
                                await RoleManager.AddUserToRole(user, role);

                                this.AlertSuccess(string.Format(StringLocalizer["{0} was successfully added to the role {1}."],
                                    user.DisplayName,
                                    role.RoleName), true);
                            }
                        }
                        else
                        {
                            this.AlertDanger(StringLocalizer["Sorry, but only other Administrators can add users to the Administrators role."], true);
                        }
                    }
                }
            }

            return RedirectToAction("UserEdit", "Useradmin", new { userId = model.UserId, siteId = model.SiteId });
        }


        [HttpGet]
        [Authorize(Policy = PolicyConstants.RoleAdminPolicy)]
        public virtual async Task<IActionResult> RoleEdit(
            Guid? siteId,
            Guid? roleId)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId, true);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - New Role"], selectedSite.SiteName);  
            }
            else
            {
                ViewData["Title"] = StringLocalizer["New Role"];
            }

            var model = new RoleViewModel
            {
                SiteId = selectedSite.Id
            };

            if (roleId.HasValue)
            {
                var role = await RoleManager.FindByIdAsync(roleId.Value.ToString());
                if ((role != null) && (role.SiteId == selectedSite.Id))
                {

                    var rejectReasons = new List<string>();
                    foreach(var guard in RoleGuards)
                    {
                        var rejectReason = await guard.GetEditRejectReason(selectedSite.Id, role.RoleName);
                        if(!string.IsNullOrWhiteSpace(rejectReason))
                        {
                            rejectReasons.Add(rejectReason);
                        }
                    }
                    if(rejectReasons.Count > 0)
                    {
                        var alertMessage = string.Join("<br />", rejectReasons.ToArray());

                        this.AlertDanger(alertMessage, true);

                        return RedirectToAction("Index", new { siteId = selectedSite.Id });
                    }


                    model = RoleViewModel.FromISiteRole(role);
                    ViewData["Title"] = StringLocalizer["Edit Role"];
                    var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
                    {
                        KeyToAdjust = "RoleEdit",
                        AdjustedText = StringLocalizer["Edit Role"],
                        ViewFilterName = NamedNavigationFilters.Breadcrumbs // this is default but showing here for readers of code 
                    };
                    currentCrumbAdjuster.AddToContext();
                }
            }
            
            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.RoleAdminPolicy)]
        public virtual async Task<IActionResult> RoleEdit(RoleViewModel model, int returnPageNumber = 1)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(model.SiteId, true);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - Edit Role"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["Edit Role"];
            }
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string successFormat;
            var role = SiteRole.FromISiteRole(model);

            if (model.Id == Guid.Empty)
            {
                var exists = await RoleManager.RoleExistsAsync(model.RoleName);

                if(exists)
                {
                    ModelState.AddModelError("roleerror", StringLocalizer["The role name is already in use."]);
                    return View(model);
                }

                role.SiteId = selectedSite.Id;
                successFormat = StringLocalizer["The role <b>{0}</b> was successfully created."];
                await RoleManager.CreateAsync(role);
            }
            else
            {
                var existingRole = await RoleManager.FindByIdAsync(model.Id.ToString());
                if(existingRole != null && !role.RoleName.Equals(existingRole.RoleName))
                {
                    // hooks available here to respond to a role whose name has changed
                    await RoleManager.UpdateRole(role, existingRole.RoleName);
                }
                else
                {
                    await RoleManager.UpdateAsync(role);
                }

                successFormat = StringLocalizer["The role <b>{0}</b> was successfully updated."];
            }

            this.AlertSuccess(string.Format(successFormat, role.RoleName), true);
            
            return RedirectToAction("Index", new { siteId = selectedSite.Id, pageNumber = returnPageNumber });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.RoleAdminPolicy)]
        public virtual async Task<IActionResult> RoleDelete(
            Guid? siteId,
            Guid roleId, 
            int returnPageNumber = 1)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId, true);
            
            var role = await RoleManager.FindByIdAsync(roleId.ToString());
            if (role != null && role.SiteId == selectedSite.Id && role.IsDeletable(SetupOptions.RolesThatCannotBeDeleted))
            {
                var rejectReasons = new List<string>();
                foreach (var guard in RoleGuards)
                {
                    var rejectReason = await guard.GetEditRejectReason(selectedSite.Id, role.RoleName);
                    if (!string.IsNullOrWhiteSpace(rejectReason))
                    {
                        rejectReasons.Add(rejectReason);
                    }
                }
                if (rejectReasons.Count > 0)
                {
                    var alertMessage = string.Join("<br />", rejectReasons.ToArray());

                    this.AlertDanger(alertMessage, true);

                    return RedirectToAction("Index", new { siteId = selectedSite.Id });
                }


                await RoleManager.DeleteUserRolesByRole(roleId);
                await RoleManager.DeleteRole(roleId);
                
                string successFormat = StringLocalizer["The role <b>{0}</b> was successfully deleted."];

                this.AlertWarning(string.Format(successFormat,role.RoleName), true);
                
            }

            return RedirectToAction("Index", new { siteId = selectedSite.Id, pageNumber = returnPageNumber });
        }


        [HttpGet]
        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [Authorize(Policy = PolicyConstants.RoleLookupPolicy)]
        public virtual async Task<IActionResult> RoleMembers(
            Guid? siteId,
            Guid roleId,
            string searchInput = "",
            int pageNumber = 1,
            int pageSize = -1)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId, true);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - Role Members"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["Role Members"];
            }
            
            var role = await RoleManager.FindByIdAsync(roleId.ToString());
            if ((role == null) || (role.SiteId != selectedSite.Id ))
            {
                return RedirectToAction("Index");
            }

            int itemsPerPage = UIOptions.DefaultPageSize_RoleMemberList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            RoleMemberListViewModel model = new RoleMemberListViewModel
            {
                SiteId = selectedSite.Id,
                //UseEmailForLogin = selectedSite.UseEmailForLogin,
                Role = RoleViewModel.FromISiteRole(role)
            };
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                model.Heading1 = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - {1}"], selectedSite.SiteName, role.RoleName);
            }
            else
            {
                model.Heading1 = role.RoleName;
            }
            
            model.Heading2 = StringLocalizer["Role Members"];
            model.SearchQuery = searchInput;

            model.Members = await RoleManager.GetUsersInRole(
                role.SiteId,
                role.Id,
                searchInput,
                pageNumber,
                itemsPerPage);
            
            return View(model);

        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [Authorize(Policy = PolicyConstants.RoleLookupPolicy)]
        public virtual async Task<IActionResult> RoleNonMembers(
            Guid? siteId,
            Guid roleId,
            string searchInput = "",
            int pageNumber = 1,
            int pageSize = 0,
            bool ajaxGrid = false)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId, true);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["{0} - Non Role Members"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = StringLocalizer["Non Role Members"];
            }
            
            var role = await RoleManager.FindByIdAsync(roleId.ToString());
            if ((role == null) || (role.SiteId != selectedSite.Id))
            {
                return RedirectToAction("Index");
            }

            var itemsPerPage = UIOptions.DefaultPageSize_RoleMemberList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = new RoleMemberListViewModel
            {
                Role = RoleViewModel.FromISiteRole(role)
            };
            if (selectedSite.Id != SiteManager.CurrentSite.Id)
            {
                model.Heading1 = string.Format(CultureInfo.CurrentUICulture, "{0} - {1}", selectedSite.SiteName, role.RoleName);
            }
            else
            {
                model.Heading1 = role.RoleName;
            }

            model.Heading2 = StringLocalizer["Non Role Members"];
            model.SearchQuery = searchInput; // unsafe input

            model.Members = await RoleManager.GetUsersNotInRole(
                role.SiteId,
                role.Id,
                searchInput,
                pageNumber,
                itemsPerPage);
            
            if (ajaxGrid)
            {
                return PartialView("NonMembersGridPartial", model);
            }

            return PartialView("NonMembersPartial", model);

        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.UserManagementPolicy)]
        [Authorize(Policy = PolicyConstants.RoleLookupPolicy)]
        public virtual async Task<IActionResult> AddUser(
            Guid? siteId, 
            Guid roleId, 
            Guid userId)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId, true);
            
            var user = await UserManager.Fetch(selectedSite.Id, userId);

            if (user != null)
            {
                var role = await RoleManager.FindByIdAsync(roleId.ToString());

                var canAdd = true;
                if(role.NormalizedRoleName == "ADMINISTRATORS")
                {
                    var adminAuthResult = await AuthorizationService.AuthorizeAsync(User, "AdminPolicy");
                    canAdd = adminAuthResult.Succeeded;
                }

                if(canAdd)
                {
                    if ((role != null) && (role.SiteId == selectedSite.Id))
                    {
                        await RoleManager.AddUserToRole(user, role);

                        this.AlertSuccess(string.Format(StringLocalizer["{0} was successfully added to the role {1}."],
                            user.DisplayName,
                            role.RoleName), true);
                    }
                }
                else
                {
                    this.AlertDanger(StringLocalizer["Sorry, but only other Administrators can add users to the Administrators role."], true);
                }
                

            }

            return RedirectToAction("RoleMembers", new { siteId = selectedSite.Id, roleId });
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.RoleAdminPolicy)]
        public virtual async Task<IActionResult> RemoveUser(
            Guid? siteId,
            Guid roleId, 
            Guid userId)
        {
            var selectedSite = await SiteManager.GetSiteForDataOperations(siteId, true);
            
            var user = await UserManager.Fetch(selectedSite.Id, userId);
            if (user != null)
            {
                var role = await RoleManager.FindByIdAsync(roleId.ToString());

                var canRemove = true;
                if (role.NormalizedRoleName == "ADMINISTRATORS")
                {
                    var adminAuthResult = await AuthorizationService.AuthorizeAsync(User, "AdminPolicy");
                    canRemove = adminAuthResult.Succeeded;
                }

                if(canRemove)
                {
                    if ((role != null) && (role.SiteId == selectedSite.Id))
                    {
                        await RoleManager.RemoveUserFromRole(user, role);
                        this.AlertWarning(string.Format(
                            StringLocalizer["{0} was successfully removed from the role {1}."],
                            user.DisplayName, role.RoleName)
                            , true);
                    }
                }
                else
                {
                    this.AlertDanger(StringLocalizer["Sorry, but only other Administrators can remove users from the Administrators role."], true);
                }
                

            }

            return RedirectToAction("RoleMembers", new { siteId = selectedSite.Id, roleId });
        }

    }
}
