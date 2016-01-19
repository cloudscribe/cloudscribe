// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-19
// Last Modified:			2016-01-19
// 


namespace cloudscribe.Core.Web.Components.Messaging
{
    public interface IEmailTemplateService
    {
        string GetHtmlTemplate(string templateName, string cultureCode);
        string GetPlainTextTemplate(string templateName, string cultureCode);
    }
}
