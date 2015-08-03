// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-03
// Last Modified:			2015-08-03
// 

using Microsoft.Framework.Configuration;
using System;


namespace cloudscribe.Configuration
{
    public class ConfigHelper
    {
        public ConfigHelper(IConfiguration configuration)
        {
            config = configuration;
        }

        private IConfiguration config;


        public string GetOrDefault(string key, string defaultIfNotFound)
        {
            string result = config.Get(key);

            if (string.IsNullOrEmpty(result)) { return defaultIfNotFound; }

            return result;
        }

        public int GetOrDefault(string key, int defaultIfNotFound)
        {
            string result = config.Get(key);

            if (string.IsNullOrEmpty(result)) { return defaultIfNotFound; }

            return Convert.ToInt32(result);
        }

        public bool GetOrDefault(string key, bool defaultIfNotFound)
        {
            string result = config.Get(key);

            if (string.IsNullOrEmpty(result)) { return defaultIfNotFound; }

            return Convert.ToBoolean(result);
        }

        public Guid GetOrDefault(string key, Guid defaultIfNotFound)
        {
            string result = config.Get(key);

            if (string.IsNullOrEmpty(result)) { return defaultIfNotFound; }
            if (result.Length != 36) { return defaultIfNotFound; }

            return new Guid(result);
        }

    }
}
