// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-22
// Last Modified:			2016-01-19
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public interface ISiteMessageEmailSender
    {
        Task SendAccountConfirmationEmailAsync(
            ISiteSettings siteSettings,
            string toAddress, 
            string subject, 
            string confirmationUrl);

        Task SendSecurityCodeEmailAsync(
            ISiteSettings siteSettings,
            string toAddress,
            string subject,
            string securityCode);

        Task SendPasswordResetEmailAsync(
            ISiteSettings siteSettings,
            string toAddress,
            string subject,
            string resetUrl);

        Task AccountPendingApprovalAdminNotification(
            ISiteSettings siteSettings,
            ISiteUser user);

        Task SendAccountApprovalNotificationAsync(
            ISiteSettings siteSettings,
            string toAddress,
            string subject,
            string loginUrl);
    }
}
