// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2018-06-16
// Last Modified:			2018-06-16
// 

using cloudscribe.Core.Models;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class AdministratorsRoleGuard : IGuardNeededRoles
    {
        public AdministratorsRoleGuard(IStringLocalizer<CloudscribeCore> localizer)
        {
            _localizer = localizer;
        }

        private readonly IStringLocalizer<CloudscribeCore> _localizer;

        public Task<string> GetEditRejectReason(Guid siteId, string role)
        {
            string result = null;

            if (string.IsNullOrWhiteSpace(role)) return Task.FromResult(result);

            
            if (role == "Administrators")
            {
               
                result = _localizer["The Administrators role is a required system role that cannot be deleted or renamed."];
            }

            return Task.FromResult(result);
        }

    }
}
