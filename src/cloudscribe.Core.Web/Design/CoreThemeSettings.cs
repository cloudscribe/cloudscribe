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

        public bool AllowGravatar { get; set; } = true;

        /// <summary>
        /// used if not using gravatar and user has no avatar
        /// </summary>
        public string DefaultAvatarUrl { get; set; } = "/cr/images/generic-user.png";

        public bool AdminSideNavExpanded{ get; set; }

        public CoreIconSet Icons { get; set; }

        public bool EnableHeaderFooterEditingInSiteSettings { get; set; }

        public int NavbarHeightInPixels { get; set; } = 60;

        public int BrandHeaderHeightInPixels { get; set; } = 0;

  

        public bool DisableNavbarStyle { get; set; }
        public bool DisableBodyStyle { get; set; }

        public bool ResizeLogoOnUpload { get; set; } = true;

        public string GetBrandHeaderStyle()
        {
            if (DisableNavbarStyle) return null;

            return "height:" + BrandHeaderHeightInPixels.ToString(CultureInfo.InvariantCulture) + "px;";
        }

        public string GetNavbarStyle()
        {
            if (DisableNavbarStyle) return null;

            var style = "height:" + NavbarHeightInPixels.ToString(CultureInfo.InvariantCulture) + "px;top:" + BrandHeaderHeightInPixels.ToString(CultureInfo.InvariantCulture) + "px;";
            

            return style;
        }
        
        public string GetBodyStyle()
        {
            if (DisableBodyStyle) return null;
            var bodyPadding = BrandHeaderHeightInPixels + NavbarHeightInPixels;

            return "padding-top:" + bodyPadding.ToString(CultureInfo.InvariantCulture) + "px;";
        }


    }
}
