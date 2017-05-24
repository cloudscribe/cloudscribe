// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-09
// Last Modified:			2016-10-08
// 

using System.Collections.Generic;

namespace cloudscribe.Core.Models
{
    public static class ModelExtensions
    {

        public static bool IsDeletable(this ISiteRole role, string undeletableRolesSemiColonSeparated)
        {
            List<string> rolesThatCannotBeDelete = undeletableRolesSemiColonSeparated.SplitOnChar(';');
            return role.IsDeletable(rolesThatCannotBeDelete);
        }

        public static bool IsDeletable(this ISiteRole role, List<string> rolesThatCannotBeDeleted)
        {
            if (role.NormalizedRoleName == "Admins") { return false; }
            if (role.NormalizedRoleName == "Content Administrators") { return false; }
            if (role.NormalizedRoleName == "Authenticated Users") { return false; }
            if (role.NormalizedRoleName == "Role Admins") { return false; }

            if (rolesThatCannotBeDeleted != null)
            {
                foreach (string roleName in rolesThatCannotBeDeleted)
                {
                    if (role.NormalizedRoleName == roleName) { return false; }
                    if (role.RoleName == roleName) { return false; }
                }
            }

            return true;
        }

        public static bool HasAnySocialAuthEnabled(this ISiteContext site)
        {
            if ((site.MicrosoftClientId.Length > 0) && (site.MicrosoftClientSecret.Length > 0)) return true;
            if ((site.FacebookAppId.Length > 0) && (site.FacebookAppSecret.Length > 0)) return true;
            if ((site.GoogleClientId.Length > 0) && (site.GoogleClientSecret.Length > 0)) return true;
            if ((site.TwitterConsumerKey.Length > 0) && (site.TwitterConsumerSecret.Length > 0)) return true;
            
            return false;
        }

        public static bool HasAnySocialAuthEnabled(this ISiteSettings site)
        {
            if ((site.MicrosoftClientId.Length > 0) && (site.MicrosoftClientSecret.Length > 0)) return true;
            if ((site.FacebookAppId.Length > 0) && (site.FacebookAppSecret.Length > 0)) return true;
            if ((site.GoogleClientId.Length > 0) && (site.GoogleClientSecret.Length > 0)) return true;
            if ((site.TwitterConsumerKey.Length > 0) && (site.TwitterConsumerSecret.Length > 0)) return true;

            return false;
        }

        /// <summary>
        /// this method returns false if not configured in site but
        /// fails to account for smtp can be configured from config
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        //public static bool SmtpIsConfigured(this ISiteContext site)
        //{
        //    if (!string.IsNullOrEmpty(site.SmtpServer)) return true;

        //    return false;
        //}

        public static bool SmtpIsConfigured(this ISiteSettings site)
        {
            if (!string.IsNullOrEmpty(site.SmtpServer)) return true;

            return false;
        }

        public static bool SmsIsConfigured(this ISiteContext site)
        {
            if (!string.IsNullOrEmpty(site.SmsClientId)) return true;

            return false;
        }

        public static bool SmsIsConfigured(this ISiteSettings site)
        {
            if (!string.IsNullOrEmpty(site.SmsClientId)) return true;

            return false;
        }





    }
}
