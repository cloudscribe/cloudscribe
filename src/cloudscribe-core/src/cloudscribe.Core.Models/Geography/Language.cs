// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2014-11-03
// 

using System;

namespace cloudscribe.Core.Models.Geography
{
    public class Language : ILanguage
    {

        public Language()
        { }

        private Guid guid = Guid.Empty;

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        private string name = string.Empty;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string code = string.Empty;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        private int sort = -1;

        public int Sort
        {
            get { return sort; }
            set { sort = value; }
        }


    }
}
