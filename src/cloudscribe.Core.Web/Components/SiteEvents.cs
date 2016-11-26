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

namespace cloudscribe.Core.Web.Components
{
    public class SiteEvents
    {
        public SiteEvents(
            IEnumerable<IHandleSiteCreated> createdHandlers,
            IEnumerable<IHandleSitePreUpdate> preUpdateHandlers,
            IEnumerable<IHandleSitePreDelete> preDeleteHandlers,
            IEnumerable<IHandleSiteUpdated> updateHandlers,
            ILogger<SiteEvents> logger
            )
        {
            this.createdHandlers = createdHandlers;
            this.preUpdateHandlers = preUpdateHandlers;
            this.preDeleteHandlers = preDeleteHandlers;
            this.updateHandlers = updateHandlers;
            log = logger;
        }

        private IEnumerable<IHandleSiteCreated> createdHandlers;
        private IEnumerable<IHandleSitePreUpdate> preUpdateHandlers;
        private IEnumerable<IHandleSitePreDelete> preDeleteHandlers;
        private IEnumerable<IHandleSiteUpdated> updateHandlers;
        private ILogger log;

        public async Task HandleSiteCreated(
            ISiteSettings site,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in createdHandlers)
            {
                try
                {
                    await handler.HandleSiteCreated(site, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message + " " + ex.StackTrace);
                }
            }
        }

        public async Task HandleSitePreUpdate(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in preUpdateHandlers)
            {
                try
                {
                    await handler.HandleSitePreUpdate(siteId, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message + " " + ex.StackTrace);
                }
            }
        }

        public async Task HandleSitePreDelete(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in preDeleteHandlers)
            {
                try
                {
                    await handler.HandleSitePreDelete(siteId, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message + " " + ex.StackTrace);
                }
            }
        }

        public async Task HandleSiteUpdated(
            ISiteSettings site,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in updateHandlers)
            {
                try
                {
                    await handler.HandleSiteUpdated(site, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message + " " + ex.StackTrace);
                }
            }
        }


    }
}
