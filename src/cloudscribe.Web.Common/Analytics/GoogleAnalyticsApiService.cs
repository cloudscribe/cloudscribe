// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// Author:                  Joe Audette
// Created:                 2017-09-21
// Last Modified:           2017-09-21
// 

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            ILogger<GoogleAnalyticsApiService> logger
            )
        {
            _log = logger;
        }

        private ILogger _log;
        private const string version = "1";
        private const string gaUrl = "http://www.google-analytics.com/collect";

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

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(
                        gaUrl,
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
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "error posting to google analytics", null);
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
            string dimension1 = "",
            string dimension2 = "",
            string dimension3 = "",
            string dimension4 = "",
            string dimension5 = "",
            string metric1 = "",
            string metric2 = "",
            string metric3 = "",
            string metric4 = "",
            string metric5 = ""

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

            if (!string.IsNullOrWhiteSpace(dimension1))
            {
                keyValues.Add(new KeyValuePair<string, string>("cd1", dimension1));
            }

            if (!string.IsNullOrWhiteSpace(dimension2))
            {
                keyValues.Add(new KeyValuePair<string, string>("cd2", dimension2));
            }

            if (!string.IsNullOrWhiteSpace(dimension3))
            {
                keyValues.Add(new KeyValuePair<string, string>("cd3", dimension3));
            }

            if (!string.IsNullOrWhiteSpace(dimension4))
            {
                keyValues.Add(new KeyValuePair<string, string>("cd4", dimension4));
            }

            if (!string.IsNullOrWhiteSpace(dimension5))
            {
                keyValues.Add(new KeyValuePair<string, string>("cd5", dimension5));
            }

            if (!string.IsNullOrWhiteSpace(metric1))
            {
                keyValues.Add(new KeyValuePair<string, string>("cm1", metric1));
            }

            if (!string.IsNullOrWhiteSpace(metric2))
            {
                keyValues.Add(new KeyValuePair<string, string>("cm2", metric2));
            }

            if (!string.IsNullOrWhiteSpace(metric3))
            {
                keyValues.Add(new KeyValuePair<string, string>("cm3", metric3));
            }

            if (!string.IsNullOrWhiteSpace(metric4))
            {
                keyValues.Add(new KeyValuePair<string, string>("cm4", metric4));
            }

            if (!string.IsNullOrWhiteSpace(metric5))
            {
                keyValues.Add(new KeyValuePair<string, string>("cm5", metric5));
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

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(
                        gaUrl,
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
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "error posting to google analytics", null);
            }


        }



    }

}
