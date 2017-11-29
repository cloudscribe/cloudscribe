namespace cloudscribe.Web.Common.Gps
{
    public class Airy1830Ellipsoid : IEllipsoid
    {
        public double EquatorialRadius { get; } = 6377563.396;
        public double PolarRadius { get; } = 6356256.909;
    }
}
