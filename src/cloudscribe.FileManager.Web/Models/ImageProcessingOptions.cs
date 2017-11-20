namespace cloudscribe.FileManager.Web.Models
{
    public class ImageProcessingOptions
    {
        public string ImageDefaultVirtualSubPath { get; set; } = "/media/images";

       
        public bool AutoResize { get; set; } = true;

        public bool AllowEnlargement { get; set; } = false;

        public bool KeepOriginalImages { get; set; } = true;

        public int WebSizeImageMaxWidth { get; set; } = 550;

        public int WebSizeImageMaxHeight { get; set; } = 550;

        /// <summary>
        /// since we allow passing in the resize options as url params
        /// we need to have limits on how large or small to allow
        /// if someone passes in values out of range we will ignore them and use
        /// the default configured resize options
        /// </summary>
        public int ResizeMaxAllowedWidth { get; set; } = 1024;

        public int ResizeMaxAllowedHeight { get; set; } = 1024;

        public int ResizeMinAllowedWidth { get; set; } = 50;

        public int ResizeMinAllowedHeight { get; set; } = 50;

        public string AllowedFileExtensions { get; set; } = ".gif|.jpg|.jpeg|.svg|.svgz|.png|.flv|.swf|.wmv|.mp3|.mp4|.m4a|.m4v|.oga|.ogv|.webma|.webmv|.webm|.wav|.fla|.tif|.asf|.asx|.avi|.mov|.mpeg|.mpg|.zip|.pdf|.doc|.docx|.xls|.xlsx|.ppt|.pptx|.pps|.csv|.txt|.htm|.html|.css";

        public string ImageFileExtensions { get; set; } = ".gif|.jpg|.jpeg|.png|.svg|.svgz";

        //public string AudioFileExtensions { get; set; } = ".wma|.mp3|.m4a|.m4a|.oga|.webma|.webm|.wav|.asf|.asx|.fla";

        //public string VideoFileExtensions { get; set; } = ".flv|.swf|.wmv|.mp4|.m4v|.ogv|.webmv|.webm|.avi|.mov|.mpeg|.mpg";


    }
}
