

namespace cloudscribe.FileManager.Web.Models
{
    public interface IImageResizer
    {
        /// <summary>
        /// returns a boolean indicating if the image was resized. determined by whether the input image is smaller thena the resize options and whether enlargement is allowed
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="targetDirectoryPath"></param>
        /// <param name="newFileName"></param>
        /// <param name="mimeType"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="allowEnlargement"></param>
        /// <param name="quality"></param>
        /// <param name="backgroundColor"></param>
        /// <returns></returns>
        bool ResizeImage(
            string sourceFilePath,
            string targetDirectoryPath,
            string newFileName,
            string mimeType,
            int maxWidth,
            int maxHeight,
            bool allowEnlargement = false,
            int quality = 70 
            );

        bool CropExistingImage(
            string sourceFilePath,
            string targetFilePath,
            int offsetX,
            int offsetY,
            int widthToCrop,
            int heightToCrop,
            int finalWidth,
            int finalHeight,
            int quality = 70
            );

        ImageSize GetImageSize(string pathToImage);
    }
}
