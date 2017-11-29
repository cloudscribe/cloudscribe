namespace cloudscribe.Web.Common.Gps
{
    public interface IEllipsoid
    {
        double EquatorialRadius { get; }
        double PolarRadius { get; }
    }
}
