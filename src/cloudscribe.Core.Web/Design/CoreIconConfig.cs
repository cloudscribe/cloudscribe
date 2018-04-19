using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Core.Web.Design
{
    public class CoreIconConfig
    {
        public CoreIconConfig()
        {
            IconSets = new List<CoreIconSet>();
        }

        public string DefaultSetId { get; set; } = "glyphicon";

        public List<CoreIconSet> IconSets { get; set; }

        public CoreIconSet GetIcons(string setId)
        {
            foreach(var set in IconSets)
            {
                if(set.SetId == setId)
                {
                    return set;
                }
            }

            foreach (var set in IconSets)
            {
                if (set.SetId == DefaultSetId)
                {
                    return set;
                }
            }

            return new CoreIconSet();
        }
    }
}
