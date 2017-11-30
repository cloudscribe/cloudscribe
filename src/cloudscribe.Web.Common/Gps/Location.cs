namespace cloudscribe.Web.Common.Gps
{
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
}
