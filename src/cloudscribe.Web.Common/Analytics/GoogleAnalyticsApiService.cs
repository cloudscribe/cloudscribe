// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// Author:                  Joe Audette
// Created:                 2017-09-21
// Last Modified:           2019-02-20
// 

//using cloudscribe.Web.Common.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

//https://developers.google.com/analytics/devguides/collection/protocol/v1/
//https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide
//https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters

namespace cloudscribe.Web.Common.Analytics
{
    public class GoogleAnalyticsApiService
    {
        public GoogleAnalyticsApiService(
            IHttpClientFactory httpClientFactory,
            ILogger<GoogleAnalyticsApiService> logger
            )
        {
            _httpClientFactory = httpClientFactory;
            _log = logger;
        }

        private IHttpClientFactory _httpClientFactory;
        private ILogger _log;
        private const string version = "1";
        private const string gaUrl = "https://www.google-analytics.com/";
        private const string httpClientName = "google-analytics";

        //https://stackoverflow.com/questions/14227331/what-is-the-client-id-when-sending-tracking-data-to-google-analytics-via-the-mea

        public string GetGAClientIdFromCookie(HttpContext context)
        {
            var gaCookie = context.Request.Cookies["_ga"];
            if (!string.IsNullOrWhiteSpace(gaCookie))
            {
                var gaSplit = gaCookie.Split('.');
                if (gaSplit.Length >= 4)
                {
                    return gaSplit[2] + "." + gaSplit[3];
                }
            }

            return null;
        }

        public StandardPostProps GetStandardProps(HttpContext context)
        {
            var props = new StandardPostProps();
            
            props.ClientId = GetGAClientIdFromCookie(context);
            props.Host = context.Request.Host.Value;
            props.IpAddress = context.Connection.RemoteIpAddress.ToString();
            props.UserAgent = context.Request.Headers["User-Agent"].ToString();
            if (!string.IsNullOrWhiteSpace(props.UserAgent))
            {
                props.UserAgent = WebUtility.UrlEncode(props.UserAgent);
            }

            return props;
        }


        public async Task TrackPageView(
            string profileId,
            string clientId,
            string userId,
            string hostName,
            string path,
            string title,
            string ipAddress = "",
            string userAgent = "",
            string campaignSource = "",
            string campaignMedium = "",
            string campaignName = "",
            string campaignTerm = "",
            string campaignContent = ""

            )
        {
            if (string.IsNullOrWhiteSpace(profileId))
            {
                _log.LogWarning("ignoring call to TrackPageView because profileid not provided");
                return;
            }

            if (string.IsNullOrWhiteSpace(hostName))
            {
                _log.LogWarning("ignoring call to TrackPageView because hostName not provided");
                return;
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                _log.LogWarning("ignoring call to TrackPageView because path not provided");
                return;
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                _log.LogWarning("ignoring call to TrackPageView because title not provided");
                return;
            }

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("v", version));
            keyValues.Add(new KeyValuePair<string, string>("tid", profileId));

            if (!string.IsNullOrWhiteSpace(userId))
            {
                keyValues.Add(new KeyValuePair<string, string>("uid", userId));
            }
            else if (!string.IsNullOrWhiteSpace(clientId))
            {
                keyValues.Add(new KeyValuePair<string, string>("cid", clientId));
            }
            else
            {
                keyValues.Add(new KeyValuePair<string, string>("uid", Guid.NewGuid().ToString()));
            }

            keyValues.Add(new KeyValuePair<string, string>("t", "pageview"));
            keyValues.Add(new KeyValuePair<string, string>("dh", hostName));
            keyValues.Add(new KeyValuePair<string, string>("dp", path));
            keyValues.Add(new KeyValuePair<string, string>("dt", title));

            if (!string.IsNullOrWhiteSpace(campaignSource))
            {
                keyValues.Add(new KeyValuePair<string, string>("cs", campaignSource));
            }

            if (!string.IsNullOrWhiteSpace(campaignMedium))
            {
                keyValues.Add(new KeyValuePair<string, string>("cm", campaignMedium));
            }

            if (!string.IsNullOrWhiteSpace(campaignName))
            {
                keyValues.Add(new KeyValuePair<string, string>("cn", campaignName));
            }

