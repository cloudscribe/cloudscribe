using System.Text.Json;
using cloudscribe.Core.Models;
using cloudscribe.Core.Storage.EFCore.Common;

namespace cloudscribe.Core.Storage.UserInteractiveServiceTokens
{
    public class UserInteractiveServiceTokensProvider : IUserInteractiveServiceTokensProvider
    {

        public UserInteractiveServiceTokensProvider(
            ICoreDbContextFactory dbContextFactory
        )
        {
            _contextFactory = dbContextFactory;
        }

        private readonly ICoreDbContextFactory _contextFactory;

        //Make a redirect Uri to an auth provider's login page
        //e.g. https://login.microsoftonline.com/{tenant}/oauth2/v2.0/authorize?
        // client_id=11111111-1111-1111-1111-111111111111
        // &response_type=code&redirect_uri=http%3A%2F%2Flocalhost%2Fmyapp%2F
        // &response_mode=query&scope=offline_access%20user.read%20mail.read&state=12345
        public async Task<string> GetInteractiveAuthenticationUriAsync(
            string oAuthAuthorizeEndpointUri,
            string oAuthClientId,
            string oauthScopesCsv,
            string userPrincipalName,
            string returnRedirectUri,
            Guid siteId,
            string cloudscribeServiceProvider
        )
        {
            var randomGuid = Guid.NewGuid().ToString();
            UserInteractiveServiceToken? tokenStore = null;

            using(var dbContext= _contextFactory.CreateContext())
            {
                tokenStore = dbContext.UserInteractiveServiceTokens.SingleOrDefault(
                    x => x.SiteId == siteId
                    && x.UserPrincipalName == userPrincipalName
                    && x.CloudscribeServiceProvider == cloudscribeServiceProvider
                );

                if (tokenStore == null)
                {
                    tokenStore = new UserInteractiveServiceToken
                    {
                        SiteId = siteId,
                        CloudscribeServiceProvider = cloudscribeServiceProvider,
                        UserPrincipalName = userPrincipalName,
                        SecureToken = randomGuid,
                        TokenExpiresUtc = DateTime.UtcNow,
                        TokenHasExpired = true
                    };
                    dbContext.UserInteractiveServiceTokens.Add(tokenStore);

                }
                else
                {
                    tokenStore.SecureToken = randomGuid;
                    tokenStore.TokenExpiresUtc = DateTime.UtcNow;
                    tokenStore.TokenHasExpired = true;
                }
                await dbContext.SaveChangesAsync();
            }

            oauthScopesCsv = oauthScopesCsv.Replace(",", " ");
            var state = siteId + "_" + randomGuid;
            var redirectTo = new UriBuilder(oAuthAuthorizeEndpointUri);
            redirectTo.Query = "client_id=" + oAuthClientId
                + "&response_type=code"
                + "&redirect_uri=" + returnRedirectUri
                + "&response_mode=form_post"
                + "&scope=" + oauthScopesCsv
                + "&state=" + state
                + "&access_type=offline"                    //for google
                + "&login_hint=" + userPrincipalName;       //for google + microsoft
            return redirectTo.ToString();
        }

