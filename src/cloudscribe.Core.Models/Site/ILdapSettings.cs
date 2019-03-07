namespace cloudscribe.Core.Models
{
    public interface ILdapSettings
    {
        bool UseLdapAuth { get; }
        bool AllowDbFallbackWithLdap { get; }
        bool EmailLdapDbFallback { get; }
        bool AutoCreateLdapUserOnFirstLogin { get; }

        //TODO: the above are not used and could be removed

        string LdapServer { get; }
        string LdapDomain { get; }
        int LdapPort { get; }
        string LdapRootDN { get; }
        string LdapUserDNKey { get; }
    }
}
