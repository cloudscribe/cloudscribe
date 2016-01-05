// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-04
// Last Modified:			2016-01-05
// 

using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.Setup
{
    public interface ISetupTask
    {
        
        Task DoSetupStep(
            Func<string, bool, Task> output, // a function that can be used to write page output
            Func<string, bool> appNeedsUpgrade, // a bool function indicating if an app needs schema upgrade
            Func<string, Version> schemaVersionLookup, // a function to lookup the schema Version from mp_SchemaVerssion table if it exists, may return null
            Func<string, Version> codeVersionLookup // a function to lookup the code version of an app from the available list of IVersionProvider, may return null
            );
    }
}
