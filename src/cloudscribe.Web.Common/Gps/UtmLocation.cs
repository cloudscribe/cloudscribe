namespace cloudscribe.Web.Common.Gps
{
    public class UtmLocation
    {
        public UtmLocation(double northing, double easting)
        {
            Northing = northing;
            Easting = easting;
        }

        public double Northing { get; private set; }
        public double Easting { get; private set; }

        public override string ToString()
        {
            return string.Format("{0:G17}, {1:G17}",
                Northing, Easting);
        }
    }
}
