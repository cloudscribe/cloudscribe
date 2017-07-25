namespace cloudscribe.FileManager.Web.Models
{
    public class MediaLoggingEvents
    {
        /// <summary>
        /// ie file drop in ckeditor
        /// </summary>
        public const int FILE_PROCCESSING = 40000;

        public const int RESIZE_OPERATION = 40001;

        public const int FOLDER_CREATION = 40002;

        public const int FOLDER_DELETE = 40003;

        public const int FOLDER_RENAME = 40004;

        public const int FILE_DELETE = 40005;

        public const int FILE_RENAME = 40006;
    }
}
