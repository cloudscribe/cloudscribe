using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public interface IOidcHybridFlowHelper
    {
        Task CaptureJwt(ClaimsPrincipal principal, string jwt);
        Task<string> GetCurrentJwt(ClaimsPrincipal principal);
    }

    public class NoopOidcHybridFlowHelper : IOidcHybridFlowHelper
    {
        public Task CaptureJwt(ClaimsPrincipal principal, string jwt)
        {
            return Task.CompletedTask;
        }

        public Task<string> GetCurrentJwt(ClaimsPrincipal principal)
        {
            string result = null;
            return Task.FromResult(result);
        }
    }
}
