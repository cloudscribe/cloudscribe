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
            Mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Entities.PersistedGrant, PersistedGrant>(MemberList.Destination);
                config.CreateMap<PersistedGrant, Entities.PersistedGrant>(MemberList.Source);
            }).CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static PersistedGrant ToModel(this Entities.PersistedGrant token)
        {
            if (token == null) return null;

            return Mapper.Map<Entities.PersistedGrant, PersistedGrant>(token);
        }

        public static Entities.PersistedGrant ToEntity(this PersistedGrant token)
        {
            if (token == null) return null;

            return Mapper.Map<PersistedGrant, Entities.PersistedGrant>(token);
        }

        public static void UpdateEntity(this PersistedGrant token, Entities.PersistedGrant target)
        {
            Mapper.Map<PersistedGrant, Entities.PersistedGrant>(token, target);
        }
    }
}