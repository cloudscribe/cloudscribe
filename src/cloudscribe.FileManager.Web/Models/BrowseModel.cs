namespace cloudscribe.FileManager.Web.Models
{
    public class BrowseModel
    {
        /// <summary>
        /// the type of files to browse
        /// </summary>
        public string Type { get; set; } = "file";

        /// <summary>
        /// the client side id of the editor
        /// </summary>
        public string CKEditor { get; set; }

        public string CKEditorFuncNum { get; set; }

        public string LangCode { get; set; } = "en";

        public bool ResizeImages { get; set; } = true;
        public bool KeepOriginal { get; set; } 

        public string InitialVirtualPath { get; set; }

        public string FileTreeServiceUrl { get; set; }

        public string UploadServiceUrl { get; set; }
        public string FileDownloadServiceUrl { get; set; }

        public string CreateFolderServiceUrl { get; set; }

        public string DeleteFolderServiceUrl { get; set; }

        public string RenameFolderServiceUrl { get; set; }

        public string DeleteFileServiceUrl { get; set; }

        public string RenameFileServiceUrl { get; set; }

        public string AllowedFileExtensions { get; set; }

        public bool CanDelete { get; set; }


    }
}
