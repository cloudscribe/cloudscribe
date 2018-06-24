//using cloudscribe.FileManager.Web.Models;
//using Microsoft.Extensions.Logging;
//using SkiaSharp;
//using System;
//using System.IO;


//namespace cloudscribe.FileManager.Web.Services
//{
//    public class SkiaSharpImageResizer : IImageResizer
//    {
//        public SkiaSharpImageResizer(
//            ILogger<SkiaSharpImageResizer> logger
//            )
//        {
//            log = logger;
//        }

//        private ILogger log;

//        public ImageSize GetImageSize(string pathToImage)
//        {
//            //TODO: this processor is not currently used
//            return null;
//        }


//        public bool ResizeImage(
//            string sourceFilePath,
//            string targetDirectoryPath,
//            string newFileName,
//            string mimeType,
//            int maxWidth,
//            int maxHeight,
//            bool allowEnlargement = false,
//            int quality = 90
//            )
//        {
//           // var backgroundColor = default(Color)
//            if (string.IsNullOrEmpty(sourceFilePath))
//            {
//                throw new ArgumentException("imageFilePath must be provided");
//            }

//            if (string.IsNullOrEmpty(targetDirectoryPath))
//            {
//                throw new ArgumentException("targetDirectoryPath must be provided");
//            }

//            if (string.IsNullOrEmpty(newFileName))
//            {
//                throw new ArgumentException("newFileName must be provided");
//            }

//            if (!File.Exists(sourceFilePath))
//            {
//                log.LogError("imageFilePath does not exist " + sourceFilePath);
//                return false;
//            }

//            if (!Directory.Exists(targetDirectoryPath))
//            {
//                log.LogError("targetDirectoryPath does not exist " + targetDirectoryPath);
//                return false;
//            }

//            double scaleFactor = 0;
//            bool imageNeedsResizing = true;
//            var targetFilePath = Path.Combine(targetDirectoryPath, newFileName);
//            var format = GetFormat(sourceFilePath);

//            try
//            {
                
//                using (Stream tmpFileStream = File.OpenRead(sourceFilePath))
//                {

//                    using (var fullsizeImage = SKBitmap.Decode(tmpFileStream))
//                    {

//                        scaleFactor = GetScaleFactor(fullsizeImage.Width, fullsizeImage.Height, maxWidth, maxHeight);


//                        if (!allowEnlargement)
//                        {
//                            // don't need to resize since image is smaller than max
//                            if (scaleFactor > 1) { imageNeedsResizing = false; }
//                            if (scaleFactor == 0) { imageNeedsResizing = false; }
//                        }

//                        if (imageNeedsResizing)
//                        {
//                            int newWidth = (int)(fullsizeImage.Width * scaleFactor);
//                            int newHeight = (int)(fullsizeImage.Height * scaleFactor);

//                            //string thumbnailPath = Path.Combine(dir, $"{displayName}-{width}x{height}{ext}");
//                            var info = new SKImageInfo(newWidth, newHeight);

//                            using (var resized = fullsizeImage.Resize(info, SKBitmapResizeMethod.Lanczos3))
//                            using (var thumb = SKImage.FromBitmap(resized))
//                            using (var fs = new FileStream(targetFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
//                            {
//                                thumb.Encode(format, quality)
//                                     .SaveTo(fs);
//                            }
//                        }
//                    }

//                } //end using stream


//            }
//            catch (OutOfMemoryException ex)
//            {
//                log.LogError(MediaLoggingEvents.RESIZE_OPERATION, ex, ex.Message);
//                return false;
//            }
//            catch (ArgumentException ex)
//            {
//                log.LogError(MediaLoggingEvents.RESIZE_OPERATION, ex, ex.Message);
//                return false;
//            }

//            return imageNeedsResizing;


//        }

//        private static SKEncodedImageFormat GetFormat(string fileName)
//        {
//            string ext = Path.GetExtension(fileName.ToLowerInvariant());

//            switch (ext)
//            {
//                case ".gif":
//                    return SKEncodedImageFormat.Gif;
//                case ".png":
//                    return SKEncodedImageFormat.Png;
//                case ".webp":
//                    return SKEncodedImageFormat.Webp;
//            }

//            return SKEncodedImageFormat.Jpeg;
//        }

//        private double GetScaleFactor(int inputWidth, int inputHeight, int maxWidth, int maxHeight)
//        {
//            double scaleFactor = 0;

//            if (inputWidth == 0) { return scaleFactor; }
//            if (inputHeight == 0) { return scaleFactor; }
//            if (maxHeight == 0) { return scaleFactor; }

//            double aspectRatio = (double)inputWidth / (double)inputHeight;
//            double boxRatio = (double)maxWidth / (double)maxHeight;

//            if (boxRatio > aspectRatio)
//            {
//                //Use height, since that is the most restrictive dimension of box.
//                scaleFactor = (double)maxHeight / (double)inputHeight;
//            }
//            else
//            {
//                scaleFactor = (double)maxWidth / (double)inputWidth;
//            }

