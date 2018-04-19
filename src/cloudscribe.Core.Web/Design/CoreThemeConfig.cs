using System.Collections.Generic;

namespace cloudscribe.Core.Web.Design
{
    public class CoreThemeConfig
    {
        public CoreThemeConfig()
        {
            ThemeSettings = new List<CoreThemeSettings>();

        }

        public string DefaultTheme { get; set; } = "default";

        public List<CoreThemeSettings> ThemeSettings { get; set; }

        public CoreThemeSettings GetThemSettings(string themeSettingsId)
        {
            foreach(var theme in ThemeSettings)
            {
                if(theme.ThemeName == themeSettingsId)
                {
                    return theme;
                }

            }

            foreach (var theme in ThemeSettings)
            {
                if (theme.ThemeName == DefaultTheme)
                {
                    return theme;
                }

            }

            return new CoreThemeSettings();

        }
    }
}
