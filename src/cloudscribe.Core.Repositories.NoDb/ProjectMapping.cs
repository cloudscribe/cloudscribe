using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.NoDb
{
    public class ProjectMapping
    {
        public string ProjectId { get; set; } = "common";
        public string SiteGuid { get; set; } = string.Empty;
        public int SiteId { get; set; } = -1;
        public string HostName { get; set; } = string.Empty;
        public string FolderName { get; set; } = string.Empty;
        public int Port { get; set; } = 0;
    }
}
