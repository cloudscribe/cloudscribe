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

        public bool DisableNavbarStyle { get; set; }
        public bool DisableBodyStyle { get; set; }

        public bool ResizeLogoOnUpload { get; set; } = true;

        public string GetNavbarStyle()
        {
            if (DisableNavbarStyle) return null;

            return "height:" + NavbarHeightInPixels.ToString(CultureInfo.InvariantCulture) + "px;";
        }
        
        public string GetBodyStyle()
        {
            if (DisableBodyStyle) return null;

            return "padding-top:" + NavbarHeightInPixels.ToString(CultureInfo.InvariantCulture) + "px;";
        }


    }
}
