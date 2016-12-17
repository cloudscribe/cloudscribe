// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer4
{
    public static class IdentityServerPrincipal
    {
        public static ClaimsPrincipal Create(
            string subject,
            string name,
            params Claim[] claims)
        {
            return Create(subject, name, IdentityServerConstants.LocalIdentityProvider, new[] { OidcConstants.AuthenticationMethods.Password }, claims);
        }

        public static ClaimsPrincipal Create(
            string subject,
            string name,
            string identityProvider,
            params Claim[] claims)
        {
            return Create(subject, name, identityProvider, new[] { Constants.ExternalAuthenticationMethod }, claims);
        }

        public static ClaimsPrincipal Create(
            string subject,
            string name,
            IEnumerable<string> authenticationMethods,
            params Claim[] claims)
        {
            return Create(subject, name, IdentityServerConstants.LocalIdentityProvider, authenticationMethods, claims);
        }

        public static ClaimsPrincipal Create(
            string subject,
            string name,
            string identityProvider,
            IEnumerable<string> authenticationMethods,
            params Claim[] claims)
        {
            if (subject.IsMissing()) throw new ArgumentNullException(nameof(subject));
            if (name.IsMissing()) throw new ArgumentNullException(nameof(name));
            if (authenticationMethods.IsNullOrEmpty()) throw new ArgumentNullException(nameof(authenticationMethods));
            if (identityProvider.IsMissing()) throw new ArgumentNullException(nameof(identityProvider));
            
            var allClaims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, subject),
                new Claim(JwtClaimTypes.Name, name),
                new Claim(JwtClaimTypes.IdentityProvider, identityProvider),
                new Claim(JwtClaimTypes.AuthenticationTime, DateTimeHelper.UtcNow.ToEpochTime().ToString(), ClaimValueTypes.Integer)
            };

            foreach (var amr in authenticationMethods)
            {
                allClaims.Add(new Claim(JwtClaimTypes.AuthenticationMethod, amr));
            }

            // todo: filtering?
            foreach (var claim in claims)
            {
                allClaims.Add(claim);
            }

            var id = new ClaimsIdentity(allClaims.Distinct(new ClaimComparer()), Constants.IdentityServerAuthenticationType, JwtClaimTypes.Name, JwtClaimTypes.Role);
            return new ClaimsPrincipal(id);
        }

        internal static void AssertRequiredClaims(this ClaimsPrincipal principal)
        {
            // for now, we don't allow more than one identity in the principal/cookie
            if (principal.Identities.Count() != 1) throw new InvalidOperationException("only a single identity supported");
            if (principal.FindFirst(JwtClaimTypes.Subject) == null) throw new InvalidOperationException("sub claim is missing");
            if (principal.FindFirst(JwtClaimTypes.Name) == null) throw new InvalidOperationException("name claim is missing");
        }

        internal static void AugmentMissingClaims(this ClaimsPrincipal principal)
        {
            var identity = principal.Identities.First();

            // ASP.NET Identity issues this claim type and uses the authentication middleware name
            // such as "Google" for the value. this code is trying to correct/convert that for
            // our scenario. IOW, we take their old AuthenticationMethod value of "Google"
            // and issue it as the idp claim. we then also issue a amr with "external"
            var amr = identity.FindFirst(ClaimTypes.AuthenticationMethod);
            if (amr != null &&
                identity.FindFirst(JwtClaimTypes.IdentityProvider) == null && 
                identity.FindFirst(JwtClaimTypes.AuthenticationMethod) == null)
            {
                identity.RemoveClaim(amr);
                identity.AddClaim(new Claim(JwtClaimTypes.IdentityProvider, amr.Value));
                identity.AddClaim(new Claim(JwtClaimTypes.AuthenticationMethod, Constants.ExternalAuthenticationMethod));
            }

            if (identity.FindFirst(JwtClaimTypes.IdentityProvider) == null)
            {
                identity.AddClaim(new Claim(JwtClaimTypes.IdentityProvider, IdentityServerConstants.LocalIdentityProvider));
            }

            if (identity.FindFirst(JwtClaimTypes.AuthenticationMethod) == null)
            {
                identity.AddClaim(new Claim(JwtClaimTypes.AuthenticationMethod, OidcConstants.AuthenticationMethods.Password));
            }

            if (identity.FindFirst(JwtClaimTypes.AuthenticationTime) == null)
            {
                identity.AddClaim(new Claim(JwtClaimTypes.AuthenticationTime, DateTimeHelper.UtcNow.ToEpochTime().ToString(), ClaimValueTypes.Integer));
            }
        }

        internal static ClaimsPrincipal FromSubjectId(string subjectId, IEnumerable<Claim> additionalClaims = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, subjectId)
            };

            if (additionalClaims != null)
            {
                claims.AddRange(additionalClaims);
            }

            return Principal.Create(Constants.IdentityServerAuthenticationType,
                claims.Distinct(new ClaimComparer()).ToArray());
        }
    }
}