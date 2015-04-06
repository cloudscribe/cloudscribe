// Author:					Joe Audette
// Created:				    2015-04-06
// Last Modified:		    2015-04-06
// 

using cloudscribe.Core.Models.Site;
using log4net;
using System;
using System.Web.Hosting;
using System.Xml;

namespace cloudscribe.Core.Web.Components
{
    public class DoNothingStartupTrigger : ITriggerStartup
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebConfigStartupTrigger));
        /// <summary>
        /// doesn't do anything. This implementation can be plugged in from dependency injection
        /// for scenarios that don't actually need to trigger retsart of the app such as unit tests
        /// also, possibly we can eliminate the need for restarting if we can solve the new site problem
        /// ie configuration per site happens as app startup but if a new site is created currently we need to run that code again
        /// </summary>
        /// <returns></returns>
        public bool TriggerStartup()
        {
            return false;   
        }
    }
}
