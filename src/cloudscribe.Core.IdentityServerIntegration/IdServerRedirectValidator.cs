using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration
{
    /// <summary>
    /// jk - 
    /// For debug purposes: an override of the URI Validator used by IS4.
    /// 
    /// Not used in production - but useful if you're having difficulties with the redirect URIs.
    /// 
    /// Wire-up:
    ///  services.AddTransient<IRedirectUriValidator, IdServerRedirectValidator>();
    /// </summary>
    public class IdServerRedirectValidator : StrictRedirectUriValidator, IRedirectUriValidator
    {
        public override async Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
        {
            //TODO: change this to check the client id
            if (requestedUri == "xamarinformsclients://callback")
            {
                return true;
            }

            if (requestedUri == "https://notused")
            {
                return true;
            }

            // var istrue = client.PostLogoutRedirectUris.Contains(requestedUri, StringComparer.OrdinalIgnoreCase);

            var check = await base.IsPostLogoutRedirectUriValidAsync(requestedUri, client);
            return check;
        }


        public override async Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
        {
            //TODO: change this to check the client id
            if (requestedUri == "xamarinformsclients://callback")
            {
                return true;
            }

            if (requestedUri == "https://notused")
            {
                return true;
            }

            var check = await base.IsRedirectUriValidAsync(requestedUri, client);
            return check;
        }
    }
}
