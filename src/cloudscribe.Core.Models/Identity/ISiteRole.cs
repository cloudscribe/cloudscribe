// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
namespace cloudscribe.Core.Models
{
    public interface ISiteRole
    {
        string DisplayName { get; set; }
        Guid RoleGuid { get; set; }
        int RoleId { get; set; }
        string RoleName { get; set; }
        Guid SiteGuid { get; set; }
        int SiteId { get; set; }
        int MemberCount { get; set; }
    }
}
