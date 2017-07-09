// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-09
// Last Modified:			2017-07-09
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.ExtensionPoints
{
    public interface IHandleCustomUserInfo
    {
        Task<string> GetUserInfoViewName(
            ISiteContext site,
            SiteUser siteUser,
            HttpContext httpContext);

        Task HandleUserInfoGet(
            ISiteContext site,
            SiteUser siteUser,
            UserInfoViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<bool> HandleUserInfoValidation(
            ISiteContext site,
            SiteUser siteUser,
            UserInfoViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            ModelStateDictionary modelState,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task HandleUserInfoPostSuccess(
            ISiteContext site,
            SiteUser siteUser,
            UserInfoViewModel viewModel,
            HttpContext httpContext,
            CancellationToken cancellationToken = default(CancellationToken)
            );

    }

    public class NoUserInfoCustomization : IHandleCustomUserInfo
    {
        public Task<string> GetUserInfoViewName(ISiteContext site, SiteUser siteUser, HttpContext httpContext)
        {
            return Task.FromResult("UserInfo"); // this is just returning the default view name.
        }

        public Task HandleUserInfoGet(
            ISiteContext site,
            SiteUser siteUser,
            UserInfoViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.FromResult(0);
        }

        public Task<bool> HandleUserInfoValidation(
            ISiteContext site,
            SiteUser siteUser,
            UserInfoViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            ModelStateDictionary modelState,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.FromResult(true);
        }

        public Task HandleUserInfoPostSuccess(
            ISiteContext site,
            SiteUser siteUser,
            UserInfoViewModel viewModel,
            HttpContext httpContext,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.FromResult(0);
        }

    }
}
