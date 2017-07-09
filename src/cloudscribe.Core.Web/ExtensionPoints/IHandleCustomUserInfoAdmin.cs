// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-09
// Last Modified:			2017-07-09
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.ExtensionPoints
{
    public interface IHandleCustomUserInfoAdmin
    {
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
    }

    public class NoUserEditCustomization : IHandleCustomUserInfoAdmin
    {
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
            return Task.FromResult(0);
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
            return Task.FromResult(0);
        }

    }

}
