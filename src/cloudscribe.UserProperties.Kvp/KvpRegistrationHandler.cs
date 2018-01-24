// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-10
// Last Modified:			2017-07-14
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ExtensionPoints;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.UserProperties.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.UserProperties.Kvp
{
    public class KvpRegistrationHandler : IHandleCustomRegistration
    {
        public KvpRegistrationHandler(
            IProfileOptionsResolver customPropsResolver,
            IUserPropertyService userPropertyService,
            IUserPropertyValidator userPropertyValidator,
            ILogger<KvpRegistrationHandler> logger
            )
        {
            _customPropsResolver = customPropsResolver;
            _log = logger;
            _userPropertyValidator = userPropertyValidator;
            _userPropertyService = userPropertyService;
        }

        protected IProfileOptionsResolver _customPropsResolver;
        protected ILogger _log;
        protected UserPropertySet _props = null;
        protected IUserPropertyService _userPropertyService;
        protected IUserPropertyValidator _userPropertyValidator;

        private async Task EnsureProps()
        {
            if (_props == null)
            {
                _props = await _customPropsResolver.GetProfileProps();
            }
        }

        public virtual Task<string> GetRegisterViewName(ISiteContext site, HttpContext httpContext)
        {
            return Task.FromResult("Register"); // this is just returning the default view name.
        }

        public virtual async Task HandleRegisterGet(
            ISiteContext site,
            RegisterViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            await EnsureProps();
            // if any configured properties have default values add them to viewData
            // to pass them to the view
            foreach (var p in _props.Properties)
            {
                if(p.VisibleOnRegistration)
                {
                    if(!string.IsNullOrWhiteSpace(p.DefaultValue))
                    {
                        viewData[p.Key] = p.DefaultValue;
                    }
                }
            }
            
            
        }

        public virtual async Task<bool> HandleRegisterValidation(
            ISiteContext site,
            RegisterViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            ModelStateDictionary modelState,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            await EnsureProps();
            var result = true;

            foreach (var p in _props.Properties)
            {
                if (p.VisibleOnRegistration)
                {
                    var postedValue = httpContext.Request.Form[p.Key];
                    if(_userPropertyValidator.IsValid(p, postedValue, modelState))
                    {
                        // if valid keep the field populated in case some other model validation failed and the form is re-displayed
                        viewData[p.Key] = postedValue;
                    }
                    else
                    {
                        result = false;
                    }
                    
                }
            }
            
            return result;
        }

        public Task ProcessUserBeforeCreate(ISiteUser siteUser, HttpContext httpContext)
        {
            if (siteUser != null)
            {
                foreach (var p in _props.Properties)
                {
                    if (p.VisibleOnRegistration)
                    {
                        if (_userPropertyService.IsNativeUserProperty(p.Key))
                        {
                            var postedValue = httpContext.Request.Form[p.Key];
                            _userPropertyService.UpdateNativeUserProperty(siteUser, p.Key, postedValue);
                        }
                       
                    }
                }

                // we don't need to save the user here it is saved after this method
            }

            return Task.FromResult(0);
        }

        public virtual async Task HandleRegisterPostSuccess(
            ISiteContext site,
            RegisterViewModel viewModel,
            HttpContext httpContext,
            UserLoginResult loginResult,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            await EnsureProps();
            // we "could" re-validate here but
            // the method above gets called just before this in the same postback
            // so we know there were no validation errors or this method would not be invoked
            SiteUser siteUser = null;
            if(_userPropertyService.HasAnyNativeProps(_props.Properties))
            {
                siteUser = await _userPropertyService.GetUser(loginResult.User.Id.ToString());
            }
            if (loginResult.User != null)
            {
                foreach (var p in _props.Properties)
                {
                    if (p.VisibleOnRegistration)
                    {
                        if(!_userPropertyService.IsNativeUserProperty(p.Key))
                        {
                            var postedValue = httpContext.Request.Form[p.Key];
                            // persist to kvp storage
                            await _userPropertyService.CreateOrUpdate(
                                site.Id.ToString(),
                                loginResult.User.Id.ToString(),
                                p.Key,
                                postedValue);
                        }  
                    }
                }
    
            }
            else
            {
                _log.LogError("user was null in HandleRegisterPostSuccess, unable to update user with custom data");
            }
   
        }

    }
}
