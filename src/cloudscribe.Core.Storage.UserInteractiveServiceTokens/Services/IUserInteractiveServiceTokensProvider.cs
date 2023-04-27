using cloudscribe.Core.Models;

namespace cloudscribe.Core.Storage.UserInteractiveServiceTokens
{
    public interface IUserInteractiveServiceTokensProvider
    {
        Task<string> GetInteractiveAuthenticationUriAsync(
            string oAuthAuthorizeEndpointUri,
            string oAuthClientId,
            string oauthScopesCsv,
            string userPrincipalName,
            string returnRedirectUri,
            Guid siteId,
            string cloudscribeServiceProvider
        );

        Task<string?> AquireTokenFromOAuthCodeAsync(
            string oAuthTokenEndpointUri,
            string oAuthClientId,
            string oAuthClientSecret,
            string oAuthScopesCsv,
            string oAuthCode,
            string oAuthRedirectUri,
            string randomStateGuid,
            string userPrincipalName,
            Guid siteId,
            string cloudscribeServiceProvider
        );

        Task<string?> GetAccessTokenAsync(
            Guid siteId,
            string userPrincipalName,
            string cloudscribeServiceProvider,
            string oAuthTokenEndpointUri,
            string oAuthClientId,
            string oAuthClientSecret
        );

        Task<bool> DeleteAccessTokenAsync(
            string siteId,
            string userPrincipalName,
            string cloudscribeServiceProvider
        );
    }
}