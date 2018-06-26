using System.Globalization;

namespace cloudscribe.Web.Common.Models
{
    public class CkeditorOptions
    {
        public string CustomConfigPath { get; set; } = "/cr/js/cloudscribe-ckeditor-config.min.js";
        public string FileBrowseUrl { get; set; } = string.Empty;
        public string ImageBrowseUrl { get; set; } = string.Empty;
        public string DropFileUrl { get; set; } = string.Empty;

        public string CropFileUrl { get; set; } = string.Empty;

        public string LanguageCode { get; set; } = CultureInfo.CurrentUICulture.Name;
    }
}
