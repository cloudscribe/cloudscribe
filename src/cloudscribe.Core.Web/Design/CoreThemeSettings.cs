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
    }
}
