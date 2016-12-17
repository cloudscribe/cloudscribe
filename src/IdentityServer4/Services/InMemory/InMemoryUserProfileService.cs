// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Services.InMemory
{
    /// <summary>
    /// In-memory user service
    /// </summary>
    public class InMemoryUserProfileService : IProfileService
    {
        readonly IEnumerable<InMemoryUser> _users;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryUserService"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        public InMemoryUserProfileService(List<InMemoryUser> users)
        {
            _users = users;
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var query =
                from u in _users
                where u.Subject == context.Subject.GetSubjectId()
                select u;
            var user = query.Single();

            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Subject, user.Subject));
            context.AddFilteredClaims(user.Claims);

            return Task.FromResult(0);
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. during token issuance or validation)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">subject</exception>
        public Task IsActiveAsync(IsActiveContext context)
        {
            if (context.Subject == null) throw new ArgumentNullException("subject");

            var query =
                from u in _users
                where
                    u.Subject == context.Subject.GetSubjectId()
                select u;

            var user = query.SingleOrDefault();
            
            context.IsActive = (user != null) && user.Enabled;

            return Task.FromResult(0);
        }
    }
}