//            return scaleFactor;
//        }

//        //private ImageCodecInfo GetEncoderInfo(string mimeType)
//        //{
//        //    // Get image codecs for all image formats
//        //    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

//        //    // Find the correct image codec
//        //    for (int i = 0; i < codecs.Length; i++)
//        //        if (codecs[i].MimeType == mimeType)
//        //            return codecs[i];
//        //    // if not found use jpeg
//        //    return codecs.FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
//        //}

//        //public bool ResizeImage(
//        //    string sourceFilePath,
//        //    string targetDirectoryPath,
//        //    string newFileName,
//        //    string mimeType,
//        //    int maxWidth,
//        //    int maxHeight,
//        //    bool allowEnlargement = false,
//        //    long quality = 90
//        //    )
//        //{
//        //    // var backgroundColor = default(Color)
//        //    if (string.IsNullOrEmpty(sourceFilePath))
//        //    {
//        //        throw new ArgumentException("imageFilePath must be provided");
//        //    }

//        //    if (string.IsNullOrEmpty(targetDirectoryPath))
//        //    {
//        //        throw new ArgumentException("targetDirectoryPath must be provided");
//        //    }

//        //    if (string.IsNullOrEmpty(newFileName))
//        //    {
//        //        throw new ArgumentException("newFileName must be provided");
//        //    }

//        //    if (!File.Exists(sourceFilePath))
//        //    {
//        //        log.LogError("imageFilePath does not exist " + sourceFilePath);
//        //        return false;
//        //    }

//        //    if (!Directory.Exists(targetDirectoryPath))
//        //    {
//        //        log.LogError("targetDirectoryPath does not exist " + targetDirectoryPath);
//        //        return false;
//        //    }

//        //    double scaleFactor = 0;
//        //    bool imageNeedsResizing = true;
//        //    var targetFilePath = Path.Combine(targetDirectoryPath, newFileName);

//        //    try
//        //    {

//        //        using (Stream tmpFileStream = File.OpenRead(sourceFilePath))
//        //        {
//        //            using (Image fullsizeImage = Image.FromStream(tmpFileStream))
//        //            {
//        //                scaleFactor = GetScaleFactor(fullsizeImage.Width, fullsizeImage.Height, maxWidth, maxHeight);

//        //                if (!allowEnlargement)
//        //                {
//        //                    // don't need to resize since image is smaller than max
//        //                    if (scaleFactor > 1) { imageNeedsResizing = false; }
//        //                    if (scaleFactor == 0) { imageNeedsResizing = false; }
//        //                }

//        //                if (imageNeedsResizing)
//        //                {
//        //                    int newWidth = (int)(fullsizeImage.Width * scaleFactor);
//        //                    int newHeight = (int)(fullsizeImage.Height * scaleFactor);

//        //                    var codecInfo = GetEncoderInfo(mimeType);

//        //                    using (Bitmap resizedBitmap = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb))
//        //                    {
//        //                        using (Graphics graphics = Graphics.FromImage(resizedBitmap))
//        //                        {
//        //                            using (var attributes = new ImageAttributes())
//        //                            {
//        //                                attributes.SetWrapMode(WrapMode.TileFlipXY);
//        //                                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
//        //                                graphics.CompositingQuality = CompositingQuality.HighSpeed;
//        //                                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
//        //                                graphics.CompositingMode = CompositingMode.SourceCopy;
//        //                                graphics.DrawImage(
//        //                                    fullsizeImage,
//        //                                    Rectangle.FromLTRB(0, 0, newWidth, newHeight),
//        //                                    0, 0, fullsizeImage.Width, fullsizeImage.Height, GraphicsUnit.Pixel, attributes);
//        //                                // Save the results
//        //                                using (var output = File.Open(targetFilePath, FileMode.Create))
//        //                                {
//        //                                    using (EncoderParameters encoderParams = new EncoderParameters(1))
//        //                                    {
//        //                                        encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
//        //                                        resizedBitmap.Save(output, codecInfo, encoderParams);
//        //                                    }

//        //                                }
//        //                            } //end attributes

//        //                        } //end graphics

//        //                    } //end using resized bitmap

//        //                } //if (imageNeedsResizing)

//        //            } //end using bitmap
//        //        } //end using stream


//        //    }
//        //    catch (OutOfMemoryException ex)
//        //    {
//        //        log.LogError(MediaLoggingEvents.RESIZE_OPERATION, ex, ex.Message);
//        //        return false;
//        //    }
//        //    catch (ArgumentException ex)
//        //    {
//        //        log.LogError(MediaLoggingEvents.RESIZE_OPERATION, ex, ex.Message);
//        //        return false;
//        //    }

//        //    return imageNeedsResizing;


//        //}

//    }
//}
