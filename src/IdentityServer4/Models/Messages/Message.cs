// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;

namespace IdentityServer4.Models
{
    /// <summary>
    /// Base class for data that needs to be written out as cookies.
    /// </summary>
    public class Message<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        public Message(TModel data)
        {
            Created = DateTimeHelper.UtcNow.Ticks;
            Data = data;
        }

        /// <summary>
        /// Gets or sets the UTC ticks the <see cref="Message"/> was created.
        /// </summary>
        /// <value>
        /// The created UTC ticks.
        /// </value>
        public long Created { get; set; }
        public TModel Data { get; set; }
    }

    public class MessageWithId<TModel> : Message<TModel>
    {
        public MessageWithId(TModel data) : base(data)
        {
            Id = CryptoRandom.CreateUniqueId();
        }

        public string Id { get; set; }
    }
}