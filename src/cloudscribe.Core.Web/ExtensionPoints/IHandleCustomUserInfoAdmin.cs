// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-09
// Last Modified:			2018-01-24
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.UserAdmin;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.ExtensionPoints
{
    public interface IHandleCustomUserInfoAdmin
    {
        Task<string> GetNewUserViewName(
            ISiteContext site,
            HttpContext httpContext);

        Task HandleNewUserGet(
            ISiteContext site,
            NewUserViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<bool> HandleNewUserValidation(
            ISiteContext site,
            NewUserViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            ModelStateDictionary modelState,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task ProcessUserBeforeCreate(ISiteUser siteUser, HttpContext httpContext);

        Task HandleNewUserPostSuccess(
            ISiteContext site,
            ISiteUser siteUser,
            NewUserViewModel viewModel,
            HttpContext httpContext,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<string> GetUserEditViewName(
            ISiteContext site,
            HttpContext httpContext);

        Task HandleUserEditGet(
            ISiteContext site,
            EditUserViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<bool> HandleUserEditValidation(
            ISiteContext site,
            EditUserViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            ModelStateDictionary modelState,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task HandleUserEditPostSuccess(
            ISiteContext site,
            ISiteUser siteUser,
            EditUserViewModel viewModel,
            HttpContext httpContext,
            CancellationToken cancellationToken = default(CancellationToken)
            );


        Task<PagedResult<IUserInfo>> GetCustomUserAdminSearchPage(
            Guid siteId, 
            int pageNumber, 
            int pageSize, 
            string searchInput, 
            int sortMode,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<string> GetUserListViewName(
            ISiteContext site,
            HttpContext httpContext);
    }

    public class NoUserEditCustomization : IHandleCustomUserInfoAdmin
    {
        public Task<string> GetNewUserViewName(
            ISiteContext site,
            HttpContext httpContext)
        {
            return Task.FromResult("NewUser"); // this is just returning the default view name.
        }

        public Task HandleNewUserGet(
            ISiteContext site,
            NewUserViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.CompletedTask;
        }

        public Task<bool> HandleNewUserValidation(
            ISiteContext site,
            NewUserViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            ModelStateDictionary modelState,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.FromResult(true);
        }

        public Task ProcessUserBeforeCreate(ISiteUser siteUser, HttpContext httpContext)
        {
            return Task.CompletedTask;
        }

        public Task HandleNewUserPostSuccess(
            ISiteContext site,
            ISiteUser siteUser,
            NewUserViewModel viewModel,
            HttpContext httpContext,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.CompletedTask;
        }


        public Task<string> GetUserEditViewName(
            ISiteContext site,
            HttpContext httpContext)
        {
            return Task.FromResult("UserEdit"); // this is just returning the default view name.
        }

        public Task HandleUserEditGet(
            ISiteContext site,
            EditUserViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.CompletedTask;
        }

        public Task<bool> HandleUserEditValidation(
            ISiteContext site,
            EditUserViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            ModelStateDictionary modelState,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.FromResult(true);
        }

        public Task HandleUserEditPostSuccess(
            ISiteContext site,
            ISiteUser siteUser,
            EditUserViewModel viewModel,
            HttpContext httpContext,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.CompletedTask;
        }

        public Task<PagedResult<IUserInfo>> GetCustomUserAdminSearchPage(
            Guid siteId, 
            int pageNumber, 
            int pageSize, 
            string searchInput, 
            int sortMode, 
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(null as PagedResult<IUserInfo>);
        }

        public Task<string> GetUserListViewName(ISiteContext site, HttpContext httpContext)
        {
            return Task.FromResult("Index"); // this is just returning the default view name.
        }
    }
}
