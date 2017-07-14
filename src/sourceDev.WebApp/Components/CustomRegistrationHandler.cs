// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:	                2017-07-01
// Last Modified:           2017-07-14
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ExtensionPoints;
using cloudscribe.Core.Web.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System;
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
            else
            {
                viewData["FirstName"] = firstName;
            }

            var lastName = httpContext.Request.Form["LastName"];
            if (string.IsNullOrWhiteSpace(lastName))
            {
                modelState.AddModelError("lastNameError", "Last Name is required");
                result = false;
            }
            else
            {
                viewData["LastName"] = lastName;
            }
            var dobString = httpContext.Request.Form["DateOfBirth"];
            if (string.IsNullOrWhiteSpace(dobString))
            {
                modelState.AddModelError("DOBError", "Date of Birth is required");
                result = false;
            }
            else
            {
                DateTime dob;
                var dobParsed = DateTime.TryParse(dobString, out dob);
                if(!dobParsed)
                {
                    modelState.AddModelError("DOBError", "Date of Birth must be a valid date");
                    result = false;
                }
                else
                {
                    viewData["DateOfBirth"] = dobString;
                }
            }


            return Task.FromResult(result);
        }

        public Task ProcessUserBeforeCreate(ISiteUser siteUser, HttpContext httpContext)
        {
            if (siteUser != null)
            {
                siteUser.FirstName = httpContext.Request.Form["FirstName"];
                siteUser.LastName = httpContext.Request.Form["LastName"];
                
                var dobString = httpContext.Request.Form["DateOfBirth"];

                DateTime dob;
                var dobParsed = DateTime.TryParse(dobString, out dob);
                if (!dobParsed)
                {
                    siteUser.DateOfBirth = dob.Date;
                }

                // we don't need to save the user here it is saved after this method
            }

            return Task.FromResult(0);
        }

        public Task HandleRegisterPostSuccess(
            ISiteContext site,
            RegisterViewModel viewModel,
            HttpContext httpContext,
            UserLoginResult loginResult,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            
            if(loginResult.User != null)
            {
                // here is where you could process additional custom fields into your own custom data storage
                // if only updating native properties of siteuser use the method above
            }
            else
            {
                log.LogError("user was null in HandleRegisterPostSuccess, unable to update user with custom data");
            }

            return Task.FromResult(0);

        }

    }
}
