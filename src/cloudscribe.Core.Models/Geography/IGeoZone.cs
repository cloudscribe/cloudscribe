// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
namespace cloudscribe.Core.Models.Geography
{
    public interface IGeoZone
    {
        string Code { get; set; }
        Guid CountryGuid { get; set; }
        Guid Guid { get; set; }
        string Name { get; set; }
    }
}
