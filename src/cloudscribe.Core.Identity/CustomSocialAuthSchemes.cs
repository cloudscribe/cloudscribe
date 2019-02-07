using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Core.Identity
{
    public class CustomSocialAuthSchemes
    {
        public CustomSocialAuthSchemes()
        {
            Schemes = new List<string>();
        }

        public List<string> Schemes { get; set; }
    }
}
