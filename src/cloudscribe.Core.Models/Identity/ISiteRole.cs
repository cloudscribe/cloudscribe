// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
namespace cloudscribe.Core.Models
{
    public interface ISiteRole
    {
        Guid Id { get; set; }
        Guid SiteId { get; set; }
        string RoleName { get; set; }  
        string NormalizedRoleName { get; set; } 
        int MemberCount { get; set; }
    }
}
