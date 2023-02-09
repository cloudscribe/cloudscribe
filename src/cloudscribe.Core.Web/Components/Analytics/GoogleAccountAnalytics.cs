// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// Author:                  Joe Audette
// Created:                 2017-09-21
// Last Modified:           2022-02-08
//

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Web.Common.Analytics;
using cloudscribe.Web.Common.Analytics.GA4;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Analytics
{
    public partial class GoogleAccountAnalytics : IHandleAccountAnalytics
    {
        public GoogleAccountAnalytics(
            SiteContext currentSite,
            GoogleAnalyticsApiService analyticsApi,
            GoogleAnalyticsHelper analyticsHelper,
            GoogleAnalyticsGA4Helper analyticsGA4Helper,
            IOptions<GoogleAnalyticsOptions> optionsAccessor,
            IOptions<GoogleAnalyticsGA4Options> optionsGA4Accessor,
            IHttpContextAccessor contextAccessor
            )
        {
            _currentSite = currentSite;
            _analyticsApi = analyticsApi;
            _analyticsHelper = analyticsHelper;
            _analyticsGA4Helper = analyticsGA4Helper;
            _options = optionsAccessor.Value;
            _optionsGA4 = optionsGA4Accessor.Value;
            _contextAccessor = contextAccessor;
        }

        private SiteContext _currentSite;
        private GoogleAnalyticsApiService _analyticsApi;
        private GoogleAnalyticsHelper _analyticsHelper;
        private GoogleAnalyticsGA4Helper _analyticsGA4Helper;
        private GoogleAnalyticsOptions _options;
        private GoogleAnalyticsGA4Options _optionsGA4;
        private IHttpContextAccessor _contextAccessor;

        public async Task HandleLoginSubmit(string source)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(_currentSite.GoogleAnalyticsProfileId.StartsWith("G")) //switch to GA4
                {
                    await HandleGA4LoginSubmit(source);
                    return;
                }


                if(_options.TrackSocialLoginServerSide && source != "Onsite")
                {
                    var props = _analyticsApi.GetStandardProps(_contextAccessor.HttpContext);
                    var dimensionAndMetrics = new List<KeyValuePair<string, string>>();
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cm" + _options.LoginSubmitMetricIndex.ToInvariantString(), "1"));

                    await _analyticsApi.TrackEvent(
                        _currentSite.GoogleAnalyticsProfileId,
                        props.ClientId,
                        null,
                        props.Host,
                        _options.LoginRegisterEventCategory,
                        _options.LoginSubmitEventAction,
                        source,
                        null,
                        props.IpAddress,
                        props.UserAgent,
                        dimensionAndMetrics
                        );
                }
                else
                {
                    var e = new GoogleAnalyticsEvent();
                    e.Category = _options.LoginRegisterEventCategory;
                    e.Action = _options.LoginSubmitEventAction;
                    e.Label = source;
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                    e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginSubmitMetricIndex.ToInvariantString(), "1"));

                    _analyticsHelper.AddEvent(e);
                }

            }


        }

        public async Task HandleLoginFail(string source, string reason)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(_currentSite.GoogleAnalyticsProfileId.StartsWith("G")) //switch to GA4
                {
                    await HandleGA4LoginFail(source, reason);
                    return;
                }


                if (_options.TrackSocialLoginServerSide && source != "Onsite")
                {
                    var props = _analyticsApi.GetStandardProps(_contextAccessor.HttpContext);
                    var dimensionAndMetrics = new List<KeyValuePair<string, string>>();
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cm" + _options.LoginFailMetricIndex.ToInvariantString(), "1"));

                    await _analyticsApi.TrackEvent(
                        _currentSite.GoogleAnalyticsProfileId,
                        props.ClientId,
                        null,
                        props.Host,
                        _options.LoginRegisterEventCategory,
                        _options.LoginFailEventAction,
                        source,
                        reason,
                        props.IpAddress,
                        props.UserAgent,
                        dimensionAndMetrics
                        );
                }
                else
                {
                    var e = new GoogleAnalyticsEvent();
                    e.Category = _options.LoginRegisterEventCategory;
                    e.Action = _options.LoginFailEventAction;
                    e.Label = source;
                    e.Value = reason;
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                    e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginFailMetricIndex.ToInvariantString(), "1"));

                    _analyticsHelper.AddEvent(e);
                }


            }

            //return Task.FromResult(0);
        }

        public async Task HandleRegisterSubmit(string source)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(_currentSite.GoogleAnalyticsProfileId.StartsWith("G")) //switch to GA4
                {
                    await HandleGA4RegisterSubmit(source);
                    return;
                }


                if (_options.TrackSocialLoginServerSide && source != "Onsite")
                {
                    var props = _analyticsApi.GetStandardProps(_contextAccessor.HttpContext);
                    var dimensionAndMetrics = new List<KeyValuePair<string, string>>();
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cm" + _options.RegisterSubmitMetricIndex.ToInvariantString(), "1"));

                    await _analyticsApi.TrackEvent(
                        _currentSite.GoogleAnalyticsProfileId,
                        props.ClientId,
                        null,
                        props.Host,
                        _options.LoginRegisterEventCategory,
                        _options.RegisterSubmitEventAction,
                        source,
                        null,
                        props.IpAddress,
                        props.UserAgent,
                        dimensionAndMetrics
                        );
                }
                else
                {
                    var e = new GoogleAnalyticsEvent();
                    e.Category = _options.LoginRegisterEventCategory;
                    e.Action = _options.RegisterSubmitEventAction;
                    e.Label = source;
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                    e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.RegisterSubmitMetricIndex.ToInvariantString(), "1"));

                    _analyticsHelper.AddEvent(e);
                }


            }
        }

        public async Task HandleRegisterFail(string source, string reason)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(_currentSite.GoogleAnalyticsProfileId.StartsWith("G")) //switch to GA4
                {
                    await HandleGA4RegisterFail(source, reason);
                    return;
                }


                if (_options.TrackSocialLoginServerSide && source != "Onsite")
                {
                    var props = _analyticsApi.GetStandardProps(_contextAccessor.HttpContext);
                    var dimensionAndMetrics = new List<KeyValuePair<string, string>>();
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cm" + _options.RegisterFailMetricIndex.ToInvariantString(), "1"));

                    await _analyticsApi.TrackEvent(
                        _currentSite.GoogleAnalyticsProfileId,
                        props.ClientId,
                        null,
                        props.Host,
                        _options.LoginRegisterEventCategory,
                        _options.RegisterFailEventAction,
                        source,
                        reason,
                        props.IpAddress,
                        props.UserAgent,
                        dimensionAndMetrics
                        );
                }
                else
                {
                    var e = new GoogleAnalyticsEvent();
                    e.Category = _options.LoginRegisterEventCategory;
                    e.Action = _options.RegisterFailEventAction;
                    e.Label = source;
                    e.Value = reason;
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                    e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.RegisterFailMetricIndex.ToInvariantString(), "1"));

                    _analyticsHelper.AddEvent(e);
                }


            }
        }

        public async Task HandleLoginSuccess(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(_currentSite.GoogleAnalyticsProfileId.StartsWith("G")) //switch to GA4
                {
                    await HandleGA4LoginSuccess(result);
                    return;
                }

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

                if (_options.TrackSocialLoginServerSide && result.ExternalLoginInfo != null)
                {
                    var props = _analyticsApi.GetStandardProps(_contextAccessor.HttpContext);
                    var dimensionAndMetrics = new List<KeyValuePair<string, string>>();
                    if (_options.TrackUserId && result.User != null)
                    {
                        dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.UserIdDimensionIndex.ToInvariantString(), result.User.Id.ToString()));
                    }
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.RegisteredUserDimensionIndex.ToInvariantString(), "Yes"));
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cm" + _options.LoginSuccessMetricIndex.ToInvariantString(), "1"));

                    await _analyticsApi.TrackEvent(
                        _currentSite.GoogleAnalyticsProfileId,
                        props.ClientId,
                        null,
                        props.Host,
                        _options.LoginRegisterEventCategory,
                        _options.LoginSuccessEventAction,
                        source,
                        null,
                        props.IpAddress,
                        props.UserAgent,
                        dimensionAndMetrics
                        );
                }
                else
                {
                    var e = new GoogleAnalyticsEvent();
                    e.Category = _options.LoginRegisterEventCategory;
                    e.Action = _options.LoginSuccessEventAction;

                    e.Label = source;
                    if (_options.TrackUserId && result.User != null)
                    {
                        e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.UserIdDimensionIndex.ToInvariantString(), result.User.Id.ToString()));
                    }
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.RegisteredUserDimensionIndex.ToInvariantString(), "Yes"));
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                    e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginSuccessMetricIndex.ToInvariantString(), "1"));

                    _analyticsHelper.AddEvent(e);
                }


            }
        }

        public async Task HandleLoginNotAllowed(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(_currentSite.GoogleAnalyticsProfileId.StartsWith("G")) //switch to GA4
                {
                    await HandleGA4LoginNotAllowed(result);
                    return;
                }

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

                    if (_options.TrackSocialLoginServerSide && result.ExternalLoginInfo != null)
                    {
                        var props = _analyticsApi.GetStandardProps(_contextAccessor.HttpContext);
                        var dimensionAndMetrics = new List<KeyValuePair<string, string>>();
                        if (_options.TrackUserId && result.User != null)
                        {
                            dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.UserIdDimensionIndex.ToInvariantString(), result.User.Id.ToString()));
                        }
                        dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                        dimensionAndMetrics.Add(new KeyValuePair<string, string>("cm" + _options.LoginFailMetricIndex.ToInvariantString(), "1"));

                        await _analyticsApi.TrackEvent(
                            _currentSite.GoogleAnalyticsProfileId,
                            props.ClientId,
                            null,
                            props.Host,
                            _options.LoginRegisterEventCategory,
                            _options.LoginFailEventAction,
                            source,
                            reason,
                            props.IpAddress,
                            props.UserAgent,
                            dimensionAndMetrics
                            );
                    }
                    else
                    {
                        var e = new GoogleAnalyticsEvent();
                        e.Category = _options.LoginRegisterEventCategory;
                        e.Action = _options.LoginFailEventAction;
                        e.Label = source;
                        e.Value = reason;
                        if (_options.TrackUserId && result.User != null)
                        {
                            e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.UserIdDimensionIndex.ToInvariantString(), result.User.Id.ToString()));
                        }
                        e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                        e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginFailMetricIndex.ToInvariantString(), "1"));

                        _analyticsHelper.AddEvent(e);
                    }


                }
            }
        }

        private async Task HandleRegisterSuccess(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(_currentSite.GoogleAnalyticsProfileId.StartsWith("G")) //switch to GA4
                {
                    await HandleGA4RegisterSuccess(result);
                    return;
                }

                var source = "Onsite";
                if (result.ExternalLoginInfo != null)
                {
                    source = result.ExternalLoginInfo.LoginProvider;
                }

                if (_options.TrackSocialLoginServerSide && result.ExternalLoginInfo != null)
                {
                    var props = _analyticsApi.GetStandardProps(_contextAccessor.HttpContext);
                    var dimensionAndMetrics = new List<KeyValuePair<string, string>>();
                    if (_options.TrackUserId && result.User != null)
                    {
                        dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.UserIdDimensionIndex.ToInvariantString(), result.User.Id.ToString()));
                    }
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.RegisteredUserDimensionIndex.ToInvariantString(), "Yes"));
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cm" + _options.RegisterSuccessMetricIndex.ToInvariantString(), "1"));

                    await _analyticsApi.TrackEvent(
                        _currentSite.GoogleAnalyticsProfileId,
                        props.ClientId,
                        null,
                        props.Host,
                        _options.LoginRegisterEventCategory,
                        _options.RegisterSuccessEventAction,
                        source,
                        null,
                        props.IpAddress,
                        props.UserAgent,
                        dimensionAndMetrics
                        );
                }
                else
                {
                    var e = new GoogleAnalyticsEvent();
                    e.Category = _options.LoginRegisterEventCategory;
                    e.Action = _options.RegisterSuccessEventAction;
                    e.Label = source;
                    if (_options.TrackUserId && result.User != null)
                    {
                        e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.UserIdDimensionIndex.ToInvariantString(), result.User.Id.ToString()));
                    }
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.RegisteredUserDimensionIndex.ToInvariantString(), "Yes"));
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                    e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.RegisterSuccessMetricIndex.ToInvariantString(), "1"));

                    _analyticsHelper.AddEvent(e);
                }
            }
        }

        public Task HandleRequiresTwoFactor(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(_currentSite.GoogleAnalyticsProfileId.StartsWith("G")) //switch to GA4
                {
                    HandleGA4RequiresTwoFactor(result);
                    return Task.FromResult(0);
                }

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
                if (_options.TrackUserId && result.User != null)
                {
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.UserIdDimensionIndex.ToInvariantString(), result.User.Id.ToString()));
                }
                e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginFailMetricIndex.ToInvariantString(), "1"));

                _analyticsHelper.AddEvent(e);
            }

            return Task.FromResult(0);
        }

        public Task HandleLockout(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(_currentSite.GoogleAnalyticsProfileId.StartsWith("G")) //switch to GA4
                {
                    HandleGA4Lockout(result);
                    return Task.FromResult(0);
                }

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
                if (_options.TrackUserId && result.User != null)
                {
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.UserIdDimensionIndex.ToInvariantString(), result.User.Id.ToString()));
                }
                e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimensionIndex.ToInvariantString(), source));
                e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginFailMetricIndex.ToInvariantString(), "1"));

                _analyticsHelper.AddEvent(e);
            }

            return Task.FromResult(0);
        }

        public Task HandleLogout(string reason)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(_currentSite.GoogleAnalyticsProfileId.StartsWith("G")) //switch to GA4
                {
                    HandleGA4Logout(reason);
                    return Task.FromResult(0);
                }

                //Logout was never implemented in the original Google Analytics module


            }
            return Task.FromResult(0);
        }

        public Task HandleSearch(string searchQuery, int numResults)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(_currentSite.GoogleAnalyticsProfileId.StartsWith("G")) //switch to GA4
                {
                    HandleGA4Search(searchQuery, numResults);
                    return Task.FromResult(0);
                }

                //Search was never implemented in the original Google Analytics module


            }
            return Task.FromResult(0);
        }

        /// <summary>
        /// This is a generic event handler for any custom event you want to track
        /// </summary>
        public Task HandleEvent(string eventName, List<KeyValuePair<string,string>> parameters)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(_currentSite.GoogleAnalyticsProfileId.StartsWith("G")) //switch to GA4
                {
                    HandleGA4Event(eventName, parameters);
                    return Task.FromResult(0);
                }

                //This was never implemented in the original Google Analytics module
            }
            return Task.FromResult(0);
        }
    }
}
