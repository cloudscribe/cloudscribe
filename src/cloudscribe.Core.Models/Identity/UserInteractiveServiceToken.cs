using System;

namespace cloudscribe.Core.Models
{
    public class UserInteractiveServiceToken : IUserInteractiveServiceToken
    {
        public Guid Id { get; set; }
        /// <summary>
        /// This is the site id of the site that will use the token
        /// </summary>
        public Guid SiteId { get; set; }
        /// <summary>
        /// Which part of cloudscribe needs this token? e.g. cloudscribe.Email.Senders.SmtpOAuth
        /// </summary>
        public string CloudscribeServiceProvider { get; set; }
        /// <summary>
        /// This is the user that will use the token in a service based capacity. Usually an email address.
        /// </summary>
        public string UserPrincipalName { get; set; }
        /// <summary>
        /// This is the token that will be used by the service to authenticate as the user.
        /// </summary>
        public string SecureToken { get; set; }
        /// <summary>
        /// A hint at the expiry date of the token. If expired we need to renew it by using the refresh token.
        /// </summary>
        public DateTime? TokenExpiresUtc { get; set; }
        /// <summary>
        /// A flag we can set if the access token has expired and we can no longer refresh it.
        /// </summary>
        public bool TokenHasExpired { get; set; }
    }
}