using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Threading.Tasks;

namespace cloudscribe.Core.Ldap.Windows
{
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

        //private bool useRootDn = false;

        // this implementation assumes only one server is defined in settings.ldapServer
        public Task<LdapUser> TryLdapLogin(
            ILdapSettings ldapSettings,
            string userName,
            string password
        )
        {
            bool success = false;
            LdapUser user = null;
            DirectoryEntry directoryEntry = null;

            try
            {

                //if (useRootDn)
                //{
                //    directoryEntry = new DirectoryEntry("LDAP://" + ldapSettings.LdapServer + "/" + ldapSettings.LdapRootDN, ldapSettings.LdapDomain + "\\" + userName, password);
                //}
                //else
                //{
                //directoryEntry = new DirectoryEntry("LDAP://" + ldapSettings.LdapServer, ldapSettings.LdapDomain + "\\" + userName, password);
                //}

                if(ldapSettings.LdapUseSsl)
                {
                    directoryEntry = new DirectoryEntry("LDAP://" + ldapSettings.LdapServer, ldapSettings.LdapDomain + "\\" + userName, password, AuthenticationTypes.SecureSocketsLayer);
                }
                else
                {
                    directoryEntry = new DirectoryEntry("LDAP://" + ldapSettings.LdapServer, ldapSettings.LdapDomain + "\\" + userName, password);
                }

            }
            catch (Exception ex)
            {
                string msg = $"Login failure for user: {userName} Exception: {ex.Message}:{ex.StackTrace}";
                _log.LogError(msg);
                user.ResultStatus = "Invalid Credentials";
            }
            if (directoryEntry != null)
            {
                //Bind to the native AdsObject to force authentication.
                try
                {
                    object testobj = directoryEntry.NativeObject;
                    success = true;
                }
                catch (Exception ex)
                {
                    string msg = $"Login failure for user: {userName} Exception: {ex.Message}:{ex.StackTrace}";
                    _log.LogError(msg);

                    success = false;
                    user.ResultStatus = "Invalid Credentials";
                }

                if (success && directoryEntry != null)
                {
                    user = GetLdapUser(directoryEntry, ldapSettings, userName);
                    user.ResultStatus = "PASS";
                }
            }

            return Task.FromResult(user);
        }

        // this implementation assumes only one server is defined in settings.ldapServer
        public Task<Dictionary<string,LdapUser>> TestLdapServers(
            ILdapSettings settings,
            string username,
            string password
        )
        {
            string message;
            var ldapUser = TryLdapLogin(settings, username, password).Result;
            if(ldapUser != null)
            {
                message = "PASS";
            }
            else
            {
                message = "Invalid Credentials";
            }

            var result = new Dictionary<string, LdapUser>();
            var user = new LdapUser();
            user.ResultStatus = message;
            result.Add(settings.LdapServer, user);
            return Task.FromResult(result);
        }

        private LdapUser GetLdapUser(DirectoryEntry directoryEntry, ILdapSettings ldapSettings, string userName)
        {

            DirectorySearcher ds = new DirectorySearcher(directoryEntry);
            ds.Filter = "(&(sAMAccountName=" + userName + "))";
            SearchResult result = ds.FindOne();
            DirectoryEntry ent = null;

            if (result != null)
            {
                ent = result.GetDirectoryEntry();
            }

            if (ent != null)
            {
                var user = new LdapUser();

                if (ent.Properties["cn"].Value != null)
                {
                    user.CommonName = ent.Properties["cn"].Value.ToString();
                }
                else
                {
                    user.CommonName = userName;
                }
                if (ent.Properties["mail"].Value != null)
                {
                    user.Email = ent.Properties["mail"].Value.ToString();
                }


                return user;
            }


            return null;
        }

    }
}