        public async Task<string?> AquireTokenFromOAuthCodeAsync(
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
        )
        {
            string? token = null;
            UserInteractiveServiceToken? tokenStore = null;
            using ( var dbContext = _contextFactory.CreateContext())
            {
                tokenStore = dbContext.UserInteractiveServiceTokens.SingleOrDefault(
                    x => x.SiteId == siteId
                    && x.UserPrincipalName == userPrincipalName
                    && x.CloudscribeServiceProvider == cloudscribeServiceProvider
                );
                if (tokenStore == null)
                {
                    throw new Exception($"Token Store not found for siteId: {siteId} userPrincipalName: {userPrincipalName} cloudscribeServiceProvider: {cloudscribeServiceProvider}");
                }
                if (tokenStore.SecureToken != randomStateGuid) {
                    throw new Exception($"Invalid OAuth return state. The randomStateGuid check does not match.");
                }

                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, oAuthTokenEndpointUri);
                tokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "client_id", oAuthClientId },
                    { "scope", oAuthScopesCsv },
                    { "code", oAuthCode },
                    { "redirect_uri", oAuthRedirectUri },
                    { "grant_type", "authorization_code" },
                    { "client_secret", oAuthClientSecret }
                });

                string? responseString = null;

                using(var client = new HttpClient())
                {
                    var response = await client.SendAsync(tokenRequest);
                    responseString = await response.Content.ReadAsStringAsync();
                }

                //store the entire response token in the db (todo: encrypt?)
                tokenStore.SecureToken = responseString;
                await dbContext.SaveChangesAsync();

                var jsonDoc = JsonDocument.Parse(responseString);
                var tokenObj = jsonDoc.RootElement;
                var missing = new List<string>();
                if(!tokenObj.TryGetProperty("token_type", out var tokenType)) missing.Add("token_type");
                if(!tokenObj.TryGetProperty("scope", out var scope)) missing.Add("scope");
                if(!tokenObj.TryGetProperty("expires_in", out var expiresInSeconds)) missing.Add("expires_in");
                if(!tokenObj.TryGetProperty("access_token", out var accessToken)) missing.Add("access_token");
                if(!tokenObj.TryGetProperty("refresh_token", out var refreshToken)) missing.Add("refresh_token");
                if(missing.Count > 0)
                {
                    throw new Exception(
                        "Authorization response validation failed",
                        new Exception(
                            "The following properties were missing from the response:\n" +
                            string.Join(", ", missing) + "\nThe response returned was:\n" +
                            responseString
                        )
                    );
                }
                if(tokenObj.TryGetProperty("error", out var error))
                {
                    throw new Exception(
                        "Authorization response validation failed",
                        new Exception(
                            "The following error was returned:\n" + error.GetString() + "\n" + responseString
                        )
                    );
                }
                token = accessToken.GetString();
                tokenStore.TokenExpiresUtc = DateTime.UtcNow.AddSeconds(expiresInSeconds.GetInt32());
                tokenStore.TokenHasExpired = false;
                await dbContext.SaveChangesAsync();
            }
            return token;
        }

        public async Task<string?> GetAccessTokenAsync(
            Guid siteId,
            string userPrincipalName,
            string cloudscribeServiceProvider,
            string oAuthTokenEndpointUri,
            string oAuthClientId,
            string oAuthClientSecret
        )
        {
            string? token = null;
            UserInteractiveServiceToken? tokenStore = null;

            using ( var dbContext = _contextFactory.CreateContext())
            {
                tokenStore = dbContext.UserInteractiveServiceTokens.SingleOrDefault(
                    x => x.SiteId == siteId
                    && x.UserPrincipalName == userPrincipalName
                    && x.CloudscribeServiceProvider == cloudscribeServiceProvider
                );
                if (tokenStore == null) return token;
                if (tokenStore.TokenHasExpired) return token;

                var jsonDoc = JsonDocument.Parse(tokenStore.SecureToken);
                var tokenObj = jsonDoc.RootElement;
                var missing = new List<string>();
                if(!tokenObj.TryGetProperty("token_type", out var tokenType)) missing.Add("token_type");
                if(!tokenObj.TryGetProperty("scope", out var scope)) missing.Add("scope");
                if(!tokenObj.TryGetProperty("expires_in", out var expiresInSeconds)) missing.Add("expires_in");
                if(!tokenObj.TryGetProperty("access_token", out var accessToken)) missing.Add("access_token");
                if(!tokenObj.TryGetProperty("refresh_token", out var refreshToken)) missing.Add("refresh_token");
                if(missing.Count > 0) return token;

                //do we need to use the refresh token? No.
                if (tokenStore.TokenExpiresUtc > DateTime.UtcNow)
                {
                    token = accessToken.GetString();
                    return token;
                }

                //use the refresh token to get and store a new token.
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, oAuthTokenEndpointUri);
                tokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "client_id", oAuthClientId },
                    { "client_secret", oAuthClientSecret },
                    { "refresh_token", refreshToken.ToString() },
                    { "grant_type", "refresh_token" }
                });

                HttpResponseMessage? response = null;
                string? responseString = null;
                using(var client = new HttpClient())
                {
                    response = await client.SendAsync(tokenRequest);
                    responseString = await response.Content.ReadAsStringAsync();
                }

                if (response.IsSuccessStatusCode)
                {
                    bool failed = false;
                    var json = JsonDocument.Parse(responseString);
                    var newTokenObj = json.RootElement;
                    missing = new List<string>();
                    if(!newTokenObj.TryGetProperty("token_type", out var newTokenType)) missing.Add("token_type");
                    if(!newTokenObj.TryGetProperty("scope", out var newScope)) missing.Add("scope");
                    if(!newTokenObj.TryGetProperty("access_token", out var newAccessToken)) missing.Add("access_token");
                    if(!newTokenObj.TryGetProperty("expires_in", out var newExpiresInSeconds)) missing.Add("expires_in");
                    if(!newTokenObj.TryGetProperty("refresh_token", out var newRefreshToken)) missing.Add("refresh_token");
                    if(missing.Count > 0)
                    {
                        tokenStore.SecureToken = null;
                        tokenStore.TokenHasExpired = true;
                        tokenStore.TokenExpiresUtc = null;
                        failed = true;
                    }
                    if(newTokenObj.TryGetProperty("error", out var error))
                    {
                        tokenStore.SecureToken = null;
                        tokenStore.TokenHasExpired = true;
                        tokenStore.TokenExpiresUtc = null;
                        failed = true;
                    }
                    if(failed) return token;

                    var newExpires = DateTime.UtcNow.AddSeconds(newExpiresInSeconds.GetInt32());
                    tokenStore.SecureToken = responseString;
                    tokenStore.TokenExpiresUtc = newExpires;
                    tokenStore.TokenHasExpired = false;
                    token = newAccessToken.ToString();
                    await dbContext.SaveChangesAsync();
                }

            }
            return token;
        }


        public async Task<bool> DeleteAccessTokenAsync(
            string siteId,
            string userPrincipalName,
            string cloudscribeServiceProvider)
        {
            throw new NotImplementedException();
        }

    }
}
