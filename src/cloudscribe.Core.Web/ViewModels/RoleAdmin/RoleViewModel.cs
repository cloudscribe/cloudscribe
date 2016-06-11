// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-04
// Last Modified:			2016-06-03
//

using cloudscribe.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.RoleAdmin
{
    public class RoleViewModel : ISiteRole
    {
        public static RoleViewModel FromISiteRole(ISiteRole siteRole)
        {
            RoleViewModel model = new RoleViewModel();
            model.Id = siteRole.Id;
            model.NormalizedRoleName = siteRole.NormalizedRoleName;
            model.RoleName = siteRole.RoleName;
            model.SiteId = siteRole.SiteId;
            //model.ConcurrencyStamp = siteRole.ConcurrencyStamp;
            
            return model;

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
        
        private Guid id = Guid.Empty;

        [Display(Name = "RoleId")]
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }
        
        private Guid siteId = Guid.Empty;

        [Display(Name = "SiteId")]
        public Guid SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        
        private string normalizedRoleName = string.Empty;
        public string NormalizedRoleName
        {
            get { return normalizedRoleName; }
            set { normalizedRoleName = value; }
        }

        private string roleName = string.Empty;

        [Required(ErrorMessage = "Role Name is required")]
        //[StringLength(50, ErrorMessageResourceName = "RoleNameLengthError", ErrorMessageResourceType = typeof(CommonResources), MinimumLength = 3)]
        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
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
