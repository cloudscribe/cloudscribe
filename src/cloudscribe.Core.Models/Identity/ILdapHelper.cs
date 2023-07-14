using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.Identity
{
    public interface ILdapHelper
    {
        bool IsImplemented { get; }
        Task<LdapUser> TryLdapLogin(
            ILdapSettings ldapSettings,
            string userName,
            string password
        );
        Task<Dictionary<string,LdapUser>> TestLdapServers(
            ILdapSettings settings,
            string username,
            string password
        );
    }

    public class NotImplementedLdapHelper : ILdapHelper
    {
        public bool IsImplemented { get; } = false;

        public Task<LdapUser> TryLdapLogin(
            ILdapSettings ldapSettings,
            string userName,
            string password
        )
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string,LdapUser>> TestLdapServers(
            ILdapSettings settings,
            string username,
            string password
        )
        {
            throw new NotImplementedException();
        }

    }
}
