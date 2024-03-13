// Copyright (c) Idox Software Ltd All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Simon Annetts/ESDM
// Created:					2022-02-07
// Last Modified:			2022-02-07
//

namespace cloudscribe.Web.Common.Analytics.GA4
{
    public class GoogleAnalyticsGA4Options
    {
        /// <summary>
        /// you can set this as false if your privacy policy does not allow it.
        /// </summary>
        public bool TrackUserId { get; set; } = true;

        /// <summary>
        /// Enable this to see events in realtime within the Google Analytics UI
        /// https://support.google.com/analytics/answer/7201382#zippy=%2Cgoogle-tag-websites
        /// </summary>
        public bool EnableDebugMode { get; set; } = false;



        // https://developers.google.com/analytics/devguides/collection/ga4/reference/events?client_type=gtag#login
        public string LoginSuccessEventName { get; set; } = "login";            //GA4 recommended event name
        public string LoginFailEventName { get; set; } = "login_fail";          //GA4 custom event name - needs to be defined in GA4
        public string LoginSubmitEventName { get; set; } = "login_submit";      //GA4 custom event name - needs to be defined in GA4
        public string LoginRequires2FaEventName { get; set; } = "login_2fa";    //GA4 custom event name - needs to be defined in GA4
        public string LoginLockoutEventName { get; set; } = "login_lockout";    //GA4 custom event name - needs to be defined in GA4

        // https://developers.google.com/analytics/devguides/collection/ga4/reference/events?client_type=gtag#sign_up
        public string RegisterSuccessEventName { get; set; } = "sign_up";       //GA4 recommended event name
        public string RegisterFailEventName { get; set; } = "sign_up_fail";     //GA4 custom event name - needs to be defined in GA4
        public string RegisterSubmitEventName { get; set; } = "sign_up_submit"; //GA4 custom event name - needs to be defined in GA4

        public string LogoutEventName { get; set; } = "logout";                 //GA4 custom event name - needs to be defined in GA4

        //https://developers.google.com/analytics/devguides/collection/ga4/reference/events?client_type=gtag#search
        public string SearchEventName { get; set; } = "search";                 //GA4 recommended event name
    }
}