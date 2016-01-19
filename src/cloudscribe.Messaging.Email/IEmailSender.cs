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

namespace cloudscribe.Messaging.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(
            SmtpOptions smtpOptions,
            string to, 
            string from, 
            string subject, 
            string message);

        Task SendEmailAsync(
            SmtpOptions smtpOptions,
            string from,
            string fromAlias,
            string replyTo,
            string to,
            string cc,
            string bcc,
            string subject,
            string textBody,
            string htmlBody,
            string priority,
            string[] attachmentPaths,
            string[] attachmentNames);
    }
}
