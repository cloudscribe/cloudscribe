namespace cloudscribe.Web.Common.Gps
{
    public class IrishNationalGridProjection : IProjection
    {
        public double ScaleFactor { get; } = 1.000035;
        public double TrueOriginLat { get; } = 53.5;
        public double TrueOriginLon { get; } = -8;
        public double TrueOriginEasting { get; } = 200000;
        public double TrueOriginNorthing { get; } = 250000;
    }
}
