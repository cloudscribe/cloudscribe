using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Messaging
{
    public class SmtpOptions
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 25;
        public string SmtpUser { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public bool SmtpRequiresAuthentication { get; set; } = false;
    }
}
