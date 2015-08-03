// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2006-02-03
// Last Modified:		    2015-06-23


using System;
using System.IO;
using System.Collections.Generic;
using cloudscribe.Core.Models;
using cloudscribe.Configuration;


namespace cloudscribe.Setup
{
    public class SetupHelper
    {

        public static int CompareFileNames(FileInfo f1, FileInfo f2)
        {
            return f1.FullName.CompareTo(f2.FullName);
        }

        public static Version ParseVersionFromFileName(String fileName)
        {
            Version version = null;

            int major = 0;
            int minor = 0;
            int build = 0;
            int revision = 0;
            bool success = true;


            if (fileName != null)
            {
                char[] separator = { '.' };
                string[] args = fileName.Replace(".config", String.Empty).Split(separator);
                if (args.Length >= 4)
                {
                    if (!(int.TryParse(args[0], out major)))
                    {
                        major = 0;
                        success = false;
                    }

                    if (!(int.TryParse(args[1], out minor)))
                    {
                        minor = 0;
                        success = false;
                    }

                    if (!(int.TryParse(args[2], out build)))
                    {
                        build = 0;
                        success = false;
                    }

                    if (!(int.TryParse(args[3], out revision)))
                    {
                        revision = 0;
                        success = false;
                    }

                    if (success)
                    {
                        version = new Version(major, minor, build, revision);
                    }

                }

            }


            return version;

        }

        

        public static bool NeedsUpgrade(
            string applicationName, 
            IDb db)
        {
            IVersionProvider provider = db.VersionProviders.Get(applicationName);
            //if (VersionProviderManager.Providers[applicationName] == null) { return true; }
            if(provider == null) { return true; }

            Version codeVersion = provider.GetCodeVersion();

            Guid appId = db.GetOrGenerateSchemaApplicationId(applicationName);
            Version schemaVersion = db.GetSchemaVersion(appId);

            bool result = false;
            if (codeVersion > schemaVersion) { result = true; }

            return result;
        }

        


        //public static string BuildHtmlErrorPage(Exception ex)
        //{
        //    String errorHtml = "<html><head><title>Error</title>"
        //        + "<link id='Link1' rel='stylesheet' href='" + "setup.css' type='text/css' /></head>"
        //        + "<body><div class='settingrow'><label class='settinglabel' >An Error Occurred:</label>"
        //        + ex.Message + "</div>"
        //        + "<div class='settingrow'><label class='settinglabel' >Source:</label>" + ex.Source + "</div>"
        //        + "<div class='settingrow'><label class='settinglabel' >Stack Trace</label>" + ex.StackTrace + "</div>"
        //        + "</body></html>";

        //    return errorHtml;

        //}


    }
}
