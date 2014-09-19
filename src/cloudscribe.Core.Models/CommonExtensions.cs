using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public static class CommonExtensions
    {
        public static string ToInvariantString(this int i)
        {
            return i.ToString(CultureInfo.InvariantCulture);

        }

        public static string ToInvariantString(this float i)
        {
            return i.ToString(CultureInfo.InvariantCulture);

        }
    }
}
