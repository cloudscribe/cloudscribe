// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
namespace cloudscribe.Core.Models.Geography
{
    public interface IGeoCountry
    {
        Guid Id { get; set; }
        string ISOCode2 { get; set; }
        string ISOCode3 { get; set; }
        string Name { get; set; }
    }
}
