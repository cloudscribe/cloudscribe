// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2017-06-09
// Last Modified:           2017-06-09
// 


using cloudscribe.Web.Common.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common.Components
{
    public interface ICkeditorOptionsResolver
    {
        Task<CkeditorOptions> GetCkeditorOptions();
    }

    public class DefaultCkeditorOptionsResolver : ICkeditorOptionsResolver
    {
        public DefaultCkeditorOptionsResolver(IOptions<CkeditorOptions> ckOptionsAccessor)
        {
            options = ckOptionsAccessor.Value;
        }

        private CkeditorOptions options;

        public Task<CkeditorOptions> GetCkeditorOptions()
        {
            return Task.FromResult(options);
        }

    }
}
