using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace cloudscribe.Web.Common.Analytics
{
    public static class GoogleAnalyticsExtensions
    {

        public static void AddEvent(this ITempDataDictionary tempData, GoogleAnalyticsEvent analyticsEvent)
        {
            List<GoogleAnalyticsEvent> list;
            if (tempData.ContainsKey(GoogleAnalyticsEvent.TempDataKey))
            {
                string json = (string)tempData[GoogleAnalyticsEvent.TempDataKey];
                list = JsonConvert.DeserializeObject<List<GoogleAnalyticsEvent>>(json);
            }
            else
            {
                list = new List<GoogleAnalyticsEvent>();
            }
            list.Add(analyticsEvent);
            
            tempData[GoogleAnalyticsEvent.TempDataKey] = JsonConvert.SerializeObject(list);
        }

        public static List<GoogleAnalyticsEvent> GetGoogleAnalyticsEvents(this ITempDataDictionary tempData)
        {

            if (tempData.ContainsKey(GoogleAnalyticsEvent.TempDataKey))
            {
                string json = (string)tempData[GoogleAnalyticsEvent.TempDataKey];
                List<GoogleAnalyticsEvent> list = JsonConvert.DeserializeObject<List<GoogleAnalyticsEvent>>(json);
                return list;
            }

            return new List<GoogleAnalyticsEvent>();
        }


    }
}
