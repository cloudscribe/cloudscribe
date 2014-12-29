// Author:					Joe Audette
// Created:					2014-12-06
// Last Modified:			2014-12-06
// 

using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.RoleAdmin;
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
        public async Task<ActionResult> Index()
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Role Management";
            //ViewBag.Heading = "Role Management";

            RoleListViewModel model = new RoleListViewModel();
            model.Heading = "Role Management";
            model.SiteRoles = Site.UserRepository.GetRolesBySite(Site.SiteSettings.SiteId);

            return View(model);


        }

        // GET: /SiteAdmin/SiteInfo
        [Authorize(Roles = "Admins,Role Admins")]
        [MvcSiteMapNode(Title = "New Role", ParentKey = "Roles", Key = "RoleEdit")]
        public async Task<ActionResult> RoleEdit(int? roleId)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Role Edit";

            //Site.UserRepository.FetchRole()

            RoleViewModel model = new RoleViewModel();

            if (roleId.HasValue)
            {
                ISiteRole role = Site.UserRepository.FetchRole(roleId.Value);
                if ((role != null) && (role.SiteId == Site.SiteSettings.SiteId || Site.SiteSettings.IsServerAdminSite))
                {
                    model = RoleViewModel.FromISiteRole(role);

                    // below we are just manipiulating the bread crumbs
                    ISiteMap map = SiteMaps.GetSiteMap();
                    if (map.CurrentNode != null)
                    {
                        map.CurrentNode.Title = "Edit Role";
                        

                    }
                }
            }
            else
            {
                model.SiteGuid = Site.SiteSettings.SiteGuid;
                model.SiteId = Site.SiteSettings.SiteId;
            }

            model.Heading = "Role Edit";



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

            if ((model.SiteId == -1) || (model.SiteGuid == Guid.Empty))
            {
                model.SiteId = Site.SiteSettings.SiteId;
                model.SiteGuid = Site.SiteSettings.SiteGuid;
            }

            Site.UserRepository.SaveRole(model);

            return RedirectToAction("Roles", new { pageNumber = returnPageNumber });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<ActionResult> RoleDelete(int roleId, int returnPageNumber = 1)
        {
            ISiteRole role = Site.UserRepository.FetchRole(roleId);
            if (role != null && role.IsDeletable(AppSettings.RolesThatCannotBeDeleted))
            {
                Site.UserRepository.DeleteRole(roleId);
            }


            return RedirectToAction("Index", new { pageNumber = returnPageNumber });
        }

        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<ActionResult> RoleMembers(
            int roleId,
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

            int totalPages = 0;
            IList<IUserInfo> members = Site.UserRepository.GetUsersInRole(
                role.SiteId,
                role.RoleId, 
                pageNumber, 
                itemsPerPage, 
                out totalPages);

            model.Members = members;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalPages = totalPages;

            return View(model);

        }

        [Authorize(Roles = "Admins,Role Admins")]
        public async Task<ActionResult> RoleNonMembers(
            int roleId,
            int pageNumber = 1,
            int pageSize = -1)
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

            int totalPages = 0;
            IList<IUserInfo> members = Site.UserRepository.GetUsersNotInRole(
                role.SiteId,
                role.RoleId,
                pageNumber,
                itemsPerPage,
                out totalPages);

            model.Members = members;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalPages = totalPages;

            return View("NonMembersPartial",model);

        }

        [HttpPost]
        [Authorize(Roles = "Admins,Role Admins")]
        public ActionResult AddUser(int roleId, Guid roleGuid, int userId, Guid userGuid)
        {
            Site.UserRepository.AddUserToRole(roleId, roleGuid, userId, userGuid);
            //return RedirectToAction("Index");
            return RedirectToAction("RoleMembers", new { roleId = roleId });
        }

        [HttpPost]
        [Authorize(Roles = "Admins,Role Admins")]
        public ActionResult RemoveUser(int roleId, int userId)
        {
            Site.UserRepository.RemoveUserFromRole(roleId, userId);
            return RedirectToAction("RoleMembers", new { roleId = roleId });
        }

    }
}
