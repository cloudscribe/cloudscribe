using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{
    public interface IProjectRequestMap
    {
        string ProjectId { get; }
        string HostName { get; }
        string FirstFolderSegment { get; }
    }
}
