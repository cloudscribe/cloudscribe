using cloudscribe.Core.Models.Identity;
using Microsoft.Extensions.Logging;
using System;

namespace cloudscribe.Core.Ldap.Windows
{
    public class LdapHelper : ILdapHelper
    {
        public LdapHelper(
            ILogger<LdapHelper> logger
            )
        {
            _log = logger;
        }

        private readonly ILogger _log;

        public bool IsImplemented { get; } = true;

    }
}
