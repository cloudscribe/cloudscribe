// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-04
// Last Modified:			2016-01-04
// 

using cloudscribe.Core.Models.Setup;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class EnsureSiteSetupStep : ISetupStep
    {
        public EnsureSiteSetupStep()
        {

        }

        public async Task<bool> DoSetupStep(HttpResponse response)
        {
            var message = "Testing an ISetupStep";

                await response.WriteAsync(
                    string.Format("{0} - {1}",
                    message,
                    DateTime.UtcNow));
           
            await response.WriteAsync("<br/>");


            return false;
        }

    }

    

}
