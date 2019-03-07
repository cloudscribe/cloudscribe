using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Identity;
using Microsoft.Extensions.Logging;
using Novell.Directory.Ldap;
using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace cloudscribe.Core.Ldap
{
    //https://stackoverflow.com/questions/49682644/asp-net-core-2-0-ldap-active-directory-authentication

    public class LdapHelper : ILdapHelper
    {
        public LdapHelper(
            ILogger<LdapHelper> logger
            )
        {
            _log = logger;
        }

        private readonly ILogger _log;

        public bool IsImplemented { get; } = true;

        public bool ValidateUser(
            ILdapSettings settings, 
            string username, 
            string password)
        {
            string userDn = $"{username}@{settings.LdapDomain}";
            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect(settings.LdapDomain, settings.LdapPort);
                    connection.Bind(userDn, password);

                    if (connection.Bound)
                        return true;
                }
            }
            catch (LdapException ex)
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
                conn.UserDefinedServerCertValidationDelegate += LdapSSLHandler;

            }

            conn.Connect(ldapSettings.LdapServer, ldapSettings.LdapPort);

            return conn;
        }


        private LdapUser LdapStandardLogin(ILdapSettings ldapSettings, string uid, string password, bool useSsl = false)
        {
            bool success = false;
            LdapUser user = null;

            LdapConnection conn = null;
            try
            {
                using (conn = GetConnection(ldapSettings, useSsl))
                {

                    if ((conn != null) && (conn.Connected))
                    {
                        LdapEntry entry = null;

                        try
                        {
                            entry = GetOneUserEntry(conn, ldapSettings, uid);
                            if (entry != null)
                            {
                                LdapConnection authConn = GetConnection(ldapSettings);
                                authConn.Bind(entry.DN, password);
                                authConn.Disconnect();
                                success = true;

                            }
                        }
                        catch (LdapException ex)
                        {
                            string msg = $"Login failure for user: {uid} Exception: {ex.Message}:{ex.StackTrace}";
                            _log.LogError(msg);

                            success = false;
                        }

                        if (success)
                        {
                            if (entry != null)
                            {
                                user = BuildUserFromEntry(entry);
                            }
                        }

                        conn.Disconnect();
                    }
                }
            }
            catch (SocketException ex)
            {
                string msg = $"Login failure for user: {uid} Exception: {ex.Message}:{ex.StackTrace}";
                _log.LogError(msg);
                
            }
            
            return user;
        }

        private LdapUser BuildUserFromEntry(LdapEntry entry)
        {
            var user = new LdapUser();

            LdapAttributeSet las = entry.getAttributeSet();

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
                    case "userPassword":
                       // this.password = a;
                        break;
                    case "uidNumber":
                        //this.uidNumber = a;
                        break;
                    case "uid":
                       // this.userid = a;
                        break;
                    case "sAMAccountName":
                       // this.userid = a;
                        break;
                    case "givenName":
                        user.FirstName = a.StringValue;
                        break;
                    case "sn":
                        user.LastName = a.StringValue;
                        break;
                }
            }
            
            return user;
        }




        private bool ValidateUserWithSsl(
            ILdapSettings settings,
            string username,
            string password)
        {
            string userDn = $"{username}@{settings.LdapDomain}";
            try
            {
                using (var connection = GetConnection(settings, true))
                {
                    connection.UserDefinedServerCertValidationDelegate += LdapSSLHandler;
                    connection.Connect(settings.LdapDomain, settings.LdapPort);
                    connection.Bind(userDn, password);

                    if (connection.Bound)
                        return true;
                }
            }
            catch (LdapException ex)
            {
                _log.LogError($"{ex.Message}:{ex.StackTrace}");
            }
            return false;
        }

        private bool ValidateUserNoSsl(
            ILdapSettings settings,
            string username,
            string password)
        {
            string userDn = $"{username}@{settings.LdapDomain}";
            try
            {
                using (var connection = GetConnection(settings, false))
                {
                    connection.Connect(settings.LdapDomain, settings.LdapPort);
                    connection.Bind(userDn, password);

                    if (connection.Bound)
                        return true;
                }
            }
            catch (LdapException ex)
            {
                _log.LogError($"{ex.Message}:{ex.StackTrace}");
            }
            return false;
        }

        private LdapEntry GetOneUserEntry(
            LdapConnection conn,
            ILdapSettings ldapSettings,
            string search)
        {

            LdapSearchConstraints constraints = new LdapSearchConstraints();

            LdapSearchQueue queue = null;
            queue = conn.Search(
                ldapSettings.LdapRootDN,
                LdapConnection.SCOPE_SUB,
                ldapSettings.LdapUserDNKey + "=" + search,
                null,
                false,
                (LdapSearchQueue)null,
                (LdapSearchConstraints)null);

            LdapEntry entry = null;

            if (queue != null)
            {
                LdapMessage message = queue.getResponse();
                if (message != null)
                {
                    if (message is LdapSearchResult)
                    {
                        entry = ((LdapSearchResult)message).Entry;
                    }
                }
            }

            return entry;
        }



        private bool LdapSSLHandler(
            object sender, 
            X509Certificate certificate, 
            X509Chain chain, 
            SslPolicyErrors sslPolicyErrors)
        {

//#if !MONO
//            X509Store store = null;
//            X509Stores stores = X509StoreManager.LocalMachine;
//            store = stores.TrustedRoot;
//            X509Certificate x509 = null;

//            byte[] data = certificate.GetRawCertData();
//            if (data != null) { x509 = new X509Certificate(data); }

//            if (x509 != null)
//            {
//                //coll.Add(x509);
//                if (!store.Certificates.Contains(x509))
//                {

//                    store.Import(x509);
//                }

//            }
//#endif

            return true;
        }

    }
}
