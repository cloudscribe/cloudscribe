using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public interface ICaptureOidcTokens
    {
        Task CaptureOidcTokens(ClaimsPrincipal principal, List<AuthenticationToken> tokens);

        Task<List<AuthenticationToken>> GetOidcTokens(ClaimsPrincipal principal);

    }

    public class NoopOidcTokenCapture : ICaptureOidcTokens
    {
        public Task CaptureOidcTokens(ClaimsPrincipal principal, List<AuthenticationToken> tokens)
        {
            return Task.CompletedTask;
        }

        public Task<List<AuthenticationToken>> GetOidcTokens(ClaimsPrincipal principal)
        {
            var result = new List<AuthenticationToken>();
            
            return Task.FromResult(result);
        }

    }
}
