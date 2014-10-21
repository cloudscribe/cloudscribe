// Author:					Joe Audette
// Created:				    2014-10-13
// Last Modified:		    2014-10-17

using cloudscribe.Configuration;
using cloudscribe.Core.Web.Helpers;
using System.Collections.Generic;
using System.Web;

namespace cloudscribe.Core.Web.DeviceDetection
{
    

    public static class RequestExtensions
    {
        public static bool IsSmartPhone(this HttpContextBase context)
        {
        
            if((context.Request != null)&&(context.Request.UserAgent != null))
            {
                return IsSmartPhone(context.Request.UserAgent, context.Request.RawUrl);
            }

            return false;
        }

        private static bool IsSmartPhone(string userAgent, string rawUrl)
        {
            if (AppSettings.MobileDetectionExcludeUrlsCsv.Length > 0)
            {
                List<string> excludeUrls = AppSettings.MobileDetectionExcludeUrlsCsv.SplitOnCharAndTrim(',');
                foreach (string u in excludeUrls)
                {
                    if (rawUrl.Contains(u)) { return false; }
                }
            }

            string loweredBrowser = userAgent.ToLower();

            //http://googlewebmastercentral.blogspot.com/2012/11/giving-tablet-users-full-sized-web.html
            //https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11092~1#post46239
            // Android phones can be differentiated by Android; Mobile;
            if (loweredBrowser.Contains("android") && loweredBrowser.Contains("mobile")) { return true; }

            string mobileAgentsConcat = string.Empty;
            //string siteSpecificConfigKey;
            //SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            //if (siteSettings != null)
            //{
            //    siteSpecificConfigKey = "Site" + siteSettings.SiteId.ToInvariantString() + "-MobilePhoneUserAgents";
            //    if (ConfigurationManager.AppSettings[siteSpecificConfigKey] != null)
            //    {
            //        mobileAgentsConcat = ConfigurationManager.AppSettings[siteSpecificConfigKey];
            //    }
            //}

            // if no site specific setting found use the general web.config setting default value = iphone,android,iemobile
            // if any of these fragments is found in the user agent it is considered a match
            if (mobileAgentsConcat.Length == 0)
            {
                mobileAgentsConcat = AppSettings.MobilePhoneUserAgents;
            }

            List<string> mobileAgents = mobileAgentsConcat.SplitOnCharAndTrim(',');
            foreach (string agent in mobileAgents)
            {
                if (loweredBrowser.Contains(agent)) { return true; }
            }


            return false;
        }

        public static bool IsTablet(this HttpContextBase context)
        {
     
            if ((context.Request != null) && (context.Request.UserAgent != null))
            {
                return IsTablet(context.Request.UserAgent);
            }

            return false;
        }

        private static bool IsTablet(string userAgent)
        {
            string loweredBrowser = userAgent.ToLower();

            if (loweredBrowser.Contains("ipad")) { return true; }

            if (loweredBrowser.Contains("android")) 
            { 
                if(!IsSmartPhone(userAgent, string.Empty))
                {  
                    return true; 
                }
                
            }

            return false;
        }
    }
}
