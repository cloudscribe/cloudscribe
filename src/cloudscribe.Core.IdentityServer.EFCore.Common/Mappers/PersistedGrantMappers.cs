// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;

namespace cloudscribe.Core.IdentityServer.EFCore.Mappers
{
    public static class PersistedGrantMappers
    {
        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static PersistedGrant ToModel(this Entities.PersistedGrant entity)
        {
            if (entity == null) return null;

            return new PersistedGrant
            {
                Key          = entity.Key,
                Type         = entity.Type,
                SubjectId    = entity.SubjectId,
                ClientId     = entity.ClientId,
                CreationTime = entity.CreationTime,
                Expiration   = entity.Expiration,
                Data         = entity.Data
            };
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Entities.PersistedGrant ToEntity(this PersistedGrant model)
        {
            if (model == null) return null;

            return new Entities.PersistedGrant
            {
                Key          = model.Key,
                Type         = model.Type,
                SubjectId    = model.SubjectId,
                ClientId     = model.ClientId,
                CreationTime = model.CreationTime,
                Expiration   = model.Expiration,
                Data         = model.Data
            };
        }

        /// <summary>
        /// Updates an entity from a model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="entity">The entity.</param>
        public static void UpdateEntity(this PersistedGrant model, Entities.PersistedGrant entity)
        {
            entity.Key          = model.Key;
            entity.Type         = model.Type;
            entity.SubjectId    = model.SubjectId;
            entity.ClientId     = model.ClientId;
            entity.CreationTime = model.CreationTime;
            entity.Expiration   = model.Expiration;
            entity.Data         = model.Data;
        }
    }
}