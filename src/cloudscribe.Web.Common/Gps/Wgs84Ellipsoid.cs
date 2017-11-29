namespace cloudscribe.Web.Common.Gps
{
    public class Wgs84Ellipsoid : IEllipsoid
    {
        public double EquatorialRadius { get; } = 6378137;
        public double PolarRadius { get; } = 6356752.3141;
    }
}
