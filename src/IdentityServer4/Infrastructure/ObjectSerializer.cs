// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IdentityServer4
{
    internal static class ObjectSerializer
    {
        private readonly static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        private readonly static JsonSerializer serializer = new JsonSerializer
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static string ToString(object o)
        {
            return JsonConvert.SerializeObject(o, settings);
        }

        public static JObject ToJObject(object o)
        {
            return JObject.FromObject(o, serializer);
        }
    }
}