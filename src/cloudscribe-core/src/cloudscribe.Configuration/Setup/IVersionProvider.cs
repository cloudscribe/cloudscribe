using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Configuration
{
    public interface IVersionProvider
    {
        string Name { get; }
        Version GetCodeVersion();
    }
}
