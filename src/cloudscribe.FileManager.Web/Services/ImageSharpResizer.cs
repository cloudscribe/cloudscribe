using cloudscribe.FileManager.Web.Models;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.Primitives;
using System;
using System.IO;

namespace cloudscribe.FileManager.Web.Services
{
    public class ImageSharpResizer : IImageResizer
    {
        public ImageSharpResizer(
            ILogger<ImageSharpResizer> logger
            )
        {
            _log = logger;
        }

        private ILogger _log;

        public ImageSize GetImageSize(string pathToImage)
        {
            try
            {
                using (Stream tmpFileStream = File.OpenRead(pathToImage))
                {

                    using (Image<Rgba32> image = Image.Load(tmpFileStream))
                    {
                        return new ImageSize(image.Width, image.Height);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message + " " + ex.StackTrace);
            }

            return null;
        }

        public bool CropExistingImage(
            string sourceFilePath,
            string targetFilePath,
            int offsetX,
            int offsetY,
            int widthToCrop,
            int heightToCrop,
            int finalWidth,
            int finalHeight,
            int quality = 70
            )
        {
            if (string.IsNullOrEmpty(sourceFilePath))
            {
                throw new ArgumentException("imageFilePath must be provided");
            }

            if (string.IsNullOrEmpty(targetFilePath))
            {
                throw new ArgumentException("targetFilePath must be provided");
            }

            if (!File.Exists(sourceFilePath))
            {
                _log.LogError($"imageFilePath does not exist {sourceFilePath}");
                return false;
            }

            if (File.Exists(targetFilePath))
            {
                _log.LogError($"{targetFilePath} already exists");
                return false;
            }

            try
            {
                using (Stream tmpFileStream = File.OpenRead(sourceFilePath))
                {
                    using (Image<Rgba32> fullsizeImage = Image.Load(tmpFileStream))
                    { 
                        var rect = new Rectangle(offsetX, offsetY, widthToCrop, heightToCrop);
                       
                        fullsizeImage
                                .Mutate(x => x
                                   .Crop(rect)
                                   .Resize(finalWidth, finalHeight)
                                );

                        var encoder = GetEncoder(sourceFilePath, quality);

                        using (var fs = new FileStream(targetFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
                        {
                            fullsizeImage.Save(fs, encoder);
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                _log.LogError($"{ex.Message}:{ex.StackTrace}");
                return false;
            }

            return true;
        }

        public bool ResizeImage(
            string sourceFilePath,
            string targetDirectoryPath,
            string newFileName,
            string mimeType,
            int maxWidth,
            int maxHeight,
            bool allowEnlargement = false,
            int quality = 70
            )
        {
            if (string.IsNullOrEmpty(sourceFilePath))
            {
                throw new ArgumentException("imageFilePath must be provided");
            }

            if (string.IsNullOrEmpty(targetDirectoryPath))
            {
                throw new ArgumentException("targetDirectoryPath must be provided");
            }

            if (string.IsNullOrEmpty(newFileName))
            {
                throw new ArgumentException("newFileName must be provided");
            }

            if (!File.Exists(sourceFilePath))
            {
                _log.LogError("imageFilePath does not exist " + sourceFilePath);
                return false;
            }

            if (!Directory.Exists(targetDirectoryPath))
            {
                _log.LogError("targetDirectoryPath does not exist " + targetDirectoryPath);
                return false;
            }

            double scaleFactor = 0;
            bool imageNeedsResizing = true;
            var targetFilePath = Path.Combine(targetDirectoryPath, newFileName);

            if (File.Exists(targetFilePath))
            {
                _log.LogWarning($"resize requested but resized image target path {targetFilePath} already exists, so ignoring");
                return true;
            }

            try
            {
                using (Stream tmpFileStream = File.OpenRead(sourceFilePath))
                {
                    using (Image<Rgba32> fullsizeImage = Image.Load(tmpFileStream))
                    {         
                        scaleFactor = GetScaleFactor(fullsizeImage.Width, fullsizeImage.Height, maxWidth, maxHeight);
                        if (!allowEnlargement)
                        {
                            // don't need to resize since image is smaller than max
                            if (scaleFactor > 1) { imageNeedsResizing = false; }
                            if (scaleFactor == 0) { imageNeedsResizing = false; }
                        }

                        if (imageNeedsResizing)
                        {
                            int newWidth = (int)(fullsizeImage.Width * scaleFactor);
                            int newHeight = (int)(fullsizeImage.Height * scaleFactor);

                            fullsizeImage
                                .Mutate(x => x
                                    .Resize(newWidth, newHeight)
                                );

                            var encoder = GetEncoder(sourceFilePath, quality);
                            
                            using (var fs = new FileStream(targetFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
                            {
                                fullsizeImage.Save(fs, encoder);
                            }
                        }

                    }

                        
                } //end using stream


            }
            catch (OutOfMemoryException ex)
            {
                _log.LogError($"{ex.Message}:{ex.StackTrace}");
                return false;
            }
            catch (ArgumentException ex)
            {
                _log.LogError($"{ex.Message}:{ex.StackTrace}");
                return false;
            }

            return imageNeedsResizing;

        }

        private static IImageEncoder GetEncoder(string fileName, int quality)
        {
            string ext = Path.GetExtension(fileName.ToLowerInvariant());

            switch (ext)
            {
                case ".gif":
                    return new GifEncoder();
                case ".png":
                    return new PngEncoder();
                //case ".webp":
                //    return SKEncodedImageFormat.Webp;
            }

            var j = new JpegEncoder
            {
                Quality = quality
            };
            return j;
        }

        private double GetScaleFactor(int inputWidth, int inputHeight, int maxWidth, int maxHeight)
        {
            double scaleFactor = 0;

            if (inputWidth == 0) { return scaleFactor; }
            if (inputHeight == 0) { return scaleFactor; }
            if (maxHeight == 0) { return scaleFactor; }

            double aspectRatio = (double)inputWidth / (double)inputHeight;
            double boxRatio = (double)maxWidth / (double)maxHeight;

            if (boxRatio > aspectRatio)
            {
                //Use height, since that is the most restrictive dimension of box.
                scaleFactor = (double)maxHeight / (double)inputHeight;
            }
            else
            {
                scaleFactor = (double)maxWidth / (double)inputWidth;
            }

            return scaleFactor;
        }
    }
}
