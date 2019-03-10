using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.Identity
{
    public interface ILdapHelper
    {
        bool IsImplemented { get; }
        Task<LdapUser> TryLdapLogin(ILdapSettings ldapSettings, string userName, string password);
    }

    public class NotImplementedLdapHelper : ILdapHelper
    {
        public bool IsImplemented { get; } = false;

        public Task<LdapUser> TryLdapLogin(ILdapSettings ldapSettings, string userName, string password)
        {
            throw new NotImplementedException();
        }



    }
}
