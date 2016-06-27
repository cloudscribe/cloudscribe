

using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serilog.NoDb.Web.Models
{
    public class LogEventWrapper
    {
        public LogEventWrapper(LogEvent logEvent, string key)
        {
            Event = logEvent;
            Key = key;
        }

        public string Key { get; private set; }
        public LogEvent Event { get; private set; }
    }
}
