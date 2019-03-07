using System;

namespace cloudscribe.Core.Models.Identity
{
    public interface ILdapHelper
    {
        bool IsImplemented { get; }
        LdapUser TryLdapLogin(ILdapSettings ldapSettings, string userName, string password);
    }

    public class NotImplementedLdapHelper : ILdapHelper
    {
        public bool IsImplemented { get; } = false;

        public LdapUser TryLdapLogin(ILdapSettings ldapSettings, string userName, string password)
        {
            throw new NotImplementedException();
        }



    }
}
