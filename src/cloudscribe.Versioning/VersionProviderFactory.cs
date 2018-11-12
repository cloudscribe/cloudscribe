// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-05
// Last Modified:			2017-09-23
// 

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace cloudscribe.Versioning
{
    public class VersionProviderFactory : IVersionProviderFactory
    {
        public VersionProviderFactory(
            ILogger<VersionProviderFactory> logger,
            IEnumerable<IVersionProvider> versionProviders = null)
        {
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
            log = logger;
            if (versionProviders != null)
            {
                VersionProviders = versionProviders;
            }
            else
            {
                VersionProviders = new List<IVersionProvider>();
                log.LogWarning("IEnumerable<IVersionProvider> was null, make sure any needed IVersionProviders have been added to DI");
            }
        }

        private ILogger log;

        public IEnumerable<IVersionProvider> VersionProviders { get; private set; }

        public IVersionProvider Get(string name)
        {
            foreach (IVersionProvider provider in VersionProviders)
            {
                if (provider.Name == name) { return provider; }
            }

            return null;
        }

    }
}
