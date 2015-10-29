// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2015-08-04
// 

using System;

namespace cloudscribe.Core.Models.Geography
{
    public class Language : ILanguage
    {

        public Language()
        { }
        
        public Guid Guid { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int Sort { get; set; } = -1;
        
    }
}
