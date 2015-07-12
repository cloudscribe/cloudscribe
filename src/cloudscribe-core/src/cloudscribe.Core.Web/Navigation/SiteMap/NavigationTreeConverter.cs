// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-07
// Last Modified:			2015-07-09
// 


using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Navigation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace cloudscribe.Core.Web.Components
{
    public class NavigationTreeConverter : JsonCreationConverter<TreeNode<NavigationNode>>
    {
        protected override TreeNode<NavigationNode> Create(Type objectType, JObject jObject)
        {
            //jObject.

            throw new NotImplementedException();
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
    }
}