            if (!string.IsNullOrWhiteSpace(campaignTerm))
            {
                keyValues.Add(new KeyValuePair<string, string>("ck", campaignTerm));
            }

            if (!string.IsNullOrWhiteSpace(campaignContent))
            {
                keyValues.Add(new KeyValuePair<string, string>("cc", campaignContent));
            }

            if (!string.IsNullOrWhiteSpace(userAgent))
            {
                keyValues.Add(new KeyValuePair<string, string>("ua", userAgent));
            }

            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                keyValues.Add(new KeyValuePair<string, string>("uip", ipAddress));
            }

            var content = new FormUrlEncodedContent(keyValues);

            var client = _httpClientFactory.CreateClient(httpClientName);

            try
            {
                var response = await client.PostAsync(
                    "collect",
                    content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    _log.LogDebug($"success posting pageview to google analytics");
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var logmessage = $"failed to send pageview data to google analytics, response was: { responseBody }";
                    _log.LogWarning(logmessage);
                }
  
            }
            catch (Exception ex)
            {
                _log.LogError("error posting to google analytics: " + ex.Message + " stacktrace: " + ex.StackTrace);
            }


        }

        public async Task TrackEvent(
            string profileId,
            string clientId,
            string userId,
            string hostName,
            string eventCategory,
            string eventAction,
            string eventLabel = "",
            string eventValue = "",
            string ipAddress = "",
            string userAgent = "",
            List<KeyValuePair<string, string>> dimensionsAndMetrics = null

            )
        {
            if (string.IsNullOrWhiteSpace(profileId))
            {
                _log.LogWarning("ignoring call to TrackEvent because profileid not provided");
                return;
            }
            
            if (string.IsNullOrWhiteSpace(eventCategory))
            {
                _log.LogWarning("ignoring call to TrackEvent because eventCategory not provided");
                return;
            }

            if (string.IsNullOrWhiteSpace(eventAction))
            {
                _log.LogWarning("ignoring call to TrackEvent because eventAction not provided");
                return;
            }

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("v", version));
            keyValues.Add(new KeyValuePair<string, string>("tid", profileId));

            if (!string.IsNullOrWhiteSpace(userId))
            {
                keyValues.Add(new KeyValuePair<string, string>("uid", userId));
            }
            else if (!string.IsNullOrWhiteSpace(clientId))
            {
                keyValues.Add(new KeyValuePair<string, string>("cid", clientId));
            }
            else
            {
                keyValues.Add(new KeyValuePair<string, string>("uid", Guid.NewGuid().ToString()));
            }

            keyValues.Add(new KeyValuePair<string, string>("t", "event"));

            if (!string.IsNullOrWhiteSpace(hostName))
            {
                keyValues.Add(new KeyValuePair<string, string>("dh", hostName));
            }
            if (!string.IsNullOrWhiteSpace(userAgent))
            {
                keyValues.Add(new KeyValuePair<string, string>("ua", userAgent));
            }

            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                keyValues.Add(new KeyValuePair<string, string>("uip", ipAddress));
            }

            // these 2 required
            keyValues.Add(new KeyValuePair<string, string>("ec", eventCategory));
            keyValues.Add(new KeyValuePair<string, string>("ea", eventAction));

            if (!string.IsNullOrWhiteSpace(eventLabel))
            {
                keyValues.Add(new KeyValuePair<string, string>("el", eventLabel));
            }

            if (!string.IsNullOrWhiteSpace(eventValue))
            {
                keyValues.Add(new KeyValuePair<string, string>("ev", eventValue));
            }

            if(dimensionsAndMetrics != null)
            {
                foreach(var prop in dimensionsAndMetrics)
                {
                    keyValues.Add(new KeyValuePair<string, string>(prop.Key, prop.Value));
                }
            }
            
            var content = new FormUrlEncodedContent(keyValues);
            var client = _httpClientFactory.CreateClient(httpClientName);

            try
            {
                var response = await client.PostAsync(
                    "collect",
                    content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    _log.LogDebug($"success posting event to google analytics");
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var logmessage = $"failed to send event data to google analytics, response was: { responseBody }";
                    _log.LogWarning(logmessage);
                }
                
            }
            catch (Exception ex)
            {
                _log.LogError("error posting to google analytics: " + ex.Message + " stacktrace: " + ex.StackTrace);
            }


        }



    }

}
