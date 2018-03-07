// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2016-11-26
// Last Modified:           2018-03-07
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
            _createdHandlers = createdHandlers;
            _preUpdateHandlers = preUpdateHandlers;
            _preDeleteHandlers = preDeleteHandlers;
            _updateHandlers = updateHandlers;
            _log = logger;
        }

        private IEnumerable<IHandleSiteCreated> _createdHandlers;
        private IEnumerable<IHandleSitePreUpdate> _preUpdateHandlers;
        private IEnumerable<IHandleSitePreDelete> _preDeleteHandlers;
        private IEnumerable<IHandleSiteUpdated> _updateHandlers;
        private ILogger _log;

        public async Task HandleSiteCreated(
            ISiteSettings site,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in _createdHandlers)
            {
                try
                {
                    await handler.HandleSiteCreated(site, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message} :  {ex.StackTrace}");
                }
            }
        }

        public async Task HandleSitePreUpdate(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in _preUpdateHandlers)
            {
                try
                {
                    await handler.HandleSitePreUpdate(siteId, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message} :  {ex.StackTrace}");
                }
            }
        }

        public async Task HandleSitePreDelete(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in _preDeleteHandlers)
            {
                try
                {
                    await handler.HandleSitePreDelete(siteId, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message} :  {ex.StackTrace}");
                }
            }
        }

        public async Task HandleSiteUpdated(
            ISiteSettings site,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in _updateHandlers)
            {
                try
                {
                    await handler.HandleSiteUpdated(site, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message} :  {ex.StackTrace}");
                }
            }
        }


    }
}
