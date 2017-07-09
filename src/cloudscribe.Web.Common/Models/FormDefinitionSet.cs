// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-09
// Last Modified:			2017-07-09
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common.Models
{
    public class FormDefinitionSet
    {
        public FormDefinitionSet()
        {
            Forms = new List<FormDefinition>();
        }

        public List<FormDefinition> Forms { get; set; }

    }
}
