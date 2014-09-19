using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

        public static bool RunningInFullTrust()
        {
            bool result = false;

            AspNetHostingPermissionLevel currentTrustLevel = GetCurrentTrustLevel();

            if (currentTrustLevel == AspNetHostingPermissionLevel.Unrestricted) { result = true; }

            return result;

        }

        public static AspNetHostingPermissionLevel GetCurrentTrustLevel()
        {
            foreach (AspNetHostingPermissionLevel trustLevel in
                    new AspNetHostingPermissionLevel[] {
                AspNetHostingPermissionLevel.Unrestricted,
                AspNetHostingPermissionLevel.High,
                AspNetHostingPermissionLevel.Medium,
                AspNetHostingPermissionLevel.Low,
                AspNetHostingPermissionLevel.Minimal 
            })
            {
                try
                {
                    new AspNetHostingPermission(trustLevel).Demand();
                }
                catch (System.Security.SecurityException)
                {
                    continue;
                }

                return trustLevel;
            }

            return AspNetHostingPermissionLevel.None;
        }


        public static string BuildHtmlErrorPage(Exception ex)
        {
            String errorHtml = "<html><head><title>Error</title>"
                + "<link id='Link1' rel='stylesheet' href='" + "setup.css' type='text/css' /></head>"
                + "<body><div class='settingrow'><label class='settinglabel' >An Error Occurred:</label>"
                + ex.Message + "</div>"
                + "<div class='settingrow'><label class='settinglabel' >Source:</label>" + ex.Source + "</div>"
                + "<div class='settingrow'><label class='settinglabel' >Stack Trace</label>" + ex.StackTrace + "</div>"
                + "</body></html>";

            return errorHtml;

        }


    }
}
