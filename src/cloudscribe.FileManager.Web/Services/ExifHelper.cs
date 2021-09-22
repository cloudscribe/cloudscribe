using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cloudscribe.FileManager.Web.Services
{
    public class ExifHelper
    {

        public Dictionary<string, string> ExtractExif(string imageFilePath)
        {
            var result = new Dictionary<string, string>();

            try
            {
                using (Stream tmpFileStream = File.OpenRead(imageFilePath))
                {
                    using (Image<Rgba32> img = (Image<Rgba32>)Image.Load(tmpFileStream))
                    {

                        foreach (var p in img.Metadata.ExifProfile.Values)
                        {

                            if (p.GetValue() is Rational[] rat)
                            {
                                if (rat.Length > 0)
                                {
                                    result.Add(p.Tag.ToString(), ExtractRats(rat));
                                }

                            }
                            else if (p.GetValue() is byte[] b) //leave out byte array props
                            {
                                continue;
                            }
                            else
                            {
                                result.Add(p.Tag.ToString(), p.GetValue().ToString());
                            }

                        }

                    }
                }
            }
            catch (Exception)
            { }
            

            return result;

        }

        public DmsLocation ExtractDmsLocation(string imageFilePath)
        {
            var exif = ExtractExif(imageFilePath);
            return ExtractDmsLocation(exif);
        }

        public DmsLocation ExtractDmsLocation(Dictionary<string, string> exifInfo)
        {
            exifInfo.TryGetValue(ExifTagNames.GPSLatitude, out string latInfo);
            exifInfo.TryGetValue(ExifTagNames.GPSLatitudeRef, out string latRef);
            exifInfo.TryGetValue(ExifTagNames.GPSLongitude, out string longInfo);
            exifInfo.TryGetValue(ExifTagNames.GPSLongitudeRef, out string longRef);

            if(!string.IsNullOrWhiteSpace(latInfo) 
                && !string.IsNullOrWhiteSpace(latRef) 
                && !string.IsNullOrWhiteSpace(longInfo) 
                && !string.IsNullOrWhiteSpace(longRef))
            {
                var lat = DmsPoint.ParseFromGps(latInfo, PointType.Lat, latRef);
                var lon = DmsPoint.ParseFromGps(longInfo, PointType.Lon, longRef);
                if(lat != null && lon != null)
                {
                    return new DmsLocation(lat, lon);
                }
            }

            return null;
        }

        public Location ExtractLocation(string imageFilePath)
        {
            var exif = ExtractExif(imageFilePath);
            return ExtractLocation(exif);
        }

        public Location ExtractLocation(Dictionary<string, string> exifInfo)
        {
            var dms = ExtractDmsLocation(exifInfo);
            if(dms != null)
            {
                return new Location(dms.Latitude.ToDouble(), dms.Longitude.ToDouble());
            }

            return null;
        }

        

        private string ExtractRats(Rational[] rat)
        {
            var sb = new StringBuilder();
            var sep = "";
            foreach(var r in rat)
            {
                sb.Append(sep + r.ToDouble().ToString("G17"));
                sep = ";";
            }
            return sb.ToString();
        }
    
       
    }

    public class Location
    {
        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public override string ToString()
        {
            return string.Format("{0:G17}, {1:G17}",
                Latitude, Longitude);
        }
    }

    public class DmsPoint
    {
        public DmsPoint(int degrees, int minutes, double seconds, PointType type)
        {
            Degrees = degrees;
            Minutes = minutes;
            Seconds = seconds;
            Type = type;

        }
        public int Degrees { get; private set; }
        public int Minutes { get; private set; }
        public double Seconds { get; private set; }
        public PointType Type { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2:G17} {3}",
                Math.Abs(Degrees),
                Minutes,
                Seconds,
                Type == PointType.Lat
                    ? Degrees < 0 ? "S" : "N"
                    : Degrees < 0 ? "W" : "E");
        }

        //42;56;42.960000000000001
        public static DmsPoint ParseFromGps(string gps, PointType type, string directionRef, char separator = ';')
        {
            if (!string.IsNullOrWhiteSpace(gps) && !string.IsNullOrWhiteSpace(directionRef))
            {
                var arr = gps.Split(separator);
                if (arr.Length == 3)
                {
                    try
                    {
                        var degrees = Convert.ToInt32(arr[0].Trim());
                        var minutes = Convert.ToInt32(arr[1].Trim());
                        var seconds = Convert.ToDouble(arr[2].Trim());
                        //var seconds = double.Parse(arr[2]);
                        if (directionRef == "S" || directionRef == "W")
                        {
                            degrees = degrees * -1;
                        }
                        return new DmsPoint(degrees, minutes, seconds, type);

                    }
                    catch (FormatException) { }
                    catch (OverflowException) { }
                }
            }

            return null;
        }

    }
    public class DmsLocation
    {
        public DmsLocation(DmsPoint latitude, DmsPoint longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public DmsPoint Latitude { get; private set; }
        public DmsPoint Longitude { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}",
                Latitude.ToString(), Longitude.ToString());
        }
    }

    public enum PointType
    {
        Lat,
        Lon
    }

    public static class DmsExtensions
    {
        public static double ToDouble(this DmsPoint point)
        {
            if (point == null)
            {
                return default(double);
            }
            var result = (double)Math.Abs(point.Degrees) + (double)point.Minutes / (double)60 + (double)point.Seconds / (double)3600;
            if (point.Degrees < 0) return result * (double)-1;
            return result;
        }

        
    }

    public static class ExifTagNames
    {
        public const string Make = "Make";
        public const string Model = "Model";
        public const string Orientation = "Orientation";
        public const string XResolution = "XResolution";
        public const string YResolution = "YResolution";
        public const string ResolutionUnit = "ResolutionUnit";
        public const string Software = "Software";
        public const string DateTime = "DateTime";
        public const string YCbCrPositioning = "YCbCrPositioning";
        public const string ExposureTime = "ExposureTime";
        public const string FNumber = "FNumber";
        public const string ExposureProgram = "ExposureProgram";
        public const string ISOSpeedRatings = "ISOSpeedRatings";
        public const string DateTimeOriginal = "DateTimeOriginal";
        public const string DateTimeDigitized = "DateTimeDigitized";
        public const string ShutterSpeedValue = "ShutterSpeedValue";
        public const string ApertureValue = "ApertureValue";
        public const string BrightnessValue = "BrightnessValue";
        public const string ExposureBiasValue = "ExposureBiasValue";
        public const string MeteringMode = "MeteringMode";
        public const string Flash = "Flash";
        public const string FocalLength = "FocalLength";
        public const string SubsecTimeOriginal = "SubsecTimeOriginal";
        public const string SubsecTimeDigitized = "SubsecTimeDigitized";
        public const string ColorSpace = "ColorSpace";
        public const string PixelXDimension = "PixelXDimension";
        public const string PixelYDimension = "PixelYDimension";
        public const string SensingMethod = "SensingMethod";
        public const string SceneType = "SceneType";
        public const string ExposureMode = "ExposureMode";
        public const string WhiteBalance = "WhiteBalance";
        public const string FocalLengthIn35mmFilm = "FocalLengthIn35mmFilm";
        public const string SceneCaptureType = "SceneCaptureType";
        public const string LensInfo = "LensInfo";
        public const string LensMake = "LensMake";
        public const string LensModel = "LensModel";
        public const string GPSLatitudeRef = "GPSLatitudeRef";
        public const string GPSLatitude = "GPSLatitude";
        public const string GPSLongitudeRef = "GPSLongitudeRef";
        public const string GPSLongitude = "GPSLongitude";
        public const string GPSAltitudeRef = "GPSAltitudeRef";
        public const string GPSAltitude = "GPSAltitude";
        public const string GPSTimestamp = "GPSTimestamp";
        public const string GPSSpeedRef = "GPSSpeedRef";
        public const string GPSSpeed = "GPSSpeed";
        public const string GPSImgDirectionRef = "GPSImgDirectionRef";
        public const string GPSImgDirection = "GPSImgDirection";
        public const string GPSDestBearingRef = "GPSDestBearingRef";
        public const string GPSDestBearing = "GPSDestBearing";
        public const string GPSDateStamp = "GPSDateStamp";

    }
}
