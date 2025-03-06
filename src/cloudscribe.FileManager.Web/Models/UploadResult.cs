namespace cloudscribe.FileManager.Web.Models
{
    public class UploadResult 
    {
        public string ResizedUrl { get; set; }
        public string ThumbUrl { get; set; }
        public string OriginalUrl { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// content length in bytes
        /// </summary>
        public long Length { get; set; }
        public string Type { get; set; }

        /// <summary>
        /// if meta data about the image is stored in a database this property could be used to relate the image to the data
        /// </summary>
        public string DataId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
