// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using cloudscribe.Core.IdentityServer.EFCore.Entities;

namespace cloudscribe.Core.IdentityServer.EFCore.Mappers
{
    /// <summary>
    /// AutoMapper configuration for API resource
    /// Between model and entity
    /// </summary>
    public class ApiResourceMapperProfile : Profile
    {
        /// <summary>
        /// <see cref="ApiResourceMapperProfile"/>
        /// </summary>
        public ApiResourceMapperProfile()
        {
            CreateMap<Entities.ApiResourceProperty, KeyValuePair<string, string>>()
                .ReverseMap();

            CreateMap<Entities.ApiResource, IdentityServer4.Models.ApiResource>(MemberList.Destination)
                .ConstructUsing(src => new IdentityServer4.Models.ApiResource())
                .ForMember(x => x.ApiSecrets, opts => opts.MapFrom(x => x.Secrets))
                .ReverseMap();

            CreateMap<Entities.ApiResourceClaim, string>()
                .ConstructUsing(x => x.Type)
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            CreateMap<Entities.ApiSecret, IdentityServer4.Models.Secret>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();

            CreateMap<Entities.ApiScope, IdentityServer4.Models.Scope>(MemberList.Destination)
                .ConstructUsing(src => new IdentityServer4.Models.Scope())
                .ReverseMap();

            CreateMap<Entities.ApiScopeClaim, string>()
               .ConstructUsing(x => x.Type)
               .ReverseMap()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));
        }

        //public ApiResourceMapperProfile()
        //{
        //    // entity to model
        //    CreateMap<ApiResource, IdentityServer4.Models.ApiResource>(MemberList.Destination)
        //        .ForMember(x => x.ApiSecrets, opt => opt.MapFrom(src => src.Secrets.Select(x => x)))
        //        .ForMember(x => x.Scopes, opt => opt.MapFrom(src => src.Scopes.Select(x => x)))
        //        .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => x.Type)));
        //    CreateMap<ApiSecret, IdentityServer4.Models.Secret>(MemberList.Destination);
        //    CreateMap<ApiScope, IdentityServer4.Models.Scope>(MemberList.Destination)
        //        .ForMember(x => x.UserClaims, opt => opt.MapFrom(src => src.UserClaims.Select(x => x.Type)));

        //    // model to entity
        //    CreateMap<IdentityServer4.Models.ApiResource, ApiResource>(MemberList.Source)
        //        .ForMember(x => x.Secrets, opts => opts.MapFrom(src => src.ApiSecrets.Select(x => x)))
        //        .ForMember(x => x.Scopes, opts => opts.MapFrom(src => src.Scopes.Select(x => x)))
        //        .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => new ApiResourceClaim { Type = x })));
        //    CreateMap<IdentityServer4.Models.Secret, ApiSecret>(MemberList.Source);
        //    CreateMap<IdentityServer4.Models.Scope, ApiScope>(MemberList.Source)
        //        .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => new ApiScopeClaim { Type = x })));
        //}
    }
}
