using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public interface IIdentityOptionsResolver
    {
        IdentityOptions ResolveOptions(IdentityOptions singletonOptions);
    }
}
