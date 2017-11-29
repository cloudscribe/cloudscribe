namespace cloudscribe.Web.Common.Gps
{
    public class DmsLocation
    {
        public DmsLocation(DmsPoint latitude, DmsPoint longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public DmsPoint Latitude { get; private set; }
        public DmsPoint Longitude { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}",
                Latitude.ToString(), Longitude.ToString());
        }
    }
}
