// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Services.InMemory
{
    public class InMemoryUserResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IEnumerable<InMemoryUser> _users;

        public InMemoryUserResourceOwnerPasswordValidator(List<InMemoryUser> users)
        {
            _users = users;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var query =
                from u in _users
                where u.Username == context.UserName && u.Password == context.Password
                select u;

            var user = query.SingleOrDefault();
            if (user != null)
            {
                context.Result = new GrantValidationResult(user.Subject, "password", user.Claims);
            }

            return Task.FromResult(0);
        }
    }
}