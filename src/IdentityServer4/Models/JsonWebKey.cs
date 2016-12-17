// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.Models
{
    public class JsonWebKey
    {
        public string kty { get; set; }
        public string use { get; set; }
        public string kid { get; set; }
        public string x5t { get; set; }
        public string e { get; set; }
        public string n { get; set; }
        public string[] x5c { get; set; }
    }
}