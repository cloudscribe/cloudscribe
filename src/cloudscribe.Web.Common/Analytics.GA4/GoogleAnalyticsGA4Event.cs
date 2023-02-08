// Copyright (c) Idox Software Ltd All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Simon Annetts/ESDM
// Created:					2022-02-07
// Last Modified:			2022-02-07
//

using System.Collections.Generic;

namespace cloudscribe.Web.Common.Analytics.GA4
{
    public class GoogleAnalyticsGA4Event
    {
        public GoogleAnalyticsGA4Event()
        {
            Parameters = new List<KeyValuePair<string, string>>();
        }
        public string Name { get; set; } //the Event Name

        public List<KeyValuePair<string, string>> Parameters { get; set; } //(optional) Event Parameters

        public const string TempDataKey = "GoogleAnalyticsGA4Events";

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}