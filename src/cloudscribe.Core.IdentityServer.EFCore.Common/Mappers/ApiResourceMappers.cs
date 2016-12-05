// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AutoMapper;
using cloudscribe.Core.IdentityServer.EFCore.Entities;

namespace cloudscribe.Core.IdentityServer.EFCore.Mappers
{
    public static class ApiResourceMappers
    {
        static ApiResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static IdentityServer4.Models.ApiResource ToModel(this ApiResource resource)
        {
            return resource == null ? null : Mapper.Map<IdentityServer4.Models.ApiResource>(resource);
        }

        public static ApiResource ToEntity(this IdentityServer4.Models.ApiResource resource)
        {
            return resource == null ? null : Mapper.Map<ApiResource>(resource);
        }
    }
}
