// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.Extensions.DependencyInjection;
using System;

namespace IdentityServer4.Configuration
{
    public class IdentityServerBuilder : IIdentityServerBuilder
    {
        public IdentityServerBuilder(IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}