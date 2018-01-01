// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// Author:                  Joe Audette
// Created:                 2017-10-12
// Last Modified:           2017-12-31
// 

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace cloudscribe.Web.Common.Analytics
{
    /// <summary>
    /// inject this wherever you want to capture events for google analytics
    /// </summary>
    public class GoogleAnalyticsHelper
    {
        public GoogleAnalyticsHelper(
            ITempDataDictionaryFactory tempaDataFactory,
            IHttpContextAccessor contextAccessor,
            ILogger<GoogleAnalyticsHelper> logger
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

        public void AddEvent(GoogleAnalyticsEvent ev)
        {
            var tempData = GetTempData();
            if (tempData != null)
            {
                tempData.AddEvent(ev); //these are detected by the google analytics taghelper and rendered
            }
            else
            {
                _log.LogWarning("failed to add google analytics event because tempData was null");
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            var tempData = GetTempData();
            if (tempData != null)
            {
                tempData.AddTransaction(transaction); //these are detected by the google analytics taghelper and rendered
            }
            else
            {
                _log.LogWarning("failed to add google analytics transaction because tempData was null");
            }
        }

    }
}
