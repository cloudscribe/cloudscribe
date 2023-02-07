// Copyright (c) Idox Software Ltd All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Simon Annetts/ESDM
// Created:					2022-02-07
// Last Modified:			2022-02-07
//

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Web.Common.Analytics.GA4;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Analytics.GA4
{
    public class GoogleAccountAnalytics : IHandleAccountAnalytics
    {
        public GoogleAccountAnalytics(
            SiteContext currentSite,
            GoogleAnalyticsGA4Helper analyticsHelper,
            IOptions<GoogleAnalyticsGA4Options> optionsAccessor,
            IHttpContextAccessor contextAccessor
            )
        {
            _currentSite = currentSite;
            _analyticsHelper = analyticsHelper;
            _options = optionsAccessor.Value;
            _contextAccessor = contextAccessor;
        }

        private SiteContext _currentSite;
        private GoogleAnalyticsGA4Helper _analyticsHelper;
        private GoogleAnalyticsGA4Options _options;
        private IHttpContextAccessor _contextAccessor;

        public Task HandleLoginSubmit(string source)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _options.LoginSubmitEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                _analyticsHelper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

        public Task HandleLoginFail(string source, string reason)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _options.LoginFailEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                e.Parameters.Add(new KeyValuePair<string, string>("reason", reason));
                _analyticsHelper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

        public Task HandleRegisterSubmit(string source)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _options.RegisterSubmitEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                _analyticsHelper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

        public Task HandleRegisterFail(string source, string reason)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _options.RegisterFailEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                e.Parameters.Add(new KeyValuePair<string, string>("reason", reason));
                _analyticsHelper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

        public async Task HandleLoginSuccess(UserLoginResult result)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(result.IsNewUserRegistration)
                {
                    await HandleRegisterSuccess(result);
                    return;
                }

                var source = "Onsite";
                if (result.ExternalLoginInfo != null)
                {
                    source = result.ExternalLoginInfo.LoginProvider;
                }

                var e = new GoogleAnalyticsGA4Event();
                e.Name = _options.LoginSuccessEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                _analyticsHelper.AddEvent(e);
            }
        }

        private async Task HandleRegisterSuccess(UserLoginResult result)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var source = "Onsite";
                if (result.ExternalLoginInfo != null)
                {
                    source = result.ExternalLoginInfo.LoginProvider;
                }
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _options.RegisterSuccessEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                _analyticsHelper.AddEvent(e);
            }
        }


        public async Task HandleLoginNotAllowed(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if (result.IsNewUserRegistration)
                {
                    // first record successful registration
                    await HandleRegisterSuccess(result);

                    var source = "Onsite";
                    if (result.ExternalLoginInfo != null)
                    {
                        source = result.ExternalLoginInfo.LoginProvider;
                    }

                    var reason = "Login not allowed";
                    if (result.MustAcceptTerms) { reason = "User must accept terms of use"; }
                    if (result.NeedsPhoneConfirmation) { reason = "Needs phone number confirmation"; }
                    if (result.NeedsEmailConfirmation) { reason = "Needs email confirmation"; }
                    if (result.NeedsAccountApproval) { reason = "Needs account approval"; }

                    var e = new GoogleAnalyticsGA4Event();
                    e.Name = _options.LoginFailEventName;
                    e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                    e.Parameters.Add(new KeyValuePair<string, string>("reason", reason));
                    _analyticsHelper.AddEvent(e);
                }
            }
        }


        public Task HandleRequiresTwoFactor(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var source = "Onsite";
                if (result.ExternalLoginInfo != null)
                {
                    source = result.ExternalLoginInfo.LoginProvider;
                }
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _options.LoginRequires2FaEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                _analyticsHelper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

        public Task HandleLockout(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var source = "Onsite";
                if (result.ExternalLoginInfo != null)
                {
                    source = result.ExternalLoginInfo.LoginProvider;
                }
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _options.LoginLockoutEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                _analyticsHelper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

        public Task HandleLogout(string reason)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _options.LogoutEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("reason", reason));
                _analyticsHelper.AddEvent(e);
            }
            return Task.FromResult(0);
        }
    }
}
