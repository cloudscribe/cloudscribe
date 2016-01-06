// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-01-10
// Last Modified:			2016-01-06
// 
using System;

namespace cloudscribe.Core.Models.Setup
{
    public interface IVersionProvider
    {
        string Name { get; }
        Guid ApplicationId { get; }
        Version CurrentVersion { get; }
    }
}
