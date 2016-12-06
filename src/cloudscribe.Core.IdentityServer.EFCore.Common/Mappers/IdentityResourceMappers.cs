// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AutoMapper;
using cloudscribe.Core.IdentityServer.EFCore.Entities;

namespace cloudscribe.Core.IdentityServer.EFCore.Mappers
{
    public static class IdentityResourceMappers
    {
        static IdentityResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static IdentityServer4.Models.IdentityResource ToModel(this IdentityResource resource)
        {
            return resource == null ? null : Mapper.Map<IdentityServer4.Models.IdentityResource>(resource);
        }

        public static IdentityResource ToEntity(this IdentityServer4.Models.IdentityResource resource)
        {
            return resource == null ? null : Mapper.Map<IdentityResource>(resource);
        }
    }
}
