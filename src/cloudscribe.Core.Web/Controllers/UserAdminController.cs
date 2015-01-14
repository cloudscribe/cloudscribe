// Author:					Joe Audette
// Created:					2014-12-08
// Last Modified:			2015-01-14
// 

using cloudscribe.Configuration;

using cloudscribe.AspNet.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using cloudscribe.Core.Web.ViewModels.UserAdmin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MvcSiteMapProvider;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Roles = "Admins")]
    public class UserAdminController : CloudscribeBaseController
    {
        public async Task<ActionResult> Index(
            string query = "",
            int sortMode = 2,
            int pageNumber = 1, 
            int pageSize = -1,
            int siteId = -1)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "User Management";
            //ViewBag.Heading = "Role Management";

            int itemsPerPage = AppSettings.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            int totalPages = 0;

            if(siteId != -1)
            {
                if(!Site.SiteSettings.IsServerAdminSite)
                {
                    siteId = Site.SiteSettings.SiteId;
                }
            }
            else
            {
                siteId = Site.SiteSettings.SiteId;
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

            siteMembers = Site.UserRepository.GetPage(
                siteId,
                pageNumber,
                itemsPerPage,
                query,
                sortMode, 
                out totalPages);
             

            UserListViewModel model = new UserListViewModel();
            model.Heading = "User Management";
            model.UserList = siteMembers;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalPages = totalPages;
            model.AlphaQuery = query; //TODO: sanitize

            return View(model);


        }

        public async Task<ActionResult> Search(
            string query = "",
            int sortMode = 2,
            int pageNumber = 1,
            int pageSize = -1,
            int siteId = -1)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "User Management";
            //ViewBag.Heading = "Role Management";

            int itemsPerPage = AppSettings.DefaultPageSize_UserList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            int totalPages = 0;

            if (siteId != -1)
            {
                if (!Site.SiteSettings.IsServerAdminSite)
                {
                    siteId = Site.SiteSettings.SiteId;
                }
            }
            else
            {
                siteId = Site.SiteSettings.SiteId;
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

            siteMembers = Site.UserRepository.GetUserAdminSearchPage(
                siteId,
                pageNumber,
                itemsPerPage,
                query,
                sortMode,
                out totalPages);


            UserListViewModel model = new UserListViewModel();
            model.Heading = "User Management";
            model.UserList = siteMembers;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalPages = totalPages;
            model.SearchQuery = query; //TODO: sanitize
            

            return View("Index", model);


        }

        public async Task<ActionResult> IpSearch(string ipQuery = "",int siteId = -1)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "User Management";
            //ViewBag.Heading = "Role Management";

            
            Guid siteGuid = Site.SiteSettings.SiteGuid;

            if (siteId != -1)
            {
                if (Site.SiteSettings.IsServerAdminSite)
                {
                    ISiteSettings otherSite = Site.SiteRepository.Fetch(siteId);
                    if(otherSite != null)
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

            siteMembers = Site.UserRepository.GetByIPAddress(
                siteGuid,
                ipQuery);


            UserListViewModel model = new UserListViewModel();
            model.Heading = "User Management";
            model.UserList = siteMembers;
            //model.Paging.CurrentPage = pageNumber;
            //model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalPages = 1;
            model.IpQuery = ipQuery; //TODO: sanitize
            model.ShowAlphaPager = false;

            return View("Index", model);


        }


        //[Authorize(Roles = "Admins")]
        //[MvcSiteMapNode(Title = "New User", ParentKey = "UserAdmin", Key = "UserEdit")]
        public async Task<ActionResult> UserEdit(int? userId)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "New User";

            EditUserViewModel model = new EditUserViewModel();
            model.SiteGuid = Site.SiteSettings.SiteGuid;

            if(userId.HasValue)
            {
                ISiteUser user = await Site.UserRepository.Fetch(Site.SiteSettings.SiteId, userId.Value);
                if(user != null)
                {
                    model.UserId = user.UserId;
                    model.Email = user.Email;
                    model.FirstName = user.FirstName;
                    model.LastName = user.LastName;
                    model.LoginName = user.UserName;
                    model.DisplayName = user.DisplayName;

                    ViewBag.Title = "Manage User";

                    var node = SiteMaps.Current.FindSiteMapNodeFromKey("UserEdit");
                    if (node != null)
                    {
                        node.Title = "Manage User";
                    }
                }
                

            }
            
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserEdit(EditUserViewModel model)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "New User";

            if (ModelState.IsValid)
            {
                if(model.UserId > -1)
                {
                    //editing an existing user
                    ISiteUser user = await Site.UserRepository.Fetch(Site.SiteSettings.SiteId, model.UserId);
                    if (user != null)
                    {
                        user.Email = model.Email;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.UserName = model.LoginName;
                        user.DisplayName = model.DisplayName;
                        bool result = await Site.UserRepository.Save(user);
                        if(result)
                        {
                            this.AlertSuccess(string.Format("user account for <b>{0}</b> was successfully updated.",
                            user.DisplayName), true);
                        }
                        

                        return RedirectToAction("Index", "UserAdmin");
                    }
                }
                else
                {
                    var user = new SiteUser {
                        UserName = model.LoginName, 
                        Email = model.Email ,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DisplayName = model.DisplayName
                    };
                    var result = await Site.SiteUserManager.CreateAsync(user, model.Password);
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
                ModelState.AddModelError("", error);
            }
        }

    }
}
