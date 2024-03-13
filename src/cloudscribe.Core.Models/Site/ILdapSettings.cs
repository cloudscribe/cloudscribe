using System;
namespace cloudscribe.Core.Models
{
    public interface ILdapSettings
    {
        Guid Id { get; } //this is siteId from SiteContext
        string LdapServer { get; }
        string LdapDomain { get; }
        int LdapPort { get; }
        string LdapRootDN { get; }
        string LdapUserDNKey { get; }
        string LdapUserDNFormat { get; }
        bool LdapUseSsl { get; }
    }
}
