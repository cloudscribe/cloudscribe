namespace cloudscribe.Web.Common.Gps
{
    public class Airy1830ModifiedEllipsoid : IEllipsoid
    {
        public double EquatorialRadius { get; } = 6377340.189;
        public double PolarRadius { get; } = 6356034.447;
    }
}
