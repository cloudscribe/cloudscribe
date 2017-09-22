// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// Author:                  Joe Audette
// Created:                 2017-09-21
// Last Modified:           2017-09-22
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Web.Common.Analytics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Analytics
{
    public class GoogleAccountAnalytics : IHandleAccountAnalytics
    {
        public GoogleAccountAnalytics(
            SiteContext currentSite,
            GoogleAnalyticsApiService analyticsApi,
            IOptions<GoogleAnalyticsOptions> optionsAccessor,
            ITempDataDictionaryFactory tempaDataFactory,
            IHttpContextAccessor contextAccessor
            )
        {
            _currentSite = currentSite;
            _analyticsApi = analyticsApi;
            _options = optionsAccessor.Value;
            _tempaDataFactory = tempaDataFactory;
            _contextAccessor = contextAccessor;
        }

        private SiteContext _currentSite;
        private GoogleAnalyticsApiService _analyticsApi;
        private GoogleAnalyticsOptions _options;
        private IHttpContextAccessor _contextAccessor;
        private ITempDataDictionaryFactory _tempaDataFactory;
        private ITempDataDictionary _tempData = null;

        internal class StandardPostProps
        {
            public string ClientId { get; set; }
            public string IpAddress { get; set; }
            public string UserAgent { get; set; }
            public string Host { get; set; }
        }

        private StandardPostProps GetStandardProps()
        {
            var props = new StandardPostProps();
            var context = _contextAccessor.HttpContext;
            props.ClientId = _analyticsApi.GetGAClientIdFromCookie(context);
            props.Host = context.Request.Host.Value;
            props.IpAddress = context.Connection.RemoteIpAddress.ToString();
            props.UserAgent = context.Request.Headers["User-Agent"].ToString();
            if (!string.IsNullOrWhiteSpace(props.UserAgent))
            {
                props.UserAgent = WebUtility.UrlEncode(props.UserAgent);
            }

            return props;
        }


        private ITempDataDictionary GetTempData()
        {
            if(_tempData == null)
            {
                _tempData = _tempaDataFactory.GetTempData(_contextAccessor.HttpContext);
            }
            
            return _tempData;
        }

        private void AddGoogleAnalyticsEvent(GoogleAnalyticsEvent ev)
        {
            //these are detected by the google analytics taghelper
            var tempData = GetTempData();
            if (tempData != null)
            {
                tempData.AddEvent(ev);
            }
        }

        public async Task HandleLoginSubmit(string source)
        {
            if(!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(_options.TrackSocialLoginServerSide && source != "Onsite")
                {
                    var props = GetStandardProps();
                    var dimensionAndMetrics = new List<KeyValuePair<string, string>>();
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
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
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                    e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginSubmitMetricIndex.ToInvariantString(), "1"));

                    AddGoogleAnalyticsEvent(e);
                }
                
            }

            
        }

        public async Task HandleLoginFail(string source, string reason)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if (_options.TrackSocialLoginServerSide && source != "Onsite")
                {
                    var props = GetStandardProps();
                    var dimensionAndMetrics = new List<KeyValuePair<string, string>>();
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
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
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                    e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginFailMetricIndex.ToInvariantString(), "1"));

                    AddGoogleAnalyticsEvent(e);
                }

                
            }

            //return Task.FromResult(0);
        }

        public async Task HandleRegisterSubmit(string source)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if (_options.TrackSocialLoginServerSide && source != "Onsite")
                {
                    var props = GetStandardProps();
                    var dimensionAndMetrics = new List<KeyValuePair<string, string>>();
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
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
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                    e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.RegisterSubmitMetricIndex.ToInvariantString(), "1"));

                    AddGoogleAnalyticsEvent(e);
                }

                
            }

            //return Task.FromResult(0);
        }

        public async Task HandleRegisterFail(string source, string reason)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if (_options.TrackSocialLoginServerSide && source != "Onsite")
                {
                    var props = GetStandardProps();
                    var dimensionAndMetrics = new List<KeyValuePair<string, string>>();
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
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
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                    e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.RegisterFailMetricIndex.ToInvariantString(), "1"));

                    AddGoogleAnalyticsEvent(e);
                }

                
            }

            //return Task.FromResult(0);
        }

        public async Task HandleLoginSuccess(UserLoginResult result)
        {
            if (!string.IsNullOrEmpty(_currentSite.GoogleAnalyticsProfileId))
            {
                if(result.IsNewUserRegistration)
                {
                    await HandleRegisterSuccess(result);
                    //return Task.FromResult(0);
                    return;
                }

                var source = "Onsite";
                if (result.ExternalLoginInfo != null)
                {
                    source = result.ExternalLoginInfo.LoginProvider;
                }

                if (_options.TrackSocialLoginServerSide && result.ExternalLoginInfo != null)
                {
                    var props = GetStandardProps();
                    var dimensionAndMetrics = new List<KeyValuePair<string, string>>();
                    if (result.User != null)
                    {
                        dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.UserIdDimensionIndex.ToInvariantString(), result.User.Id.ToString()));
                    }
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.RegisteredUserDimensionIdex.ToInvariantString(), "Yes"));
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
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
                    if (result.User != null)
                    {
                        e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.UserIdDimensionIndex.ToInvariantString(), result.User.Id.ToString()));
                    }
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.RegisteredUserDimensionIdex.ToInvariantString(), "Yes"));
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                    e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginSuccessMetricIndex.ToInvariantString(), "1"));

                    AddGoogleAnalyticsEvent(e);
                }

                
            }

            //return Task.FromResult(0);
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

                    if (_options.TrackSocialLoginServerSide && result.ExternalLoginInfo != null)
                    {
                        var props = GetStandardProps();
                        var dimensionAndMetrics = new List<KeyValuePair<string, string>>();
                        dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
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
                        e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                        e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.LoginFailMetricIndex.ToInvariantString(), "1"));

                        AddGoogleAnalyticsEvent(e);
                    }
                    

                }
            }

           // return Task.FromResult(0);
        }

        private async Task HandleRegisterSuccess(UserLoginResult result)
        {
            var source = "Onsite";
            if (result.ExternalLoginInfo != null)
            {
                source = result.ExternalLoginInfo.LoginProvider;
            }

            if (_options.TrackSocialLoginServerSide && result.ExternalLoginInfo != null)
            {
                var props = GetStandardProps();
                var dimensionAndMetrics = new List<KeyValuePair<string, string>>();
                if (result.User != null)
                {
                    dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.UserIdDimensionIndex.ToInvariantString(), result.User.Id.ToString()));
                }
                dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.RegisteredUserDimensionIdex.ToInvariantString(), "Yes"));
                dimensionAndMetrics.Add(new KeyValuePair<string, string>("cd" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
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
                if (result.User != null)
                {
                    e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.UserIdDimensionIndex.ToInvariantString(), result.User.Id.ToString()));
                }
                e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.RegisteredUserDimensionIdex.ToInvariantString(), "Yes"));
                e.Fields.Add(new KeyValuePair<string, string>("dimension" + _options.LoginRegisterSourceDimenstionIdex.ToInvariantString(), source));
                e.Fields.Add(new KeyValuePair<string, string>("metric" + _options.RegisterSuccessMetricIndex.ToInvariantString(), "1"));

                AddGoogleAnalyticsEvent(e);
            }

            
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
