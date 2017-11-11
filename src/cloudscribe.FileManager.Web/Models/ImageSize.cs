using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.FileManager.Web.Models
{
    public class ImageSize
    {
        public ImageSize(int pixelWidth, int pixelHeight)
        {
            PixelHeight = pixelHeight;
            PixelWidth = pixelWidth;
        }
        public int PixelWidth { get; private set; }
        public int PixelHeight { get; private set; }
    }
}
