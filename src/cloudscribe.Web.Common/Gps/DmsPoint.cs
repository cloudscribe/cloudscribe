using System;

namespace cloudscribe.Web.Common.Gps
{
    public class DmsPoint
    {
        public DmsPoint(int degrees, int minutes, double seconds, PointType type)
        {
            Degrees = degrees;
            Minutes = minutes;
            Seconds = seconds;
            Type = type;

        }
        public int Degrees { get; private set; }
        public int Minutes { get; private set; }
        public double Seconds { get; private set; }
        public PointType Type { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2:G17} {3}",
                Math.Abs(Degrees),
                Minutes,
                Seconds,
                Type == PointType.Lat
                    ? Degrees < 0 ? "S" : "N"
                    : Degrees < 0 ? "W" : "E");
        }

        //42;56;42.960000000000001
        public static DmsPoint ParseFromGps(string gps, PointType type, string directionRef, char separator = ';')
        {
            if(!string.IsNullOrWhiteSpace(gps) && !string.IsNullOrWhiteSpace(directionRef))
            {
                var arr = gps.Split(separator);
                if(arr.Length == 3)
                {
                    try
                    {

                        var degrees = Convert.ToInt32(arr[0].Trim());
                        var minutes = Convert.ToInt32(arr[1].Trim());
                        var seconds = Convert.ToDouble(arr[2].Trim());
                        //var seconds = double.Parse(arr[2]);
                        if (directionRef == "S" || directionRef == "W")
                        {
                            degrees = degrees * -1;
                        }
                        return new DmsPoint(degrees, minutes, seconds, type);

                    }
                    catch (FormatException) { }
                    catch (OverflowException) { }
                   
                }
            }


            return null;
        }
    }
}
