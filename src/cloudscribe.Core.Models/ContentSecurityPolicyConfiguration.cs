using System.Collections.Generic;

namespace cloudscribe.Core.Models
{
    public class ContentSecurityPolicyConfiguration
    {
        public ContentSecurityPolicyConfiguration()
        {
            WhitelistScripts = new List<string>();
            WhitelistImages = new List<string>();
            WhitelistFonts = new List<string>();
            WhitelistStyles = new List<string>();

        }

        public int HstsDays { get; set; } = 180;

        public List<string> WhitelistScripts { get; set; }
        public List<string> WhitelistImages { get; set; }
        public List<string> WhitelistFonts { get; set; }
        public List<string> WhitelistStyles { get; set; }

    }
}
