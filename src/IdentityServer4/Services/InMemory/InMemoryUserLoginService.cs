// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;

namespace IdentityServer4.Services.InMemory
{
    /// <summary>
    /// Sample implementation of a user login/provisioning services.
    /// This sample uses an in-memory store and is not suitable for production
    /// However, feel free to implement this (or a similar logic) using some form of persistent backing store
    /// </summary>
    public class InMemoryUserLoginService
    {
        private readonly List<InMemoryUser> _users;

        public InMemoryUserLoginService(List<InMemoryUser> users)
        {
            _users = users;
        }

        /// <summary>
        /// Check username and password against in-memory users
        /// </summary>
        public bool ValidateCredentials(string username, string password)
        {
            var user = FindByUsername(username);
            if (user != null)
            {
                return user.Password.Equals(password);
            }

            return false;
        }

        /// <summary>
        /// Find a user by username
        /// </summary>
        public InMemoryUser FindByUsername(string username)
        {
            return _users.FirstOrDefault(x=>x.Username.Equals(username, System.StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Find an external user by looking up the name of the provider and the unique id of that user issued by the provider
        /// </summary>
        public InMemoryUser FindByExternalProvider(string provider, string userId)
        {
            return _users.FirstOrDefault(x => 
                x.Provider == provider &&
                x.ProviderId == userId);
        }

        /// <summary>
        /// Sample auto-provision logic of new external users
        /// </summary>
        public InMemoryUser AutoProvisionUser(string provider, string userId, List<Claim> claims)
        {
            // create a list of claims that we want to transfer into our store
            var filtered = new List<Claim>();

            foreach(var claim in claims)
            {
                // if the external system sends a display name - translate that to the standard OIDC name claim
                if (claim.Type == ClaimTypes.Name)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, claim.Value));
                }
                // if the JWT handler has an outbound mapping to an OIDC claim use that
                else if (JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.ContainsKey(claim.Type))
                {
                    filtered.Add(new Claim(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[claim.Type], claim.Value));
                }
                // copy the claim as-is
                else
                {
                    filtered.Add(claim);
                }
            }
        
            // if no display name was provided, try to construct by first and/or last name
            if (!filtered.Any(x=>x.Type == JwtClaimTypes.Name))
            {
                var first = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value;
                var last = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value;
                if (first != null && last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
                }
                else if (first != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first));
                }
                else if (last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, last));
                }
            }

            // create a new unique subject id
            var sub = CryptoRandom.CreateUniqueId();

            // check if a display name is available, otherwise fallback to subject id
            var name = filtered.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value ?? sub;

            // create new user
            var user = new InMemoryUser()
            {
                Enabled = true,
                Subject = sub,
                Username = name,
                Provider = provider,
                ProviderId = userId,
                Claims = filtered
            };

            // add user to in-memory store
            _users.Add(user);

            return user;
        }
    }
}
