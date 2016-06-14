// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-04
// Last Modified:			2016-06-14
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
            return model;
        }
        
        
        /// <summary>
        /// system roles cannot be deleted
        /// other roles can also be made not deleteable by AppSettings.RolesThatCannotBeDeleted
        /// this property on the view model is just to show or hide the delete ui elements
        /// it is enforced in the controller and also the database for system roles
        /// </summary>
        public bool IsDeletable { get; set; } = true;
        

        #region ISiteRole
        
        public Guid Id { get; set; } = Guid.Empty;
        public Guid SiteId { get; set; } = Guid.Empty;
        
        public string NormalizedRoleName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Role Name is required")]
        [StringLength(50, ErrorMessage = "RoleNameLengthError", MinimumLength = 3)]
        public string RoleName { get; set; } = string.Empty;
        
        /// <summary>
        /// note that memberCount is only populated in some role list retrieval scenarios
        /// if the value is -1 then it has not been populated
        /// 
        /// </summary>
        public int MemberCount { get; set; } = -1;
        
        #endregion
    }
}
