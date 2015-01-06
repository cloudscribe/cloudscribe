// Author:					Joe Audette
// Created:					2014-12-06
// Last Modified:			2015-01-02
// 

using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Controllers;
using cloudscribe.Core.Web.ViewModels.RoleAdmin;
using cloudscribe.Core.Web.ViewModels.Common;
using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Roles = "Admins,Role Admins")]
    public class RoleAdminController : CloudscribeBaseController
    {
        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<ActionResult> Index(string searchInput = "", int pageNumber = 1, int pageSize = -1)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Role Management";
            //ViewBag.Heading = "Role Management";

            RoleListViewModel model = new RoleListViewModel();
            model.Heading = "Role Management";

            int itemsPerPage = AppSettings.DefaultPageSize_CountryList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            int totalPages = 0;

            model.SiteRoles = Site.UserRepository.GetRolesBySite(
                Site.SiteSettings.SiteId, 
                searchInput, 
                pageNumber, 
                itemsPerPage, 
                out totalPages);

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalPages = totalPages;

            return View(model);


        }

        
        [Authorize(Roles = "Admins,Role Admins")]
        //[MvcSiteMapNode(Title = "New Role", ParentKey = "Roles", Key = "RoleEdit")]
        public async Task<ActionResult> RoleEdit(int? roleId)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "New Role";

            RoleViewModel model = new RoleViewModel();
            model.Heading = "New Role";

            if (roleId.HasValue)
            {
                ISiteRole role = Site.UserRepository.FetchRole(roleId.Value);
                if ((role != null) && (role.SiteId == Site.SiteSettings.SiteId || Site.SiteSettings.IsServerAdminSite))
                {
                    model = RoleViewModel.FromISiteRole(role);

                    ViewBag.Title = "Edit Role";
                    model.Heading = "Edit Role";

                    // below we are just manipiulating the bread crumbs
                    //var node = SiteMaps.Current.FindSiteMapNodeFromKey("RoleEdit");
                    var node = SiteMaps.Current.FindSiteMapNodeFromKey("RoleEdit");
                    if (node != null)
                    {
                        node.Title = "Edit Role";
                    }
                }
            }
            else
            {
                model.SiteGuid = Site.SiteSettings.SiteGuid;
                model.SiteId = Site.SiteSettings.SiteId;
            }

            
            return View(model);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<ActionResult> RoleEdit(RoleViewModel model, int returnPageNumber = 1)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit Role";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string successFormat;
            
            if ((model.SiteId == -1) || (model.SiteGuid == Guid.Empty))
            {
                model.SiteId = Site.SiteSettings.SiteId;
                model.SiteGuid = Site.SiteSettings.SiteGuid;
                successFormat = "The role <b>{0}</b> was successfully created.";
            }
            else
            {
                successFormat = "The role <b>{0}</b> was successfully updated.";
            }

            bool result = Site.UserRepository.SaveRole(model);

            if(result)
            {
                this.AlertSuccess(string.Format(successFormat,
                            model.DisplayName), true);
            }

            return RedirectToAction("Index", new { pageNumber = returnPageNumber });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<ActionResult> RoleDelete(int roleId, int returnPageNumber = 1)
        {
            ISiteRole role = Site.UserRepository.FetchRole(roleId);
            if (role != null && role.IsDeletable(AppSettings.RolesThatCannotBeDeleted))
            {
                Site.UserRepository.DeleteUserRolesByRole(roleId);

                bool result = Site.UserRepository.DeleteRole(roleId);

                if(result)
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

        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<ActionResult> RoleMembers(
            int roleId,
            string searchInput = "",
            int pageNumber = 1,
            int pageSize = -1)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Role Members";

            ISiteRole role = Site.UserRepository.FetchRole(roleId);
            if((role == null) || (role.SiteId != Site.SiteSettings.SiteId && !Site.SiteSettings.IsServerAdminSite))
            {
                return RedirectToAction("Index");
            }

            int itemsPerPage = AppSettings.DefaultPageSize_RoleMemberList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            RoleMemberListViewModel model = new RoleMemberListViewModel();

            model.Role = RoleViewModel.FromISiteRole(role);
            model.Heading = "Role Members";
            model.SearchQuery = searchInput;

            int totalPages = 0;
            IList<IUserInfo> members = Site.UserRepository.GetUsersInRole(
                role.SiteId,
                role.RoleId, 
                searchInput,
                pageNumber, 
                itemsPerPage, 
                out totalPages);

            model.Members = members;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalPages = totalPages;

            var node = SiteMaps.Current.FindSiteMapNodeFromKey("RoleMembers");
            if (node != null)
            {
                node.Title = model.Role.DisplayName + " Role Members";
            }

            return View(model);

        }

        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<ActionResult> RoleNonMembers(
            int roleId,
            string searchInput = "",
            int pageNumber = 1,
            int pageSize = -1,
            bool ajaxGrid = false)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Non Role Members";

            ISiteRole role = Site.UserRepository.FetchRole(roleId);
            if ((role == null) || (role.SiteId != Site.SiteSettings.SiteId && !Site.SiteSettings.IsServerAdminSite))
            {
                return RedirectToAction("Index");
            }

            int itemsPerPage = AppSettings.DefaultPageSize_RoleMemberList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            RoleMemberListViewModel model = new RoleMemberListViewModel();

            model.Role = RoleViewModel.FromISiteRole(role);
            model.Heading = "Non Role Members";
            model.SearchQuery = searchInput;

            int totalPages = 0;
            IList<IUserInfo> members = Site.UserRepository.GetUsersNotInRole(
                role.SiteId,
                role.RoleId,
                searchInput,
                pageNumber,
                itemsPerPage,
                out totalPages);

            model.Members = members;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalPages = totalPages;

            if (ajaxGrid)
            {
                return PartialView("NonMembersGridPartial", model);
            }

            return PartialView("NonMembersPartial",model);

        }

        [HttpPost]
        [Authorize(Roles = "Admins,Role Admins")]
        public ActionResult AddUser(int roleId, Guid roleGuid, int userId, Guid userGuid)
        {    
            ISiteUser user = Site.UserRepository.Fetch(Site.SiteSettings.SiteId, userId);
            
            if(user != null)
            {
                ISiteRole role = Site.UserRepository.FetchRole(roleId);
                if (role != null)
                {
                    bool result = Site.UserRepository.AddUserToRole(roleId, roleGuid, userId, userGuid);
                    if(result)
                    {
                        user.RolesChanged = true;
                        Site.UserRepository.Save(user);

                        this.AlertSuccess(string.Format("<b>{0}</b> was successfully added to the role {1}.",
                            user.DisplayName, role.DisplayName), true);
                    }            
                }
   
            }

            return RedirectToAction("RoleMembers", new { roleId = roleId });
        }

        [HttpPost]
        [Authorize(Roles = "Admins,Role Admins")]
        public ActionResult RemoveUser(int roleId, int userId)
        {
           
            ISiteUser user = Site.UserRepository.Fetch(Site.SiteSettings.SiteId, userId);
            if (user != null)
            {
                ISiteRole role = Site.UserRepository.FetchRole(roleId);
                if(role != null)
                {
                    bool result = Site.UserRepository.RemoveUserFromRole(roleId, userId);
                    if(result)
                    {
                        user.RolesChanged = true;
                        Site.UserRepository.Save(user);

                        this.AlertWarning(string.Format(
                            "<b>{0}</b> was successfully removed from the role {1}.",
                            user.DisplayName, role.DisplayName)
                            , true);
                    }
                    
                }
                
            }

            return RedirectToAction("RoleMembers", new { roleId = roleId });
        }

    }
}
