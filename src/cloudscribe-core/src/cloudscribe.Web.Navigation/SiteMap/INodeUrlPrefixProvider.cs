// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-02
// Last Modified:			2015-08-02
// 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Web.Navigation
{
    public interface INodeUrlPrefixProvider
    {
        string GetPrefix();
    }

    public class DefaultNodeUrlPrefixProvider : INodeUrlPrefixProvider
    {
        public DefaultNodeUrlPrefixProvider()
        { }

        public string GetPrefix()
        {
            return string.Empty;
        }
    }
}
