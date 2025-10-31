// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					AI Assistant
// Created:					2025-10-31
// Last Modified:			2025-10-31
//

using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.RoleAdmin
{
    public class CopyRoleViewModel
    {
        public Guid SourceRoleId { get; set; }
        
        public string SourceRoleName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Role Name is required")]
        [StringLength(50, ErrorMessage = "RoleNameLengthError", MinimumLength = 3)]
        public string NewRoleName { get; set; } = string.Empty;
        
        public bool IncludeExistingUsers { get; set; } = false;
    }
}
