// Author:					Joe Audette
// Created:					2014-08-20
// Last Modified:			2014-12-04
// 
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;


namespace cloudscribe.Core.Models
{

    public class SiteRole : ISiteRole
    {

        public SiteRole()
        { }

        private int roleID = -1;
        
        public int RoleId
        {
            get { return roleID; }
            set { roleID = value; }
        }

        private Guid roleGuid = Guid.Empty;

        public Guid RoleGuid
        {
            get { return roleGuid; }
            set { roleGuid = value; }
        }

        private int siteID = -1;

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        private Guid siteGuid = Guid.Empty;

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        

        private string roleName = string.Empty;

        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        private string displayName = string.Empty;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        private int memberCount = -1;
        /// <summary>
        /// note that memberCount is only populated in some role list retrieval scenarios
        /// if the value is -1 then it has not been populated
        /// </summary>
        public int MemberCount
        {
            get { return memberCount; }
            set { memberCount = value; }
        }

        

      
    }

}
