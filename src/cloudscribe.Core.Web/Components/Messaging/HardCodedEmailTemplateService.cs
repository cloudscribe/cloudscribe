// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-19
// Last Modified:			2016-01-19
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class HardCodedEmailTemplateService : IEmailTemplateService
    {
        public string GetHtmlTemplate(string templateName, string cultureCode)
        {
            switch(templateName)
            {
                case MessagePurpose.ConfirmAccount:

                    return ConfirmAccountHtmlTemplate();
                    
            }

            return LastResortHtmlTemplate();
        }

        private string ConfirmAccountHtmlTemplate()
        {
            return "<html><head><title>Confirm Account</title></head><body>Please confirm your account by clicking this link: <a href=\"{0}\">link</a></body></html>";
        }

        private string LastResortHtmlTemplate()
        {
            return "<html><head><title>{0}</title></head><body>{1}</body></html>";
        }

        public string GetPlainTextTemplate(string templateName, string cultureCode)
        {
            switch (templateName)
            {
                case MessagePurpose.ConfirmAccount:

                    return ConfirmAccountPlainTextTemplate();

            }

            return LastResortPlainTextTemplate();
        }

        private string ConfirmAccountPlainTextTemplate()
        {
            return "Please confirm your account by clicking this link: {0} ";
        }

        private string LastResortPlainTextTemplate()
        {
            return "{0}";
        }

    }
}
