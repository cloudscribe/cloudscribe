// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// Author:                  Joe Audette
// Created:                 2017-09-21
// Last Modified:           2017-09-21
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Web.Common.Analytics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Analytics
{
    public class GoogleAccountAnalytics : IHandleAccountAnalytics
    {
        public GoogleAccountAnalytics(
            SiteContext currentSite,
            IOptions<GoogleAnalyticsOptions> optionsAccessor,
            ITempDataDictionaryFactory tempaDataFactory,
            IHttpContextAccessor contextAccessor
            )
        {
            _currentSite = currentSite;
            _options = optionsAccessor.Value;
            _tempaDataFactory = tempaDataFactory;
            _contextAccessor = contextAccessor;
        }

        private SiteContext _currentSite;
        private GoogleAnalyticsOptions _options;
        private IHttpContextAccessor _contextAccessor;
        private ITempDataDictionaryFactory _tempaDataFactory;

        
        private ITempDataDictionary GetTempData()
        {
            return _tempaDataFactory.GetTempData(_contextAccessor.HttpContext);
        }

        private void AddGoogleAnalyticsEvent(GoogleAnalyticsEvent ev)
        {
            var tempData = GetTempData();
            if(tempData != null)
            {
                tempData.AddEvent(ev);  //these are detected by the google analytics taghelper
            }
        }

        public Task HandleLoginSubmit(string source)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsEvent();
                e.Category = _options.LoginRegisterEventCategory;
                e.Action = _options.LoginSubmitEventAction;
                e.Label = source;
                e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginSubmitMetricIndex.ToInvariantString(), "1"));

                AddGoogleAnalyticsEvent(e);
            }

            return Task.FromResult(0);
        }

        public Task HandleLoginFail(string source, string reason)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsEvent();
                e.Category = _options.LoginRegisterEventCategory;
                e.Action = _options.LoginFailEventAction;
                e.Label = source;
                e.Value = reason;
                e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginFailMetricIndex.ToInvariantString(), "1"));

                AddGoogleAnalyticsEvent(e);
            }

            return Task.FromResult(0);
        }

        public Task HandleRegisterSubmit(string source)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsEvent();
                e.Category = _options.LoginRegisterEventCategory;
                e.Action = _options.RegisterSubmitEventAction;
                e.Label = source;
                e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.RegisterSubmitMetricIndex.ToInvariantString(), "1"));

                AddGoogleAnalyticsEvent(e);
            }

            return Task.FromResult(0);
        }

        public Task HandleRegisterFail(string source, string reason)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsEvent();
                e.Category = _options.LoginRegisterEventCategory;
                e.Action = _options.RegisterFailEventAction;
                e.Label = source;
                e.Value = reason;
                e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.RegisterFailMetricIndex.ToInvariantString(), "1"));

                AddGoogleAnalyticsEvent(e);
            }

            return Task.FromResult(0);
        }

        public Task HandleLoginSuccess(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(result.IsNewUserRegistration)
                {
                    HandleRegisterSuccess(result);
                    return Task.FromResult(0);
                }

                var e = new GoogleAnalyticsEvent();
                e.Category = _options.LoginRegisterEventCategory;

                e.Action = _options.LoginSuccessEventAction;
                
                var source = "Onsite";
                if(result.ExternalLoginInfo != null)
                {
                    source = result.ExternalLoginInfo.LoginProvider;
                }
               
                e.Label = source;
                if(result.User != null)
                {
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.UserIdDimensionIndex.ToInvariantString(), result.User.Id.ToString()));
                }
                e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.RegisteredUserDimensionIdex.ToInvariantString(), "Yes"));
                e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginSuccessMetricIndex.ToInvariantString(), "1"));
                
                AddGoogleAnalyticsEvent(e);
            }

            return Task.FromResult(0);
        }

        public Task HandleLoginNotAllowed(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if (result.IsNewUserRegistration)
                {
                    // first record successful registration
                    HandleRegisterSuccess(result);

                    // next reason login isn't allowed
                    var e = new GoogleAnalyticsEvent();
                    e.Category = _options.LoginRegisterEventCategory;
                    e.Action = _options.LoginFailEventAction;
                    var source = "Onsite";

                    if (result.ExternalLoginInfo != null)
                    {
                        source = result.ExternalLoginInfo.LoginProvider;
                    }

                    e.Label = source;
                    var reason = "Login not allowed";

                    if (result.MustAcceptTerms) { reason = "User must accept terms of use"; }
                    if (result.NeedsPhoneConfirmation) { reason = "Needs phone number confirmation"; }
                    if (result.NeedsEmailConfirmation) { reason = "Needs email confirmation"; }
                    if (result.NeedsAccountApproval) { reason = "Needs account approval"; }
                    
                    e.Value = reason;
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                    e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginFailMetricIndex.ToInvariantString(), "1"));

                    AddGoogleAnalyticsEvent(e);


                }
            }

            return Task.FromResult(0);
        }

        private void HandleRegisterSuccess(UserLoginResult result)
        {
            var e = new GoogleAnalyticsEvent();
            e.Category = _options.LoginRegisterEventCategory;
            
            e.Action = _options.RegisterSuccessEventAction;
           
            var source = "Onsite";
            if (result.ExternalLoginInfo != null)
            {
                source = result.ExternalLoginInfo.LoginProvider;
            }

            e.Label = source;
            if (result.User != null)
            {
                e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.UserIdDimensionIndex.ToInvariantString(), result.User.Id.ToString()));
            }
            e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.RegisteredUserDimensionIdex.ToInvariantString(), "Yes"));
            e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
            
            e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.RegisterSuccessMetricIndex.ToInvariantString(), "1"));
            
            AddGoogleAnalyticsEvent(e);
        }

        public Task HandleRequiresTwoFactor(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsEvent();
                e.Category = _options.LoginRegisterEventCategory;
                e.Action = _options.LoginFailEventAction;
                var source = "Onsite";
                if (result.ExternalLoginInfo != null)
                {
                    source = result.ExternalLoginInfo.LoginProvider;
                }
                e.Label = source;
                e.Value = "Requires 2 Factor";
                e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginFailMetricIndex.ToInvariantString(), "1"));

                AddGoogleAnalyticsEvent(e);
            }

            return Task.FromResult(0);
        }

        public Task HandleLockout(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                var e = new GoogleAnalyticsEvent();
                e.Category = _options.LoginRegisterEventCategory;
                e.Action = _options.LoginFailEventAction;
                var source = "Onsite";
                if (result.ExternalLoginInfo != null)
                {
                    source = result.ExternalLoginInfo.LoginProvider;
                }
                e.Label = source;
                e.Value = "Account lockout";
                e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginFailMetricIndex.ToInvariantString(), "1"));

                AddGoogleAnalyticsEvent(e);
            }

            return Task.FromResult(0);
        }
    }
}
