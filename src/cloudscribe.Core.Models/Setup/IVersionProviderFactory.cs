// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-18
// Last Modified:			2015-08-04
// 

using System.Collections.Generic;

namespace cloudscribe.Core.Models.Setup
{
    public interface IVersionProviderFactory
    {
        IEnumerable<IVersionProvider> VersionProviders { get; }
        IVersionProvider Get(string name);

    }
}
