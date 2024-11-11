﻿using System.Globalization;

namespace cloudscribe.Web.Common.Models
{
    public class TinyMceEditorOptions
    {
        public string CustomConfigPath { get; set; } = "/cr/js/cloudscribe-tinymceeditor-config.json";
        public string FileBrowseUrl { get; set; } = string.Empty;
        public string ImageBrowseUrl { get; set; } = string.Empty;
        public string VideoBrowseUrl { get; set; } = string.Empty;
        public string AudioBrowseUrl { get; set; } = string.Empty;
        public string DropFileUrl { get; set; } = string.Empty;

        public string CropFileUrl { get; set; } = string.Empty;

        public string LanguageCode { get; set; } = CultureInfo.CurrentUICulture.Name;
    }
}
