// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2015-02-10
// Last Modified:		    2015-02-10
// 

namespace cloudscribe.Core.Models.Site
{
    /// <summary>
    /// when new folder sites are created from the ui we need a way to restart the app
    /// or more specifically we want to trigger the execution of the startup logic
    /// so that authentication and routes can be wired up for the new folder site
    /// </summary>
    public interface ITriggerStartup
    {
        bool TriggerStartup();
    }
}
