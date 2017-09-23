using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Web.Common.Analytics
{
    public class StandardPostProps
    {
        public string ClientId { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string Host { get; set; }
    }
}
