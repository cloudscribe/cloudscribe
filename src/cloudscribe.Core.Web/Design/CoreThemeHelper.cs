using cloudscribe.Core.Models;
using Microsoft.Extensions.Options;

namespace cloudscribe.Core.Web.Design
{
    public class CoreThemeHelper : ICoreThemeHelper
    {
        public CoreThemeHelper(
            SiteContext currentSite,
            IOptions<CoreThemeConfig> themeConfigAccessor,
            IOptions<CoreIconConfig> iconConfigAccessor
            )
        {
            _currentSite = currentSite;
            _themeConfig = themeConfigAccessor.Value;
            _iconConfig = iconConfigAccessor.Value;
        }

        private SiteContext _currentSite;
        private CoreThemeConfig _themeConfig;
        private CoreIconConfig _iconConfig;

        public CoreThemeSettings GetThemeSettings()
        {
            CoreThemeSettings result = null;
            foreach(var ts in _themeConfig.ThemeSettings)
            {
                if(ts.ThemeName == _currentSite.Theme)
                {
                    result = ts;
                    break;
                }
            }
            if(result == null)
            {
                foreach (var ts in _themeConfig.ThemeSettings)
                {
                    if (ts.ThemeName == _themeConfig.DefaultTheme)
                    {
                        result = ts;
                        break;
                    }
                }
            }
            if(result == null)
            {
                result = new CoreThemeSettings();
            }
            result.Icons = _iconConfig.GetIcons(result.CoreIconSetId);


            return result;
        }
    }
}
