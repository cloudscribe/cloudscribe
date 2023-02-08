// Copyright (c) Idox Software Ltd All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Simon Annetts/ESDM
// Created:					2022-02-07
// Last Modified:			2022-02-08
//

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace cloudscribe.Web.Common.Analytics.GA4
{
    /// <summary>
    /// inject this wherever you want to capture events for google analytics GA4
    /// </summary>
    public class GoogleAnalyticsGA4Helper
    {
        public GoogleAnalyticsGA4Helper(
            ITempDataDictionaryFactory tempaDataFactory,
            IHttpContextAccessor contextAccessor,
            ILogger<GoogleAnalyticsGA4Helper> logger
            )
        {
            _tempaDataFactory = tempaDataFactory;
            _contextAccessor = contextAccessor;
            _log = logger;
        }

        private ILogger _log;
        private IHttpContextAccessor _contextAccessor;
        private ITempDataDictionaryFactory _tempaDataFactory;
        private ITempDataDictionary _tempData = null;

        private ITempDataDictionary GetTempData()
        {
            if (_tempData == null)
            {
                _tempData = _tempaDataFactory.GetTempData(_contextAccessor.HttpContext);
            }

            return _tempData;
        }

        public void AddEvent(GoogleAnalyticsGA4Event ev)
        {
            var tempData = GetTempData();
            if (tempData != null)
            {
                tempData.AddEvent(ev); //these are detected by the google analytics GA4 taghelper and rendered
            }
            else
            {
                _log.LogWarning("failed to add google analytics GA4 event because tempData was null");
            }
        }

    }
}
