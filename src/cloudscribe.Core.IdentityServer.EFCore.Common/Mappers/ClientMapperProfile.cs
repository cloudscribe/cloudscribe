// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
//using IdentityServer4.Models as Models;
using cloudscribe.Core.IdentityServer.EFCore.Entities;

namespace cloudscribe.Core.IdentityServer.EFCore.Mappers
{
    /// <summary>
    /// AutoMapper configuration for Client
    /// Between model and entity
    /// </summary>
    public class ClientMapperProfile : Profile
    {
        /// <summary>
        /// <see>
        ///     <cref>{ClientMapperProfile}</cref>
        /// </see>
        /// </summary>
        public ClientMapperProfile()
        {
            CreateMap<Entities.ClientProperty, KeyValuePair<string, string>>()
                .ReverseMap();

            CreateMap<Entities.Client, IdentityServer4.Models.Client>()
                .ForMember(dest => dest.ProtocolType, opt => opt.Condition(srs => srs != null))
                .ReverseMap();

            CreateMap<Entities.ClientCorsOrigin, string>()
                .ConstructUsing(src => src.Origin)
                .ReverseMap()
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src));

            CreateMap<Entities.ClientIdPRestriction, string>()
                .ConstructUsing(src => src.Provider)
                .ReverseMap()
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src));

            CreateMap<Entities.ClientClaim, Claim>(MemberList.None)
                .ConstructUsing(src => new Claim(src.Type, src.Value))
                .ReverseMap();

            CreateMap<Entities.ClientScope, string>()
                .ConstructUsing(src => src.Scope)
                .ReverseMap()
                .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));

            CreateMap<Entities.ClientPostLogoutRedirectUri, string>()
                .ConstructUsing(src => src.PostLogoutRedirectUri)
                .ReverseMap()
                .ForMember(dest => dest.PostLogoutRedirectUri, opt => opt.MapFrom(src => src));

            CreateMap<Entities.ClientRedirectUri, string>()
                .ConstructUsing(src => src.RedirectUri)
                .ReverseMap()
                .ForMember(dest => dest.RedirectUri, opt => opt.MapFrom(src => src));

            CreateMap<Entities.ClientGrantType, string>()
                .ConstructUsing(src => src.GrantType)
                .ReverseMap()
                .ForMember(dest => dest.GrantType, opt => opt.MapFrom(src => src));

            CreateMap<Entities.ClientSecret, IdentityServer4.Models.Secret>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();
        }

        //public ClientMapperProfile()
        //{
        //    // entity to model
        //    CreateMap<Client, IdentityServer4.Models.Client>(MemberList.Destination)
        //        .ForMember(x => x.Properties,
        //            opt => opt.MapFrom(src => src.Properties.ToDictionary(item => item.Key, item => item.Value)))
        //        .ForMember(x => x.AllowedGrantTypes,
        //            opt => opt.MapFrom(src => src.AllowedGrantTypes.Select(x => x.GrantType)))
        //        .ForMember(x => x.RedirectUris, opt => opt.MapFrom(src => src.RedirectUris.Select(x => x.RedirectUri)))
        //        .ForMember(x => x.PostLogoutRedirectUris,
        //            opt => opt.MapFrom(src => src.PostLogoutRedirectUris.Select(x => x.PostLogoutRedirectUri)))
        //        .ForMember(x => x.AllowedScopes, opt => opt.MapFrom(src => src.AllowedScopes.Select(x => x.Scope)))
        //        .ForMember(x => x.ClientSecrets, opt => opt.MapFrom(src => src.ClientSecrets.Select(x => x)))
        //        .ForMember(x => x.Claims, opt => opt.MapFrom(src => src.Claims.Select(x => new Claim(x.Type, x.Value))))
        //        .ForMember(x => x.IdentityProviderRestrictions,
        //            opt => opt.MapFrom(src => src.IdentityProviderRestrictions.Select(x => x.Provider)))
        //        .ForMember(x => x.AllowedCorsOrigins,
        //            opt => opt.MapFrom(src => src.AllowedCorsOrigins.Select(x => x.Origin)));

        //    CreateMap<ClientSecret, IdentityServer4.Models.Secret>(MemberList.Destination)
        //        .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null));

        //    // model to entity
        //    CreateMap<IdentityServer4.Models.Client, Client>(MemberList.Source)
        //        .ForMember(x => x.Properties,
        //            opt => opt.MapFrom(src => src.Properties.ToList().Select(x => new ClientProperty { Key = x.Key, Value = x.Value })))
        //        .ForMember(x => x.AllowedGrantTypes,
        //            opt => opt.MapFrom(src => src.AllowedGrantTypes.Select(x => new ClientGrantType { GrantType = x })))
        //        .ForMember(x => x.RedirectUris,
        //            opt => opt.MapFrom(src => src.RedirectUris.Select(x => new ClientRedirectUri { RedirectUri = x })))
        //        .ForMember(x => x.PostLogoutRedirectUris,
        //            opt =>
        //                opt.MapFrom(
        //                    src =>
        //                        src.PostLogoutRedirectUris.Select(
        //                            x => new ClientPostLogoutRedirectUri { PostLogoutRedirectUri = x })))
        //        .ForMember(x => x.AllowedScopes,
        //            opt => opt.MapFrom(src => src.AllowedScopes.Select(x => new ClientScope { Scope = x })))
        //        .ForMember(x => x.Claims,
        //            opt => opt.MapFrom(src => src.Claims.Select(x => new ClientClaim { Type = x.Type, Value = x.Value })))
        //        .ForMember(x => x.IdentityProviderRestrictions,
        //            opt =>
        //                opt.MapFrom(
        //                    src => src.IdentityProviderRestrictions.Select(x => new ClientIdPRestriction { Provider = x })))
        //        .ForMember(x => x.AllowedCorsOrigins,
        //            opt => opt.MapFrom(src => src.AllowedCorsOrigins.Select(x => new ClientCorsOrigin { Origin = x })));
        //    CreateMap<IdentityServer4.Models.Secret, ClientSecret>(MemberList.Source);

        //}

        //public ClientMapperProfile()
        //{
        //    // entity to model
        //    CreateMap<Client, IdentityServer4.Models.Client>(MemberList.Destination)
        //        .ForMember(x => x.AllowedGrantTypes,
        //            opt => opt.MapFrom(src => src.AllowedGrantTypes.Select(x => x.GrantType)))
        //        .ForMember(x => x.RedirectUris, opt => opt.MapFrom(src => src.RedirectUris.Select(x => x.RedirectUri)))
        //        .ForMember(x => x.PostLogoutRedirectUris,
        //            opt => opt.MapFrom(src => src.PostLogoutRedirectUris.Select(x => x.PostLogoutRedirectUri)))
        //        .ForMember(x => x.AllowedScopes, opt => opt.MapFrom(src => src.AllowedScopes.Select(x => x.Scope)))
        //        .ForMember(x => x.ClientSecrets, opt => opt.MapFrom(src => src.ClientSecrets.Select(x => x)))
        //        .ForMember(x => x.Claims, opt => opt.MapFrom(src => src.Claims.Select(x => new Claim(x.Type, x.Value))))
        //        .ForMember(x => x.IdentityProviderRestrictions,
        //            opt => opt.MapFrom(src => src.IdentityProviderRestrictions.Select(x => x.Provider)))
        //        .ForMember(x => x.AllowedCorsOrigins,
        //            opt => opt.MapFrom(src => src.AllowedCorsOrigins.Select(x => x.Origin)));

        //    CreateMap<ClientSecret, IdentityServer4.Models.Secret>(MemberList.Destination)
        //        .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null));

        //    // model to entity
        //    CreateMap<IdentityServer4.Models.Client, Client>(MemberList.Source)
        //        .ForMember(x => x.AllowedGrantTypes,
        //            opt => opt.MapFrom(src => src.AllowedGrantTypes.Select(x => new ClientGrantType { GrantType = x })))
        //        .ForMember(x => x.RedirectUris,
        //            opt => opt.MapFrom(src => src.RedirectUris.Select(x => new ClientRedirectUri { RedirectUri = x })))
        //        .ForMember(x => x.PostLogoutRedirectUris,
        //            opt =>
        //                opt.MapFrom(
        //                    src =>
        //                        src.PostLogoutRedirectUris.Select(
        //                            x => new ClientPostLogoutRedirectUri { PostLogoutRedirectUri = x })))
        //        .ForMember(x => x.AllowedScopes,
        //            opt => opt.MapFrom(src => src.AllowedScopes.Select(x => new ClientScope { Scope = x })))
        //        .ForMember(x => x.Claims,
        //            opt => opt.MapFrom(src => src.Claims.Select(x => new ClientClaim { Type = x.Type, Value = x.Value })))
        //        .ForMember(x => x.IdentityProviderRestrictions,
        //            opt =>
        //                opt.MapFrom(
        //                    src => src.IdentityProviderRestrictions.Select(x => new ClientIdPRestriction { Provider = x })))
        //        .ForMember(x => x.AllowedCorsOrigins,
        //            opt => opt.MapFrom(src => src.AllowedCorsOrigins.Select(x => new ClientCorsOrigin { Origin = x })));
        //    CreateMap<IdentityServer4.Models.Secret, ClientSecret>(MemberList.Source);

        //}
    }
}
