using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class TwoFactorAuthenticationViewModel
    {
        public bool HasAuthenticator { get; set; }

        public int RecoveryCodesLeft { get; set; }

        public bool Is2faEnabled { get; set; }
    }
}
