// Author:					Joe Audette
// Created:				    2015-02-10
// Last Modified:		    2015-02-10
// 

using cloudscribe.Core.Models.Site;
using log4net;
using System;
using System.Web.Hosting;
using System.Xml;

namespace cloudscribe.Core.Web.Components
{
    public class WebConfigStartupTrigger : ITriggerStartup
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebConfigStartupTrigger));
        /// <summary>
        /// attempts to open and save the web.config file in order to trigger
        /// application startup code Startup.cs in the root and other code in App_Start
        /// for example when a new folder site is created from the ui, 
        /// we need to setup authentication and routes for the new site
        /// 
        /// returns true if method completed with no errors
        /// 
        /// known issues and limitations
        /// 
        /// this implementation depends on the web process having permission to modify the web.config file
        /// which is not recommended but usually is the case therefore it works in most cases
        /// 
        /// in a web farm environment this would only restart a single node
        /// 
        /// if someone implements a better way to trigger startup (another implementation of ITriggerStartup), 
        /// this implementation can be easily replaced by dependency injection
        /// </summary>
        /// <returns></returns>
        public bool TriggerStartup()
        {
            try
            {
                string webConfigPath = HostingEnvironment.MapPath("~/Web.config");
                var xmlConfig = new XmlDocument();
                xmlConfig.Load(webConfigPath);
                xmlConfig.Save(webConfigPath);

                //var writer = new XmlTextWriter(webConfigPath, null) { Formatting = Formatting.Indented };
                //xmlConfig.WriteTo(writer);
                //writer.Flush();
                //writer.Close();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return false;

            // the below solution would have been preferrable but did not result in
            // re-execution of Startup.cs which is needed to setup new folder sites

            // creating a folder below the app root causes a recycle/restart
            //DirectoryInfo dir = new DirectoryInfo(HostingEnvironment.MapPath("~/restart"));
            //if (dir.Exists)
            //{
            //    Directory.Move(dir.FullName, dir.FullName + "ed");
            //}
            //else
            //{
            //    DirectoryInfo dired = new DirectoryInfo(HostingEnvironment.MapPath("~/restarted"));
            //    if (dired.Exists)
            //    {
            //        Directory.Move(dired.FullName, dir.FullName);
            //    }
            //    else
            //    {
            //        Directory.CreateDirectory(dir.FullName);
            //    }
            //}
        }

    }
}
