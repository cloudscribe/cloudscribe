using System;

namespace cloudscribe.Core.Models
{
    public interface IUserInteractiveServiceToken
    {
        Guid Id { get; set; }
        /// <summary>
        /// This is the site id of the site that will use the token
        /// </summary>
        Guid SiteId { get; set; }
        /// <summary>
        /// Which part of cloudscribe needs this token? e.g. cloudscribe.Email.Senders.SmtpOAuth
        /// </summary>
        string CloudscribeServiceProvider { get; set; }
        /// <summary>
        /// This is the user that will use the token in a service based capacity. Usually an email address.
        /// </summary>
        string UserPrincipalName { get; set; }
        /// <summary>
        /// This is the token that will be used by the service to authenticate as the user.
        /// </summary>
        string SecureToken { get; set; }
        /// <summary>
        /// A hint at the expiry date of the token. If expired we need to renew it by using the refresh token.
        /// </summary>
        DateTime? TokenExpiresUtc { get; set; }
        /// <summary>
        /// A flag we can set if the access token has expired and we can no longer refresh it.
        /// </summary>
        bool TokenHasExpired { get; set; }
    }
}