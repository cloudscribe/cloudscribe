using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Web.Common.Setup
{
    public interface IVersionProvider
    {
        string Name { get; }
        Guid ApplicationId { get; }
        Version CurrentVersion { get; }
    }
}
