namespace cloudscribe.Core.Models
{
    public interface ILdapSettings
    {
        bool UseLdapAuth { get; }
        bool AllowDbFallbackWithLdap { get; }
        bool EmailLdapDbFallback { get; }
        bool AutoCreateLdapUserOnFirstLogin { get; }
        string LdapServer { get; }
        string LdapDomain { get; }
        int LdapPort { get; }
        string LdapRootDN { get; }
        string LdapUserDNKey { get; }
    }
}
