// Copyright (c) Idox Software Ltd All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Simon Annetts/ESDM
// Created:					2022-02-07
// Last Modified:			2022-02-08
//

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Web.Common.Analytics.GA4;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Analytics
{
    public partial class GoogleAccountAnalytics : IHandleAccountAnalytics
    {
        public Task HandleGA4LoginSubmit(string source)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _optionsGA4.LoginSubmitEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                _analyticsGA4Helper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

        public Task HandleGA4LoginFail(string source, string reason)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _optionsGA4.LoginFailEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                e.Parameters.Add(new KeyValuePair<string, string>("reason", reason));
                _analyticsGA4Helper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

        public Task HandleGA4RegisterSubmit(string source)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _optionsGA4.RegisterSubmitEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                _analyticsGA4Helper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

        public Task HandleGA4RegisterFail(string source, string reason)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _optionsGA4.RegisterFailEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                e.Parameters.Add(new KeyValuePair<string, string>("reason", reason));
                _analyticsGA4Helper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

        public async Task HandleGA4LoginSuccess(UserLoginResult result)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(result.IsNewUserRegistration)
                {
                    await HandleGA4RegisterSuccess(result);
                    return;
                }

                var source = "Onsite";
                if (result.ExternalLoginInfo != null)
                {
                    source = result.ExternalLoginInfo.LoginProvider;
                }

                var e = new GoogleAnalyticsGA4Event();
                e.Name = _optionsGA4.LoginSuccessEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                _analyticsGA4Helper.AddEvent(e);
            }
        }

        private async Task HandleGA4RegisterSuccess(UserLoginResult result)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var source = "Onsite";
                if (result.ExternalLoginInfo != null)
                {
                    source = result.ExternalLoginInfo.LoginProvider;
                }
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _optionsGA4.RegisterSuccessEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                _analyticsGA4Helper.AddEvent(e);
            }
        }


        public async Task HandleGA4LoginNotAllowed(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if (result.IsNewUserRegistration)
                {
                    // first record successful registration
                    await HandleGA4RegisterSuccess(result);

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
                    e.Name = _optionsGA4.LoginFailEventName;
                    e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                    e.Parameters.Add(new KeyValuePair<string, string>("reason", reason));
                    _analyticsGA4Helper.AddEvent(e);
                }
            }
        }


        public Task HandleGA4RequiresTwoFactor(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var source = "Onsite";
                if (result.ExternalLoginInfo != null)
                {
                    source = result.ExternalLoginInfo.LoginProvider;
                }
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _optionsGA4.LoginRequires2FaEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                _analyticsGA4Helper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

        public Task HandleGA4Lockout(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var source = "Onsite";
                if (result.ExternalLoginInfo != null)
                {
                    source = result.ExternalLoginInfo.LoginProvider;
                }
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _optionsGA4.LoginLockoutEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("method", source));
                _analyticsGA4Helper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

        public Task HandleGA4Logout(string reason)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _optionsGA4.LogoutEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("reason", reason));
                _analyticsGA4Helper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

        public Task HandleGA4Search(string searchQuery, int numResults)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsGA4Event();
                e.Name = _optionsGA4.SearchEventName;
                e.Parameters.Add(new KeyValuePair<string, string>("search_term", searchQuery));
                e.Parameters.Add(new KeyValuePair<string, string>("number_of_results", numResults.ToString()));
                _analyticsGA4Helper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

        /// <summary>
        /// This is a generic event handler for any custom event you want to track
        /// </summary>
        public Task HandleGA4Event(string eventName, List<KeyValuePair<string,string>> parameters)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsGA4Event();
                e.Name = eventName;
                e.Parameters = parameters;
                _analyticsGA4Helper.AddEvent(e);
            }
            return Task.FromResult(0);
        }

    }
}
