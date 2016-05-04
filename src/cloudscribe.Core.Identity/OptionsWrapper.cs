
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class OptionsWrapper<TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        public OptionsWrapper(TOptions options)
        {
            Value = options;
        }

        public TOptions Value { get; }
    }
}
