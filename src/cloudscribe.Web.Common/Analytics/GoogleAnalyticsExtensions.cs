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

        public static void AddTransaction(this ITempDataDictionary tempData, Transaction transaction)
        {
            List<Transaction> list;
            if (tempData.ContainsKey(Transaction.TempDataKey))
            {
                string json = (string)tempData[Transaction.TempDataKey];
                list = JsonConvert.DeserializeObject<List<Transaction>>(json);
            }
            else
            {
                list = new List<Transaction>();
            }
            list.Add(transaction);

            tempData[Transaction.TempDataKey] = JsonConvert.SerializeObject(list);
        }

        public static List<Transaction> GetGoogleAnalyticsTransactions(this ITempDataDictionary tempData)
        {

            if (tempData.ContainsKey(Transaction.TempDataKey))
            {
                string json = (string)tempData[Transaction.TempDataKey];
                List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
                return list;
            }

            return new List<Transaction>();
        }




    }
}
