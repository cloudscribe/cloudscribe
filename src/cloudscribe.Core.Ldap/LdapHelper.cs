using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace cloudscribe.Core.Ldap
{
    //https://stackoverflow.com/questions/49682644/asp-net-core-2-0-ldap-active-directory-authentication

    public class LdapHelper : ILdapHelper
    {
        public LdapHelper(
            ILdapSslCertificateValidator ldapSslCertificateValidator,
            ILogger<LdapHelper> logger,
            IMemoryCache memoryCache,
            IConfiguration configuration
            )
        {
            _ldapSslCertificateValidator = ldapSslCertificateValidator;
            _log = logger;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        private readonly ILdapSslCertificateValidator _ldapSslCertificateValidator;
        private readonly ILogger _log;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public bool IsImplemented { get; } = true;

        public Task<LdapUser> TryLdapLogin(
            ILdapSettings ldapSettings,
            string userName,
            string password)
        {
            LdapUser user = ValidateUser(ldapSettings, userName, password);
            return Task.FromResult(user);
        }

        public Task<Dictionary<string,LdapUser>> TestLdapServers(
            ILdapSettings settings,
            string username,
            string password
        )
        {
            var result = new Dictionary<string,LdapUser>();
            var userDn = makeUserDn(settings, username);
            bool getLdapUserDetails = _configuration.GetValue<bool>("LdapOptions:GetLdapUserDetails", false);

            //determine which LDAP server to use
            string[] servers = settings.LdapServer.Split(',');
            int activeConnection = 0;
            while(activeConnection < servers.Length) //only try each server in the list once
            {
                var user = new LdapUser();
                string activeServer = servers[activeConnection].Trim(); //the current host/ip we will use
                string message = $"Test querying LDAP server: {activeServer} for {userDn} -";
                try
                {
                    using (var connection = GetConnection(activeServer, settings.LdapPort, settings.LdapUseSsl))
                    {
                        connection.Bind(userDn, password);
                        if (connection.Bound)
                        {
                            _log.LogInformation($"{message} bind succeeded");
                            if (getLdapUserDetails)
                            {
                                LdapEntry entry = GetOneUserEntry(connection, settings, username);
                                if (entry == null)
                                {
                                    _log.LogWarning($"{message} user details query succeeded, but no user attributes were returned");
                                }
                                else
                                {
                                    user = BuildUserFromEntry(entry);
                                    _log.LogInformation($"{message} user details query succeeded.\nEmail: {user.Email}\nFirstame: {user.FirstName}\nLastname: {user.LastName}\nDisplayName: {user.DisplayName}");
                                }
                            }
                            user.ResultStatus = "PASS";
                            result.Add(activeServer, user);
                        }
                        else
                        {
                            _log.LogWarning($"{message} bind failed");
                            user.ResultStatus = "Bind Failed";
                            result.Add(activeServer, user);
                        }
                        connection.Disconnect();
                    }
                }
                catch(Exception ex)
                {

                    switch (ex.Message) {
                        case "Connect Error":
                            _log.LogError($"{message} connect to LDAP server failed");
                            user.ResultStatus = "Connect Error";
                            result.Add(activeServer, user);
                            break;
                        case "Invalid Credentials":
                        case "Unwilling To Perform":
                            _log.LogWarning($"{message} bind failed. The exception was:\n{ex.Message}:{ex.StackTrace}");
                            user.ResultStatus = "Bind Failed";
                            result.Add(activeServer, user);
                            break;
                        default:
                            _log.LogError($"{message} connect to LDAP server failed. The exception was:\n{ex.Message}:{ex.StackTrace}");
                            user.ResultStatus = ex.Message;
                            result.Add(activeServer, user);
                            break;
                    }
                }
                activeConnection++;
            }

            return Task.FromResult(result);
        }

        private string makeUserDn(ILdapSettings settings , string username)
        {
            //determine which DN format the LDAP server uses for user authentication
            string userDn;
            switch (settings.LdapUserDNFormat)
            {
                case "username@LDAPDOMAIN":     //support for Active Directory (using userPrincipalName)
                    userDn = $"{username}@{settings.LdapDomain}";
                    break;
                case "uid=username,LDAPDOMAIN": //new additional support for Open LDAP or 389 Directory Server
                    userDn = $"uid={username},{settings.LdapDomain}";
                    break;
                default:
                    userDn = $"{settings.LdapDomain}\\{username}"; //Active Directory (using sAMAccountName)
                    break;
            }
            return userDn;
        }

        private string makeUserFilter(ILdapSettings settings , string username)
        {
            //determine which format a user filter should be in when we are querying for user details
            string filter;
            switch (settings.LdapUserDNFormat)
            {
                case "username@LDAPDOMAIN":     //support for Active Directory (using userPrincipalName)
                    filter = $"(sAMAccountName={username})";
                    break;
                case "uid=username,LDAPDOMAIN": //new additional support for Open LDAP or 389 Directory Server
                    filter = $"(&(|(objectclass=person)(objectclass=iNetOrgPerson))(uid={username}))";
                    break;
                default:                        //Active Directory (using sAMAccountName)
                    filter = $"(sAMAccountName={username})";
                    break;
            }
            return filter;
        }

        private string makeSearchBaseDn(ILdapSettings settings, string username)
        {
            //determine which base DN format the LDAP server uses for searches. In AD this is different from the user DN format.
            string searchBase = string.Empty;
            switch (settings.LdapUserDNFormat)
            {
                case "username@LDAPDOMAIN":     //support for Active Directory (using userPrincipalName)
                    var domainParts1 = settings.LdapDomain.Split('.'); //a dotted format domain needs to be converted to a DC= format
                    foreach(var part in domainParts1) searchBase += $"dc={part},";
                    searchBase = "cn=users," + searchBase.TrimEnd(',');
                    break;
                case "uid=username,LDAPDOMAIN": //new additional support for Open LDAP or 389 Directory Server
                    searchBase = settings.LdapDomain; //openldap is consistent, and the same base DN is used for searches and logins
                    break;
                default:                        //Active Directory (using sAMAccountName)
                    var domainParts2 = settings.LdapDomain.Split('.');
                    foreach(var part in domainParts2) searchBase += $"dc={part},";
                    searchBase = "cn=users," + searchBase.TrimEnd(',');
                    break;
            }
            return searchBase;
        }

        private LdapUser ValidateUser(ILdapSettings settings, string username, string password)
        {
            var userDn = makeUserDn(settings, username);
            var user = new LdapUser();
            bool getLdapUserDetails = _configuration.GetValue<bool>("LdapOptions:GetLdapUserDetails", false);

            //determine which LDAP server to use
            string[] servers = settings.LdapServer.Split(',');
            string memCacheKey = $"LdapActiveConnection_{settings.Id}"; //support for multi-tenancy
            int activeConnection = _memoryCache.TryGetValue<int>(memCacheKey, out activeConnection) ? activeConnection : 0;
            string activeServer = servers[activeConnection].Trim();     //the current host/ip we will use

            int tryCount = 0;
            while (tryCount < servers.Length) //only try each server in the list once
            {
                string message = $"Querying LDAP server: {activeServer} for {userDn} -";
                try
                {
                    using (var connection = GetConnection(activeServer, settings.LdapPort, settings.LdapUseSsl))
                    {
                        connection.Bind(userDn, password);
                        if (connection.Bound)
                        {
                            _log.LogInformation($"{message} bind succeeded");
                            _memoryCache.Set(memCacheKey, activeConnection);
                            if (getLdapUserDetails)
                            {
                                LdapEntry entry = GetOneUserEntry(connection, settings, username);
                                if (entry != null) user = BuildUserFromEntry(entry);
                            }
                            user.ResultStatus = "PASS";
                            connection.Disconnect();
                            return user;
                        }
                        else // I don't think this is ever triggered as an exception is thrown if the bind fails?
                        {
                            _log.LogWarning($"{message} bind failed");
                            _memoryCache.Set(memCacheKey, activeConnection);
                            user.ResultStatus = "Bind Failed";
                            connection.Disconnect();
                            return user;
                        }
                    }
                }
                catch (Exception ex)
                {
                    switch(ex.Message)
                    {
                        case "Invalid Credentials":
                        case "Unwilling To Perform":
                            _log.LogWarning($"{message} bind failed. The exception was:\n{ex.Message}:{ex.StackTrace}");
                            _memoryCache.Set(memCacheKey, activeConnection);
                            user.ResultStatus = "Bind Failed";
                            return user;
                        case "Connect Error":
                            _log.LogError($"{message} connect to LDAP server failed. The exception was:\n{ex.Message}:{ex.StackTrace}");
                            break;
                        default:
                            _log.LogError($"{message} the exception was:\n{ex.Message}:\n{ex.StackTrace}");
                            break;
                    }
                }

                activeConnection = (activeConnection + 1) % servers.Length;
                activeServer = servers[activeConnection].Trim();
                tryCount++;
            }
            _log.LogError($"All LDAP servers failed for {userDn}");
            user.ResultStatus = "Connect Error";
            return user;
        }

        private LdapConnection GetConnection(string server, int port, bool useSsl = false)
        {
            LdapConnection conn = new LdapConnection();

            if (useSsl)
            {
                // make this support ssl/tls
                //http://stackoverflow.com/questions/386982/novell-ldap-c-novell-directory-ldap-has-anybody-made-it-work
                conn.SecureSocketLayer = true;
                conn.UserDefinedServerCertValidationDelegate += LdapSSLCertificateValidator;
            }

            conn.Connect(server, port);
            return conn;
        }


        private bool LdapSSLCertificateValidator(
           object sender,
           X509Certificate certificate,
           X509Chain chain,
           SslPolicyErrors sslPolicyErrors)
        {
            return _ldapSslCertificateValidator.ValidateCertificate(
                sender,
                certificate,
                chain,
                sslPolicyErrors
                );

        }

        private LdapEntry GetOneUserEntry(
            LdapConnection conn,
            ILdapSettings ldapSettings,
            string username)
        {
            string baseDn = makeSearchBaseDn(ldapSettings, username);
            string userDn = makeUserDn(ldapSettings, username);
            string filter = makeUserFilter(ldapSettings, username);
            LdapEntry entry = null;

            // Console.ForegroundColor = ConsoleColor.Yellow;
            // Console.WriteLine($"Searching for UserDN: {userDn} in BaseDN: {baseDn} with filter: {filter}");
            // Console.ResetColor();

            var lsc = conn.Search(
                baseDn,
                LdapConnection.ScopeSub,
                filter,
                null,
                false
                );

            while (lsc.HasMore())
            {
                try
                {
                    entry = lsc.Next();
                    // Console.WriteLine(entry.ToString());
                    if(entry.Dn == userDn) return entry;
                }
                catch (LdapException e)
                {
                    continue;
                }
            }
            return entry;
        }

        private LdapUser BuildUserFromEntry(LdapEntry entry)
        {
            var user = new LdapUser();
            LdapAttributeSet las = entry.GetAttributeSet();

            foreach (LdapAttribute a in las)
            {
                switch (a.Name)
                {
                    case "mail":
                        user.Email = a.StringValue;
                        break;
                    case "cn":
                        user.CommonName = a.StringValue;
                        break;
                    case "givenName":
                        user.FirstName = a.StringValue;
                        break;
                    case "sn":
                        user.LastName = a.StringValue;
                        break;
                    case "displayName":
                        user.DisplayName = a.StringValue;
                        break;
                }
                if(string.IsNullOrEmpty(user.FirstName)) user.FirstName = user.CommonName;
            }

            return user;
        }
    }
}
