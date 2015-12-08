// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2015-12-08
// 

using System;

namespace cloudscribe.Core.Models.Geography
{
    public class Language : ILanguage
    {

        public Language()
        { }
        
        public Guid Guid { get; set; } = Guid.Empty;

        private string name = string.Empty;
        public string Name
        {
            get { return name ?? string.Empty; }
            set { name = value; }
        }

        private string code = string.Empty;
        public string Code
        {
            get { return code ?? string.Empty; }
            set { code = value; }
        }
        
        public int Sort { get; set; } = -1;

        public static Language FromILanguage(ILanguage ilang)
        {
            Language lang = new Language();
            lang.Guid = ilang.Guid;
            lang.Sort = ilang.Sort;
            lang.Code = ilang.Code;
            lang.Name = ilang.Name;

            return lang;
        }

    }
}
