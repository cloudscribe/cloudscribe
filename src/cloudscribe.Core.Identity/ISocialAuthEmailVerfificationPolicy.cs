using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public interface ISocialAuthEmailVerfificationPolicy
    {
        bool HasVerifiedEmail(ExternalLoginInfo externalLoginInfo);
    }
}
