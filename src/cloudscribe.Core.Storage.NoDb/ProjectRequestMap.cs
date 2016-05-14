using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{
    public class ProjectRequestMap : IProjectRequestMap
    {
        public string ProjectId { get; set; }
        public string HostName { get; set; }
        public string FirstFolderSegment { get; set; }
    }

    internal class DefaultProjectRequestMap : IProjectRequestMap
    {
        public string ProjectId { get { return "default"; } }
        public string HostName { get { return string.Empty; } }
        public string FirstFolderSegment { get { return string.Empty; } }
    }
}
