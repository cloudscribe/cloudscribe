// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
namespace cloudscribe.Core.Models
{
    public interface IUserLogin
    {
        Guid SiteId { get; set; }
        Guid UserId { get; set; }
        string LoginProvider { get; set; }
        string ProviderKey { get; set; }
        string ProviderDisplayName { get; set; }
        
    }
}
