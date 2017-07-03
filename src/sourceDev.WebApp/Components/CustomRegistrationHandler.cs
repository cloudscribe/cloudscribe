// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:	                2017-07-01
// Last Modified:           2017-07-01
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ExtensionPoints;
using cloudscribe.Core.Web.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace sourceDev.WebApp.Components
{
    /// <summary>
    /// this is just a proof of concept implementation of IHandleCustomRegistration. 
    /// to capture first name and last name as required fields during registration. Since there are existing data fields for those on siteuser
    /// this implementation is pretty simple. A more complex custom solution could store additional user data in a custom table.
    /// </summary>
    public class CustomRegistrationHandler : IHandleCustomRegistration
    {
        public CustomRegistrationHandler(
            SiteUserManager<SiteUser> userManager,
            ILogger<CustomRegistrationHandler> logger
            )
        {
            this.userManager = userManager;
            log = logger;
        }

        private SiteUserManager<SiteUser> userManager;
        private ILogger log;

        public Task<string> GetRegisterViewName(ISiteContext site, HttpContext httpContext)
        {
            return Task.FromResult("Register"); // this is just returning the default view name.
        }

        public Task HandleRegisterGet(
            ISiteContext site,
            RegisterViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            // just testing that custom fields can be pre-populated using ViewData
            //viewData["FirstName"] = "John";
            //viewData["LastName"] = "Doe";

            return Task.FromResult(0);
        }

        public Task<bool> HandleRegisterValidation(
            ISiteContext site,
            RegisterViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            ModelStateDictionary modelState,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            // just testing that server side validation can be implemented
            var result = true;
            var firstName = httpContext.Request.Form["FirstName"];
            if(string.IsNullOrWhiteSpace(firstName))
            {
                modelState.AddModelError("firstNameError", "First Name is required");
                result = false;
            }

            var lastName = httpContext.Request.Form["LastName"];
            if (string.IsNullOrWhiteSpace(lastName))
            {
                modelState.AddModelError("lastNameError", "Last Name is required");
                result = false;
            }


            return Task.FromResult(result);
        }

        public async Task HandleRegisterPostSuccess(
            ISiteContext site,
            RegisterViewModel viewModel,
            HttpContext httpContext,
            UserLoginResult loginResult,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            // just testing that on success the user can be updated with custom data
            if(loginResult.User != null)
            {
                var siteUser = await userManager.FindByIdAsync(loginResult.User.Id.ToString());
                if(siteUser != null)
                {
                    siteUser.FirstName = httpContext.Request.Form["FirstName"];
                    siteUser.LastName = httpContext.Request.Form["LastName"];
                    await userManager.UpdateAsync(siteUser);
                }

            }
            else
            {
                log.LogError("user was null in HandleRegisterPostSuccess, unable to update user with custom data");
            }
            
        }

    }
}
