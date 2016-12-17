// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Threading.Tasks;

namespace IdentityServer4.Stores
{
    internal class CookieMessageStore<TModel> : IMessageStore<TModel>
    {
        private readonly MessageCookie<TModel> _cookie;

        public CookieMessageStore(MessageCookie<TModel> cookie)
        {
            _cookie = cookie;
        }

        public Task DeleteAsync(string id)
        {
            _cookie.Clear(id);
            return Task.FromResult(0);
        }

        public Task<Message<TModel>> ReadAsync(string id)
        {
            return Task.FromResult(_cookie.Read(id));
        }

        public Task WriteAsync(string id, Message<TModel> message)
        {
            _cookie.Write(id, message);
            return Task.FromResult(0);
        }
    }
}
