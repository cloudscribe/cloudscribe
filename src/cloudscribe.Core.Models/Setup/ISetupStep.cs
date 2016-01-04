// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-04
// Last Modified:			2016-01-04
// 

using Microsoft.AspNet.Http;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.Setup
{
    public interface ISetupStep
    {
        Task<bool> DoSetupStep(HttpResponse response);
    }
}
