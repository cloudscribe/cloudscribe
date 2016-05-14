using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public interface ISiteHost
    {
        Guid Id { get; set; }
        string HostName { get; set; }
        Guid SiteId { get; set; }

    }
}
