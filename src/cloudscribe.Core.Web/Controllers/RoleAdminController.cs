// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-06
// Last Modified:			2018-03-14
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
using System.Globalization;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers.Mvc
{
    
    public class RoleAdminController : Controller
    {
        public RoleAdminController(
            SiteManager siteManager,
            SiteUserManager<SiteUser> userManager,
            SiteRoleManager<SiteRole> roleManager,
            IStringLocalizer<CloudscribeCore> localizer,
            IOptions<UIOptions> uiOptionsAccessor,
            IOptions<SiteConfigOptions> setupOptionsAccessor
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _siteManager = siteManager;
            _uiOptions = uiOptionsAccessor.Value;
            _setupOptions = setupOptionsAccessor.Value;
            _sr = localizer;
        }

        private SiteManager _siteManager;
        private UIOptions _uiOptions;
        private IStringLocalizer _sr;
        private SiteConfigOptions _setupOptions;
        private SiteUserManager<SiteUser> _userManager;
        private SiteRoleManager<SiteRole> _roleManager;


        [HttpGet]
        [Authorize(Policy = PolicyConstants.RoleAdminPolicy)]
        public async Task<IActionResult> Index(
            Guid? siteId,
            string searchInput = "", 
            int pageNumber = 1, 
            int pageSize = -1)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - Role Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["Role Management"];
            }

            var model = new RoleListViewModel
            {
                SiteId = selectedSite.Id
            };

            int itemsPerPage = _uiOptions.DefaultPageSize_RoleList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            
            model.SiteRoles = await _roleManager.GetRolesBySite(
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
        public async Task<IActionResult> Modal(
            Guid? siteId,
            string searchInput = "",
            bool ajaxGrid = false,
            int pageNumber = 1,
            int pageSize = -1)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId, true);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            { 
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - Role Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["Role Management"];
            }

            var model = new RoleListViewModel
            {
                SiteId = selectedSite.Id
            };

            int itemsPerPage = _uiOptions.DefaultPageSize_RoleList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            if (searchInput == null) searchInput = string.Empty;
            
            model.SiteRoles = await _roleManager.GetRolesBySite(
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
        [Authorize(Policy = PolicyConstants.RoleAdminPolicy)]
        public async Task<IActionResult> RoleEdit(
            Guid? siteId,
            Guid? roleId)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId, true);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - New Role"], selectedSite.SiteName);  
            }
            else
            {
                ViewData["Title"] = _sr["New Role"];
            }

            var model = new RoleViewModel
            {
                SiteId = selectedSite.Id
            };

            if (roleId.HasValue)
            {
                var role = await _roleManager.FindByIdAsync(roleId.Value.ToString());
                if ((role != null) && (role.SiteId == selectedSite.Id))
                {
                    model = RoleViewModel.FromISiteRole(role);
                    ViewData["Title"] = _sr["Edit Role"];
                    var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
                    {
                        KeyToAdjust = "RoleEdit",
                        AdjustedText = _sr["Edit Role"],
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
        public async Task<IActionResult> RoleEdit(RoleViewModel model, int returnPageNumber = 1)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(model.SiteId, true);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - Edit Role"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["Edit Role"];
            }
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string successFormat;
            var role = SiteRole.FromISiteRole(model);
            if (model.Id == Guid.Empty)
            {
                var exists = await _roleManager.RoleExistsAsync(model.RoleName);

                if(exists)
                {
                    ModelState.AddModelError("roleerror", _sr["The role name is already in use."]);
                    return View(model);
                }

                role.SiteId = selectedSite.Id;
                successFormat = _sr["The role <b>{0}</b> was successfully created."];
                await _roleManager.CreateAsync(role);
            }
            else
            {
                successFormat = _sr["The role <b>{0}</b> was successfully updated."];
                await _roleManager.UpdateAsync(role);
            }
            
            this.AlertSuccess(string.Format(successFormat, role.RoleName), true);
            
            return RedirectToAction("Index", new { siteId = selectedSite.Id, pageNumber = returnPageNumber });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.RoleAdminPolicy)]
        public async Task<IActionResult> RoleDelete(
            Guid? siteId,
            Guid roleId, 
            int returnPageNumber = 1)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId, true);
            
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role != null && role.SiteId == selectedSite.Id && role.IsDeletable(_setupOptions.RolesThatCannotBeDeleted))
            {
                await _roleManager.DeleteUserRolesByRole(roleId);
                await _roleManager.DeleteRole(roleId);
                
                string successFormat = _sr["The role <b>{0}</b> was successfully deleted."];

                this.AlertWarning(string.Format(successFormat,role.RoleName), true);
                
            }

            return RedirectToAction("Index", new { siteId = selectedSite.Id, pageNumber = returnPageNumber });
        }


        [HttpGet]
        [Authorize(Policy = PolicyConstants.RoleAdminPolicy)]
        public async Task<IActionResult> RoleMembers(
            Guid? siteId,
            Guid roleId,
            string searchInput = "",
            int pageNumber = 1,
            int pageSize = -1)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId, true);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - Role Members"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["Role Members"];
            }
            
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if ((role == null) || (role.SiteId != selectedSite.Id ))
            {
                return RedirectToAction("Index");
            }

            int itemsPerPage = _uiOptions.DefaultPageSize_RoleMemberList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            RoleMemberListViewModel model = new RoleMemberListViewModel
            {
                SiteId = selectedSite.Id,
                UseEmailForLogin = selectedSite.UseEmailForLogin,
                Role = RoleViewModel.FromISiteRole(role)
            };
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                model.Heading1 = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - {1}"], selectedSite.SiteName, role.RoleName);
            }
            else
            {
                model.Heading1 = role.RoleName;
            }
            
            model.Heading2 = _sr["Role Members"];
            model.SearchQuery = searchInput;

            model.Members = await _roleManager.GetUsersInRole(
                role.SiteId,
                role.Id,
                searchInput,
                pageNumber,
                itemsPerPage);
            
            return View(model);

        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.RoleAdminPolicy)]
        public async Task<IActionResult> RoleNonMembers(
            Guid? siteId,
            Guid roleId,
            string searchInput = "",
            int pageNumber = 1,
            int pageSize = 0,
            bool ajaxGrid = false)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId, true);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["{0} - Non Role Members"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = _sr["Non Role Members"];
            }
            
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if ((role == null) || (role.SiteId != selectedSite.Id))
            {
                return RedirectToAction("Index");
            }

            var itemsPerPage = _uiOptions.DefaultPageSize_RoleMemberList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = new RoleMemberListViewModel
            {
                Role = RoleViewModel.FromISiteRole(role)
            };
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                model.Heading1 = string.Format(CultureInfo.CurrentUICulture, "{0} - {1}", selectedSite.SiteName, role.RoleName);
            }
            else
            {
                model.Heading1 = role.RoleName;
            }

            model.Heading2 = _sr["Non Role Members"];
            model.SearchQuery = searchInput; // unsafe input

            model.Members = await _roleManager.GetUsersNotInRole(
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
        [Authorize(Policy = PolicyConstants.RoleAdminPolicy)]
        public async Task<IActionResult> AddUser(
            Guid? siteId, 
            Guid roleId, 
            Guid userId)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId, true);
            
            var user = await _userManager.Fetch(selectedSite.Id, userId);

            if (user != null)
            {
                var role = await _roleManager.FindByIdAsync(roleId.ToString());
                if ((role != null) && (role.SiteId == selectedSite.Id))
                {
                    await _roleManager.AddUserToRole(user, role);
                    
                    this.AlertSuccess(string.Format(_sr["{0} was successfully added to the role {1}."],
                        user.DisplayName, 
                        role.RoleName), true);
                }

            }

            return RedirectToAction("RoleMembers", new { siteId = selectedSite.Id, roleId });
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.RoleAdminPolicy)]
        public async Task<IActionResult> RemoveUser(
            Guid? siteId,
            Guid roleId, 
            Guid userId)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId, true);
            
            var user = await _userManager.Fetch(selectedSite.Id, userId);
            if (user != null)
            {
                var role = await _roleManager.FindByIdAsync(roleId.ToString());
                if ((role != null) && (role.SiteId == selectedSite.Id))
                {
                    await _roleManager.RemoveUserFromRole(user, role);
                    this.AlertWarning(string.Format(
                        _sr["{0} was successfully removed from the role {1}."],
                        user.DisplayName, role.RoleName)
                        , true);   
                }

            }

            return RedirectToAction("RoleMembers", new { siteId = selectedSite.Id, roleId });
        }

    }
}
