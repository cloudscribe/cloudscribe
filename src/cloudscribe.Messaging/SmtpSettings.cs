// Author:				Joe Audette
// Created:			    2008-09-12
// Last Modified:		2014-08-22
// 
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Messaging
{
    public class SmtpSettings
    {
        public SmtpSettings()
        { }

        private string user = string.Empty;

        public string User
        {
            get { return user; }
            set { user = value; }
        }

        private string password = string.Empty;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string server = string.Empty;

        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        private int port = 25;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        private bool requiresAuthentication = false;

        public bool RequiresAuthentication
        {
            get { return requiresAuthentication; }
            set { requiresAuthentication = value; }
        }

        private bool useSsl = false;

        public bool UseSsl
        {
            get { return useSsl; }
            set { useSsl = value; }
        }

        private string preferredEncoding = string.Empty;

        public string PreferredEncoding
        {
            get { return preferredEncoding; }
            set { preferredEncoding = value; }
        }

        private bool addBulkMailHeader = false;

        public bool AddBulkMailHeader
        {
            get { return addBulkMailHeader; }
            set { addBulkMailHeader = value; }
        }

        public bool IsValid
        {
            get
            {
                if (server.Length == 0) { return false; }

                if (requiresAuthentication)
                {
                    if (user.Length == 0) { return false; }
                    if (password.Length == 0) { return false; }
                }

                return true;
            }

        }


    }
}
