﻿using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public interface IOidcHybridFlowHelper
    {
        //Task CaptureJwt(ClaimsPrincipal principal, string jwt);
        //Task<string> GetCurrentJwt(ClaimsPrincipal principal);

        Task CaptureOidcTokens(ClaimsPrincipal principal, List<AuthenticationToken> tokens);

        Task<List<AuthenticationToken>> GetOidcTokens(ClaimsPrincipal principal);


    }

    public class NoopOidcHybridFlowHelper : IOidcHybridFlowHelper
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



        //public Task CaptureJwt(ClaimsPrincipal principal, string jwt)
        //{
        //    return Task.CompletedTask;
        //}

        //public Task<string> GetCurrentJwt(ClaimsPrincipal principal)
        //{
        //    string result = null;
        //    return Task.FromResult(result);
        //}
    }
}
