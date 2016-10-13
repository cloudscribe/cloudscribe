// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-10-13
// Last Modified:			2016-10-13
// 

using cloudscribe.Core.IdentityServerIntegration.StorageModels;
using IdentityServer4.Models;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Services
{
    public class ClientsManager
    {
        public ClientsManager(
            IClientCommands commands,
            IClientQueries queries
            )
        {
            _commands = commands;
            _queries = queries;
        }

        private IClientCommands _commands;
        private IClientQueries _queries;
    }
}
