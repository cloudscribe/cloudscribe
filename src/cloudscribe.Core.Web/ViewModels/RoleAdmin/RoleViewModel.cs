// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-04
// Last Modified:			2015-06-10
//

using cloudscribe.Core.Models;
//using cloudscribe.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.RoleAdmin
{
    public class RoleViewModel : ISiteRole
    {
        public static RoleViewModel FromISiteRole(ISiteRole siteRole)
        {
            RoleViewModel model = new RoleViewModel();
            model.RoleGuid = siteRole.RoleGuid;
            model.RoleId = siteRole.RoleId;
            model.RoleName = siteRole.RoleName;
            model.DisplayName = siteRole.DisplayName;
            model.SiteGuid = siteRole.SiteGuid;
            model.SiteId = siteRole.SiteId;

            return model;

        }


        private string heading = string.Empty;

        public string Heading
        {
            get { return heading; }
            set { heading = value; }
        }

        private bool isDeletable = true;
        /// <summary>
        /// system roles cannot be deleted
        /// other roles can also be made not deleteable by AppSettings.RolesThatCannotBeDeleted
        /// this property on the view model is just to show or hide the delete ui elements
        /// it is enforced in the controller and also the database for system roles
        /// </summary>
        public bool IsDeletable
        {
            get { return isDeletable; }
            set { isDeletable = value; }
        }

        #region ISiteRole

        private int roleID = -1;

        [Display(Name = "RoleId")]
        public int RoleId
        {
            get { return roleID; }
            set { roleID = value; }
        }

        private Guid roleGuid = Guid.Empty;

        [Display(Name = "RoleGuid")]
        public Guid RoleGuid
        {
            get { return roleGuid; }
            set { roleGuid = value; }
        }

        private int siteID = -1;

        [Display(Name = "SiteId")]
        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        private Guid siteGuid = Guid.Empty;

        [Display(Name = "SiteGuid")]
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        /// <summary>
        /// role name is the actual role name usedin the role cookie
        /// once a role is created the rolename cannot be edited
        /// only displayname can be edited
        /// when a new role is created the displayname is used also to populate the role name
        /// but subsequent edits of displayname do not change the rolename
        /// this is mainly so role names we have coded against don't change while allowing the display to be customized/localized
        /// if you really need to change the rolename, then delete the role and re-create it
        /// certain system roles are not allowed to be deleted
        /// </summary>
        private string roleName = string.Empty;
        [Display(Name = "RoleName")]
        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        private string displayName = string.Empty;

        //[Display(Name = "RoleName", ResourceType = typeof(CommonResources))]
        //[Required(ErrorMessageResourceName = "RoleNameRequired", ErrorMessageResourceType = typeof(CommonResources))]
        //[StringLength(50, ErrorMessageResourceName = "RoleNameLengthError", ErrorMessageResourceType = typeof(CommonResources), MinimumLength = 3)]
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        private int memberCount = -1;
        /// <summary>
        /// note that memberCount is only populated in some role list retrieval scenarios
        /// if the value is -1 then it has not been populated
        /// 
        /// </summary>
        public int MemberCount
        {
            get { return memberCount; }
            set { memberCount = value; }
        }

        #endregion



    }
}
