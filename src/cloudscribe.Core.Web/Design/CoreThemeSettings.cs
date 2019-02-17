using System.Globalization;

namespace cloudscribe.Core.Web.Design
{
    public class CoreThemeSettings
    {
        public CoreThemeSettings()
        {
            Icons = new CoreIconSet();
        }
        public string ThemeName { get; set; } = "default";
        public string CoreIconSetId { get; set; } = "glyphicons";

        public bool AdminSideNavExpanded{ get; set; }

        public CoreIconSet Icons { get; set; }

        public bool EnableHeaderFooterEditingInSiteSettings { get; set; }

        public int NavbarHeightInPixels { get; set; } = 60;

        public string GetNavbarStyle()
        {
            return "height:" + NavbarHeightInPixels.ToString(CultureInfo.InvariantCulture) + "px;";
        }

        public string GetBodyStyle()
        {
            return "padding-top:" + NavbarHeightInPixels.ToString(CultureInfo.InvariantCulture) + "px;";
        }


    }
}
