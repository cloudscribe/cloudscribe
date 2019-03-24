using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Identity;
using Microsoft.Extensions.Logging;
using Novell.Directory.Ldap;
using System;
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
            ILogger<LdapHelper> logger
            )
        {
            _ldapSslCertificateValidator = ldapSslCertificateValidator;
            _log = logger;
        }

        private readonly ILdapSslCertificateValidator _ldapSslCertificateValidator;
        private readonly ILogger _log;

        public bool IsImplemented { get; } = true;
        

        public Task<LdapUser> TryLdapLogin(ILdapSettings ldapSettings, string userName, string password)
        {
            LdapUser user = null;
            
            var isValid = ValidateUser(ldapSettings, userName, password);
            
            if (isValid)
            {
                user = new LdapUser()
                {
                    CommonName = userName
                };
            }
            

            return Task.FromResult(user);
        }

        private bool ValidateUser(
            ILdapSettings settings,
            string username,
            string password)
        {
            string userDn;
            switch (settings.LdapUserDNFormat)
            {
                case "username@LDAPDOMAIN":
                    userDn = $"{username}@{settings.LdapDomain}";
                    break;
                default:
                    userDn = $"{settings.LdapDomain}\\{username}";
                    break;
            }
            
            //string userDn = $"{settings.LdapUserDNKey}={username}";
            try
            {
                using (var connection = GetConnection(settings, settings.LdapUseSsl))
                {
                    connection.Bind(userDn, password);

                    if (connection.Bound)
                        return true;
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"{ex.Message}:{ex.StackTrace}");
            }
            return false;
        }

        

        private LdapConnection GetConnection(ILdapSettings ldapSettings, bool useSsl = false)
        {
            LdapConnection conn = new LdapConnection();
            
           
            if (useSsl)
            {
                // make this support ssl/tls
                //http://stackoverflow.com/questions/386982/novell-ldap-c-novell-directory-ldap-has-anybody-made-it-work
                conn.SecureSocketLayer = true;
                conn.UserDefinedServerCertValidationDelegate += LdapSSLCertificateValidator;

            }
           
            conn.Connect(ldapSettings.LdapServer, ldapSettings.LdapPort);

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


        //private LdapUser BuildUserFromEntry(LdapEntry entry)
        //{
        //    var user = new LdapUser();

        //    LdapAttributeSet las = entry.getAttributeSet();

        //    foreach (LdapAttribute a in las)
        //    {
        //        switch (a.Name)
        //        {
        //            case "mail":
        //                user.Email = a.StringValue;
        //                break;
        //            case "cn":
        //                user.CommonName = a.StringValue;
        //                break;
        //            case "userPassword":
        //                // this.password = a;
        //                break;
        //            case "uidNumber":
        //                //this.uidNumber = a;
        //                break;
        //            case "uid":
        //                // this.userid = a;
        //                break;
        //            case "sAMAccountName":
        //                // this.userid = a;
        //                break;
        //            case "givenName":
        //                user.FirstName = a.StringValue;
        //                break;
        //            case "sn":
        //                user.LastName = a.StringValue;
        //                break;
        //        }
        //    }

        //    return user;
        //}


        //private LdapUser LdapStandardLogin(ILdapSettings ldapSettings, string userName, string password, bool useSsl)
        //{
        //    bool success = false;
        //    LdapUser user = null;

        //    LdapConnection conn = null;
        //    try
        //    {
        //        using (conn = GetConnection(ldapSettings, useSsl))
        //        {

        //            if ((conn != null) && (conn.Connected))
        //            {
        //                LdapEntry entry = null;

        //                try
        //                {
        //                    entry = GetOneUserEntry(conn, ldapSettings, userName);
        //                    if (entry != null)
        //                    {
        //                        conn.Bind(entry.DN, password);
        //                        success = true;
        //                    }
        //                    else
        //                    {
        //                        _log.LogWarning($"could not find entry for {userName}");
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    string msg = $"Login failure for user: {userName} Exception: {ex.Message}:{ex.StackTrace}";
        //                    _log.LogError(msg);

        //                    success = false;
        //                }

        //                if (success)
        //                {
        //                    if (entry != null)
        //                    {
        //                        user = BuildUserFromEntry(entry);
        //                    }
        //                }

        //                conn.Disconnect();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = $"Login failure for user: {userName} Exception: {ex.Message}:{ex.StackTrace}";
        //        _log.LogError(msg);

        //    }

        //    return user;
        //}

        //private LdapEntry GetOneUserEntry(
        //    LdapConnection conn,
        //    ILdapSettings ldapSettings,
        //    string userName)
        //{

        //    LdapSearchConstraints constraints = new LdapSearchConstraints();

        //    var filter = "(&(sAMAccountName=" + userName + "))";
        //    //ldapSettings.LdapUserDNKey + "=" + search,

        //    LdapSearchQueue queue = null;
        //    queue = conn.Search(
        //        ldapSettings.LdapRootDN,
        //        LdapConnection.SCOPE_SUB,
        //        filter,
        //        null,
        //        false,
        //        (LdapSearchQueue)null,
        //        (LdapSearchConstraints)null
        //        );



        //    LdapEntry entry = null;

        //    if (queue != null)
        //    {
        //        LdapMessage message = queue.getResponse();
        //        if (message != null)
        //        {
        //            if (message is LdapSearchResult)
        //            {
        //                entry = ((LdapSearchResult)message).Entry;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        _log.LogWarning("queue was null");
        //    }

        //    return entry;
        //}



    }
}
