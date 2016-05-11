// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
namespace cloudscribe.Core.Models.Geography
{
    public interface IGeoZone
    {
        Guid Id { get; set; }
        Guid CountryId { get; set; }
        string Code { get; set; }  
        string Name { get; set; }
    }
}
