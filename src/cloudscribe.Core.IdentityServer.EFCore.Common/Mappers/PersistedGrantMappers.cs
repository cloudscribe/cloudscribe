// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AutoMapper;
using IdentityServer4.Models;

namespace cloudscribe.Core.IdentityServer.EFCore.Mappers
{
    public static class PersistedGrantMappers
    {
        static PersistedGrantMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<PersistedGrantMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static PersistedGrant ToModel(this Entities.PersistedGrant token)
        {
            return token == null ? null : Mapper.Map<PersistedGrant>(token);
        }

        public static Entities.PersistedGrant ToEntity(this PersistedGrant token)
        {
            return token == null ? null : Mapper.Map<Entities.PersistedGrant>(token);
        }

        public static void UpdateEntity(this PersistedGrant token, Entities.PersistedGrant target)
        {
            Mapper.Map(token, target);
        }
    }
}