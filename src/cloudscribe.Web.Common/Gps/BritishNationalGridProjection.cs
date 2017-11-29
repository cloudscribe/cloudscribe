namespace cloudscribe.Web.Common.Gps
{
    public class BritishNationalGridProjection : IProjection
    {
        public double ScaleFactor { get; } = 0.9996012717;
        public double TrueOriginLat { get; } = 49;
        public double TrueOriginLon { get; } = -2;
        public double TrueOriginEasting { get; } = 400000;
        public double TrueOriginNorthing { get; } = -100000;
    }
}
