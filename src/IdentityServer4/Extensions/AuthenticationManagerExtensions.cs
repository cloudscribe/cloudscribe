// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http.Authentication
{
    public static class AuthenticationManagerExtensions
    {
        public static async Task SignInAsync(this AuthenticationManager manager, string subject, string name, params Claim[] claims)
        {
            var scheme = manager.GetIdentityServerAuthenticationScheme();

            var principal = IdentityServerPrincipal.Create(subject, name, claims);
            await manager.SignInAsync(scheme, principal);
        }

        public static async Task SignInAsync(this AuthenticationManager manager, string subject, string name, AuthenticationProperties properties, params Claim[] claims)
        {
            var scheme = manager.GetIdentityServerAuthenticationScheme();

            var principal = IdentityServerPrincipal.Create(subject, name, claims);
            await manager.SignInAsync(scheme, principal, properties);
        }

        public static async Task SignInAsync(this AuthenticationManager manager, string subject, string name, string identityProvider, params Claim[] claims)
        {
            var scheme = manager.GetIdentityServerAuthenticationScheme();

            var principal = IdentityServerPrincipal.Create(subject, name, identityProvider, claims);
            await manager.SignInAsync(scheme, principal);
        }

        public static async Task SignInAsync(this AuthenticationManager manager, string subject, string name, string identityProvider, AuthenticationProperties properties, params Claim[] claims)
        {
            var scheme = manager.GetIdentityServerAuthenticationScheme();

            var principal = IdentityServerPrincipal.Create(subject, name, identityProvider, claims);
            await manager.SignInAsync(scheme, principal, properties);
        }

        public static async Task SignInAsync(this AuthenticationManager manager, string subject, string name, IEnumerable<string> authenticationMethods, params Claim[] claims)
        {
            var scheme = manager.GetIdentityServerAuthenticationScheme();

            var principal = IdentityServerPrincipal.Create(subject, name, authenticationMethods, claims);
            await manager.SignInAsync(scheme, principal);
        }

        public static async Task SignInAsync(this AuthenticationManager manager, string subject, string name, IEnumerable<string> authenticationMethods, AuthenticationProperties properties, params Claim[] claims)
        {
            var scheme = manager.GetIdentityServerAuthenticationScheme();

            var principal = IdentityServerPrincipal.Create(subject, name, authenticationMethods, claims);
            await manager.SignInAsync(scheme, principal, properties);
        }

        public static async Task SignInAsync(this AuthenticationManager manager, string sub, string name, string identityProvider, IEnumerable<string> authenticationMethods, params Claim[] claims)
        {
            var scheme = manager.GetIdentityServerAuthenticationScheme();

            var principal = IdentityServerPrincipal.Create(sub, name, identityProvider, authenticationMethods ,claims);
            await manager.SignInAsync(scheme, principal);
        }

        public static async Task SignInAsync(this AuthenticationManager manager, string sub, string name, string identityProvider, IEnumerable<string> authenticationMethods, AuthenticationProperties properties, params Claim[] claims)
        {
            var scheme = manager.GetIdentityServerAuthenticationScheme();

            var principal = IdentityServerPrincipal.Create(sub, name, identityProvider, authenticationMethods, claims);
            await manager.SignInAsync(scheme, principal, properties);
        }

        public static async Task SignOutAsync(this AuthenticationManager manager)
        {
            var scheme = manager.GetIdentityServerAuthenticationScheme();
            await manager.SignOutAsync(scheme);
        }

        public static string GetIdentityServerAuthenticationScheme(this AuthenticationManager manager)
        {
            return manager.HttpContext.RequestServices.GetRequiredService<IdentityServerOptions>().AuthenticationOptions.EffectiveAuthenticationScheme;
        }
    }
}