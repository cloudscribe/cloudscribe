// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AutoMapper;

namespace cloudscribe.Core.IdentityServer.EFCore.Mappers
{
    public static class ClientMappers
    {
        static ClientMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ClientMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static IdentityServer4.Models.Client ToModel(this Entities.Client client)
        {
            return Mapper.Map<IdentityServer4.Models.Client>(client);
        }

        public static Entities.Client ToEntity(this IdentityServer4.Models.Client client)
        {
            return Mapper.Map<Entities.Client>(client);
        }
    }
}