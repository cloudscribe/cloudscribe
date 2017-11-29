namespace cloudscribe.Web.Common.Gps
{
    public class Hayford1909Ellipsoid : IEllipsoid
    {
        public double EquatorialRadius { get; } = 6378388;
        public double PolarRadius { get; } = 356911.946;
    }
}
