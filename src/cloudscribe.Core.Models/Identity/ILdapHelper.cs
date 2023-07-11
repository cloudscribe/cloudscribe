using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.Identity
{
    public interface ILdapHelper
    {
        bool IsImplemented { get; }
        Task<LdapUser> TryLdapLogin(ILdapSettings ldapSettings, string userName, string password, string siteId = null);
        Task<Dictionary<string,string>> TestLdapServers(ILdapSettings settings, string username, string password);
    }

    public class NotImplementedLdapHelper : ILdapHelper
    {
        public bool IsImplemented { get; } = false;

        public Task<LdapUser> TryLdapLogin(ILdapSettings ldapSettings, string userName, string password, string siteId = null)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string,string>> TestLdapServers(ILdapSettings settings, string username, string password)
        {
            throw new NotImplementedException();
        }

    }
}
