// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Linq;
using System.Security.Claims;
using AutoMapper;
using cloudscribe.Core.IdentityServer.EFCore.Entities;
using IdentityServer4.Models;
using Client = IdentityServer4.Models.Client;

namespace cloudscribe.Core.IdentityServer.EFCore.Mappers
{
    public static class ClientMappers
    {
        static ClientMappers()
        {
            Mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Entities.Client, Client>(MemberList.Destination)
                    .ForMember(x => x.AllowedGrantTypes, opt => opt.MapFrom(src => src.AllowedGrantTypes.Select(x => x.GrantType)))
                    .ForMember(x => x.RedirectUris, opt => opt.MapFrom(src => src.RedirectUris.Select(x => x.RedirectUri)))
                    .ForMember(x => x.PostLogoutRedirectUris, opt => opt.MapFrom(src => src.PostLogoutRedirectUris.Select(x => x.PostLogoutRedirectUri)))
                    .ForMember(x => x.AllowedScopes, opt => opt.MapFrom(src => src.AllowedScopes.Select(x => x.Scope)))
                    .ForMember(x => x.ClientSecrets, opt => opt.MapFrom(src => src.ClientSecrets.Select(x => x)))
                    .ForMember(x => x.Claims, opt => opt.MapFrom(src => src.Claims.Select(x => new Claim(x.Type, x.Value))))
                    .ForMember(x => x.IdentityProviderRestrictions, opt => opt.MapFrom(src => src.IdentityProviderRestrictions.Select(x => x.Provider)))
                    .ForMember(x => x.AllowedCorsOrigins, opt => opt.MapFrom(src => src.AllowedCorsOrigins.Select(x => x.Origin)));
                config.CreateMap<ClientSecret, Secret>(MemberList.Destination)
                    .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null));
                
                config.CreateMap<Client, Entities.Client>(MemberList.Source)
                    .ForMember(x => x.AllowedGrantTypes, opt => opt.MapFrom(src => src.AllowedGrantTypes.Select(x => new ClientGrantType {GrantType = x})))
                    .ForMember(x => x.RedirectUris, opt => opt.MapFrom(src => src.RedirectUris.Select(x => new ClientRedirectUri {RedirectUri = x})))
                    .ForMember(x => x.PostLogoutRedirectUris, opt => opt.MapFrom(src => src.PostLogoutRedirectUris.Select(x => new ClientPostLogoutRedirectUri {PostLogoutRedirectUri = x})))
                    .ForMember(x => x.AllowedScopes, opt => opt.MapFrom(src => src.AllowedScopes.Select(x => new ClientScope {Scope = x})))
                    .ForMember(x => x.Claims, opt => opt.MapFrom( src => src.Claims.Select(x => new ClientClaim {Type = x.Type, Value = x.Value})))
                    .ForMember(x => x.IdentityProviderRestrictions, opt => opt.MapFrom(src => src.IdentityProviderRestrictions.Select(x => new ClientIdPRestriction {Provider = x})))
                    .ForMember(x => x.AllowedCorsOrigins, opt => opt.MapFrom(src => src.AllowedCorsOrigins.Select(x => new ClientCorsOrigin {Origin = x})));
                config.CreateMap<Secret, ClientSecret>(MemberList.Source);
            }).CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static Client ToModel(this Entities.Client client)
        {
            return Mapper.Map<Entities.Client, Client>(client);
        }

        public static Entities.Client ToEntity(this Client client)
        {
            return Mapper.Map<Client, Entities.Client>(client);
        }
    }
}