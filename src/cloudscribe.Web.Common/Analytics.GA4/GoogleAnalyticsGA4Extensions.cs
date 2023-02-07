// Copyright (c) Idox Ltd All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Simon Annetts/ESDM
// Created:					2022-02-07
// Last Modified:			2022-02-07
// 

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace cloudscribe.Web.Common.Analytics.GA4
{
    public static class GoogleAnalyticsGA4Extensions
    {

        public static void AddEvent(this ITempDataDictionary tempData, GoogleAnalyticsGA4Event analyticsEvent)
        {
            List<GoogleAnalyticsGA4Event> list;
            if (tempData.ContainsKey(GoogleAnalyticsGA4Event.TempDataKey))
            {
                string json = (string)tempData[GoogleAnalyticsGA4Event.TempDataKey];
                list = JsonConvert.DeserializeObject<List<GoogleAnalyticsGA4Event>>(json);
            }
            else
            {
                list = new List<GoogleAnalyticsGA4Event>();
            }
            list.Add(analyticsEvent);
            
            tempData[GoogleAnalyticsGA4Event.TempDataKey] = JsonConvert.SerializeObject(list);
        }

        public static List<GoogleAnalyticsGA4Event> GetGoogleAnalyticsGA4Events(this ITempDataDictionary tempData)
        {

            if (tempData.ContainsKey(GoogleAnalyticsGA4Event.TempDataKey))
            {
                string json = (string)tempData[GoogleAnalyticsGA4Event.TempDataKey];
                List<GoogleAnalyticsGA4Event> list = JsonConvert.DeserializeObject<List<GoogleAnalyticsGA4Event>>(json);
                return list;
            }

            return new List<GoogleAnalyticsGA4Event>();
        }

        // public static void AddTransaction(this ITempDataDictionary tempData, Transaction transaction)
        // {
        //     List<Transaction> list;
        //     if (tempData.ContainsKey(Transaction.TempDataKey))
        //     {
        //         string json = (string)tempData[Transaction.TempDataKey];
        //         list = JsonConvert.DeserializeObject<List<Transaction>>(json);
        //     }
        //     else
        //     {
        //         list = new List<Transaction>();
        //     }
        //     list.Add(transaction);

        //     tempData[Transaction.TempDataKey] = JsonConvert.SerializeObject(list);
        // }

        // public static List<Transaction> GetGoogleAnalyticsTransactions(this ITempDataDictionary tempData)
        // {

        //     if (tempData.ContainsKey(Transaction.TempDataKey))
        //     {
        //         string json = (string)tempData[Transaction.TempDataKey];
        //         List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
        //         return list;
        //     }

        //     return new List<Transaction>();
        // }

    }
}
