using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace cloudscribe.Core.Identity
{
    public class LoginResultTemplate
    {
        public bool MustAcceptTerms { get; set; }
        public bool NeedsAccountApproval { get; set; }
        public bool NeedsEmailConfirmation { get; set; }
        public string EmailConfirmationToken { get; set; } = string.Empty;
        public bool NeedsPhoneConfirmation { get; set; }
        public ExternalLoginInfo ExternalLoginInfo { get; set; } = null;
        public List<string> RejectReasons { get; set; } = new List<string>();
        public SiteUser User { get; set; } = null;
        public SignInResult SignInResult { get; set; } = SignInResult.Failed;
    }
}
