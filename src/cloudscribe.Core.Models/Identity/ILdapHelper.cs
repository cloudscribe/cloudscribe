using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Core.Models.Identity
{
    public interface ILdapHelper
    {
        bool IsImplemented { get; }
    }

    public class NotImplementedLdapHelper : ILdapHelper
    {
        public bool IsImplemented { get; } = false;
    }
}
