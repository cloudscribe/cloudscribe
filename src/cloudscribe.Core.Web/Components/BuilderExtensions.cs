// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-26
// Last Modified:			2015-07-26
// 

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;

namespace cloudscribe.Core.Web.Components
{
    public static  class BuilderExtensions
    {
        //http://stackoverflow.com/questions/31629432/iapplicationbuilder-map-branching-not-working-as-expected-in-asp-net-5

        public static IApplicationBuilder UseWhen(
            this IApplicationBuilder app,
            Func<HttpContext, bool> condition,
            Action<IApplicationBuilder> configuration)
        {
            var builder = app.New();
            configuration(builder);

            return app.Use(next => {
                builder.Run(next);

                var branch = builder.Build();

                return context => {
                    if (condition(context))
                    {
                        return branch(context);
                    }

                    return next(context);
                };
            });
        }
    }
}
