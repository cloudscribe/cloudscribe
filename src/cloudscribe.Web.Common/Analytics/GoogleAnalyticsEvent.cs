using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Web.Common.Analytics
{
    public class GoogleAnalyticsEvent
    {
        public GoogleAnalyticsEvent()
        {
            Fields = new List<KeyValuePair<string, string>>();
        }
        public string Category { get; set; }
        public string Action { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }

        public List<KeyValuePair<string, string>> Fields { get; set; }

        public const string TempDataKey = "GoogleAnalyticsEvents";

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Category) && !string.IsNullOrWhiteSpace(Action);
        }
    }
}
