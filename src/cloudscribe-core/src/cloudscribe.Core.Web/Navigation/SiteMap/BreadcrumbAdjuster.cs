// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-12
// Last Modified:			2015-07-12
// 


using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Navigation
{
    public class BreadcrumbAdjuster
    {
        public BreadcrumbAdjuster(HttpContext context)
        {
            this.context = context;
        }

        private HttpContext context;

        public string KeyToAdjust { get; set; } = string.Empty;

        public string AdjustedText { get; set; } = string.Empty;

        public string AdjustedUrl { get; set; } = string.Empty;

        public void AddToContext()
        {
            if(string.IsNullOrWhiteSpace(KeyToAdjust)) { return; }
           
            string key = "breadcrumb-" + KeyToAdjust;
            if(context.Items[key] == null)
            {
                context.Items[key] = this;
            }
        }
    }
}
