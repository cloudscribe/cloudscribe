// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2016-11-26
// Last Modified:           2016-11-26
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.EventHandlers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class UserEvents
    {
        public UserEvents(
            IEnumerable<IHandleUserCreated> createdHandlers,
            IEnumerable<IHandleUserPreUpdate> preUpdateHandlers,
            IEnumerable<IHandleUserPreDelete> preDeleteHandlers,
            IEnumerable<IHandleUserUpdated> updateHandlers,
            ILogger<UserEvents> logger
            )
        {
            this.createdHandlers = createdHandlers;
            this.preUpdateHandlers = preUpdateHandlers;
            this.preDeleteHandlers = preDeleteHandlers;
            this.updateHandlers = updateHandlers;
            log = logger;
        }

        private ILogger log;
        private IEnumerable<IHandleUserCreated> createdHandlers;
        private IEnumerable<IHandleUserPreUpdate> preUpdateHandlers;
        private IEnumerable<IHandleUserPreDelete> preDeleteHandlers;
        private IEnumerable<IHandleUserUpdated> updateHandlers;


        public async Task HandleUserCreated(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach(var handler in createdHandlers)
            {
                try
                {
                    await handler.HandleUserCreated(user, cancellationToken).ConfigureAwait(false);
                }
                catch(Exception ex)
                {
                    log.LogError(ex.Message + " " + ex.StackTrace);
                }
            }
        }

        public async Task HandleUserPreUpdate(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in preUpdateHandlers)
            {
                try
                {
                    await handler.HandleUserPreUpdate(siteId, userId, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message + " " + ex.StackTrace);
                }
            }
        }

        public async Task HandleUserPreDelete(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in preDeleteHandlers)
            {
                try
                {
                    await handler.HandleUserPreDelete(siteId, userId, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message + " " + ex.StackTrace);
                }
            }
        }

        public async Task HandleUserUpdated(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in updateHandlers)
            {
                try
                {
                    await handler.HandleUserUpdated(user, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message + " " + ex.StackTrace);
                }
            }
        }

    }
}
