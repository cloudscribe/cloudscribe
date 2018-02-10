using System;

namespace cloudscribe.Web.Common.Gps
{
    public static class Extensions
    {
        public static Location ToLocation(this DmsLocation dmsLocation)
        {
            if (dmsLocation == null)
            {
                return null;
            }

            return new Location(dmsLocation.Latitude.ToDouble(), dmsLocation.Longitude.ToDouble());
        }

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

        public static int ToDegrees(this double d)
        {
            return (int)d;
        }


        public static int ToMinutes(this double d)
        {
            d = Math.Abs(d);
            return (int)((d - d.ToDegrees()) * 60);
        }

        public static double ToSeconds(this double d)
        {
            d = Math.Abs(d);
            double minutes = (d - (double)d.ToDegrees()) * (double)60;
            return (minutes - d.ToMinutes()) * (double)60;
            //return (minutes - (double)((d - (double)d.ToDegrees()) * (double)60)) * (double)60;
        }

        public static DmsLocation ToDmsLocation(this Location location)
        {
            
            return new DmsLocation( 
                new DmsPoint(location.Latitude.ToDegrees(), location.Latitude.ToMinutes(), location.Latitude.ToSeconds(), PointType.Lat),
                new DmsPoint(location.Longitude.ToDegrees(), location.Longitude.ToMinutes(), location.Longitude.ToSeconds(), PointType.Lon)    
                );
        }

        public static double DegreesToRadians(this double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double RadiansToDegrees(this double radians)
        {
            return radians * (180 / Math.PI);
        }

        // longitude = x, easting = x
        // latitude = y, northing = y
        // this is not producing correct results
        //public static UtmLocation ToUtmLocation(this Location location, IProjection projection = null, IEllipsoid ellipsoid = null)
        //{
        //    if (projection == null) projection = new BritishNationalGridProjection();
        //    if (ellipsoid == null) ellipsoid = new Airy1830Ellipsoid();

        //    var latRad = location.Latitude.DegreesToRadians();
        //    var longRad = location.Longitude.DegreesToRadians();
        //    var lat0 = projection.TrueOriginLat.DegreesToRadians();
        //    var lon0 = projection.TrueOriginLon.DegreesToRadians();


        //    var eccentricitySquared = 1 - (ellipsoid.PolarRadius * ellipsoid.PolarRadius) / (ellipsoid.EquatorialRadius * ellipsoid.EquatorialRadius);
        //    var n = (ellipsoid.EquatorialRadius - ellipsoid.PolarRadius) / (ellipsoid.EquatorialRadius + ellipsoid.PolarRadius);
        //    var n2 = n * n;
        //    var n3 = n * n * n;

        //    var cosLat = Math.Cos(latRad);
        //    var sinLat = Math.Sin(latRad);
        //    var transverseRadiusOfCurvature = ellipsoid.EquatorialRadius * projection.ScaleFactor / Math.Sqrt(1 - eccentricitySquared * sinLat * sinLat);
        //    var meridionalRadiusOfCurvature = ellipsoid.EquatorialRadius * projection.ScaleFactor * (1 - eccentricitySquared) / Math.Pow(1 - eccentricitySquared * sinLat * sinLat, 1.5); 

        //    var eta2 = transverseRadiusOfCurvature / meridionalRadiusOfCurvature - 1;

        //    var Ma = (1 + n + (5 / 4) * n2 + (5 / 4) * n3) * (latRad - lat0);
        //    var Mb = (3 * n + 3 * n * n + (21 / 8) * n3) * Math.Sin(latRad - lat0) * Math.Cos(latRad + lat0);
        //    var Mc = ((15 / 8) * n2 + (15 / 8) * n3) * Math.Sin(2 * (latRad - lat0)) * Math.Cos(2 * (latRad + lat0));
        //    var Md = (35 / 24) * n3 * Math.Sin(3 * (latRad - lat0)) * Math.Cos(3 * (latRad + lat0));
        //    var meridionalArc = ellipsoid.PolarRadius * projection.ScaleFactor * (Ma - Mb + Mc - Md);

        //    var cos3lat = cosLat * cosLat * cosLat;
        //    var cos5lat = cos3lat * cosLat * cosLat;
        //    var tan2lat = Math.Tan(latRad) * Math.Tan(latRad);
        //    var tan4lat = tan2lat * tan2lat;

        //    var I = meridionalArc + projection.ScaleFactor;
        //    var II = (transverseRadiusOfCurvature / 2) * sinLat * cosLat;
        //    var III = (transverseRadiusOfCurvature / 24) * sinLat * cos3lat * (5 - tan2lat + 9 * eta2);
        //    var IIIA = (transverseRadiusOfCurvature / 720) * sinLat * cos5lat * (61 - 58 * tan2lat + tan4lat);
        //    var IV = transverseRadiusOfCurvature * cosLat;
        //    var V = (transverseRadiusOfCurvature / 6) * cos3lat * (transverseRadiusOfCurvature / meridionalRadiusOfCurvature - tan2lat);
        //    var VI = (transverseRadiusOfCurvature / 120) * cos5lat * (5 - 18 * tan2lat + tan4lat + 14 * eta2 - 58 * tan2lat * eta2);

        //    var dLon = longRad - lon0;
        //    var dLon2 = dLon * dLon;
        //    var dLon3 = dLon2 * dLon;
        //    var dLon4 = dLon3 * dLon;
        //    var dLon5 = dLon4 * dLon;
        //    var dLon6 = dLon5 * dLon;

        //    var northing = I + II * dLon2 + III * dLon4 + IIIA * dLon6;
        //    var easting = projection.TrueOriginEasting + IV * dLon + V * dLon3 + VI * dLon5;

        //    return new UtmLocation(northing, easting);
        //}


    }
}
