// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  GitHub Copilot / Joe Audette
// Created:                 2026-03-10
// Last Modified:           2026-03-10
// 
// This class fixes the BuildServiceProvider() anti-pattern that previously existed in RoutingAndMvc.cs
// 
// PROBLEM: Calling services.BuildServiceProvider() during ConfigureServices creates a premature service provider
// that causes memory leaks and lifecycle issues.
//
// SOLUTION: Use IConfigureOptions<MvcOptions> which allows proper dependency injection.
// The IStringLocalizerFactory is injected by the framework after the service provider is built.
//
// PURPOSE: Localizes ASP.NET Core's built-in model binding error messages for multi-language support.
// Example: When user enters "abc" into a numeric Age field, shows localized error message.
//
// NOTE: This is only needed in sourceDev.WebApp for testing advanced localization features.
// The cloudscribe templates do not include this - it's optional for most applications.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace sourceDev.WebApp.Configuration
{
    /// <summary>
    /// Configures localized model binding error messages for ASP.NET Core MVC.
    /// This replaces the anti-pattern of calling BuildServiceProvider() during service registration.
    /// </summary>
    public class ConfigureModelBindingLocalization : IConfigureOptions<MvcOptions>
    {
        private readonly IStringLocalizerFactory _localizerFactory;

        public ConfigureModelBindingLocalization(IStringLocalizerFactory localizerFactory)
        {
            _localizerFactory = localizerFactory;
        }

        public void Configure(MvcOptions options)
        {
            var localizer = _localizerFactory.Create("ModelBindingMessages", null);
            
            // Localize the "attempted value is invalid" error message
            // Example: "The value 'abc' is not valid for Age."
            options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
                (attemptedValue, propertyName) => localizer["The value supplied for {0} is invalid.", propertyName]
            );
        }
    }
}
