// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-19
// Last Modified:			2016-02-03
// 


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

                case MessagePurpose.SendSecurityCode:
                    
                    return SecurityCodeHtmlTemplate();

                case MessagePurpose.PasswordReset:

                    return PasswordResetHtmlTemplate();

                case MessagePurpose.AccountApproved:

                    return AccountApprovedHtmlTemplate();


            }

            return LastResortHtmlTemplate();
        }

        private string ConfirmAccountHtmlTemplate()
        {
            return "<html><head><title>Confirm Account</title></head><body>Please confirm your account by clicking this link: <a href=\"{0}\">link</a></body></html>";
        }

        private string SecurityCodeHtmlTemplate()
        {
            return "<html><head><title>Security Code</title></head><body>Your security code is: {1}</body></html>";
        }

        private string PasswordResetHtmlTemplate()
        {
            return "<html><head><title>Password Reset</title></head><body>A password reset has been requested for your account at {0}.<br /> If you did not request a password reset, you can ignore this message, otherwise please <a href=\"{1}\">click here to reset your password</a>.</body></html>";
        }

        private string AccountApprovedHtmlTemplate()
        {
            return "<html><head><title>Account Approved</title></head><body>Your account at {0} has been approved. You may now login by clicking here: <a href=\"{1}\">link</a></body></html>";
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

                case MessagePurpose.SendSecurityCode:

                    return SecurityCodePlainTextTemplate();

                case MessagePurpose.PasswordReset:

                    return PasswordResetPlainTextTemplate();

                case MessagePurpose.AccountApproved:

                    return AccountApprovedPlainTextTemplate();

            }

            return LastResortPlainTextTemplate();
        }

        private string ConfirmAccountPlainTextTemplate()
        {
            return "Please confirm your account by clicking this link: {0} ";
        }

        private string PasswordResetPlainTextTemplate()
        {
            return "A password reset has been requested for your account at {0}. If you did not request a password reset, you can ignore this message, otherwise if you need to reset your password please click this link: {1} ";
        }

        private string AccountApprovedPlainTextTemplate()
        {
            return "Your account at {0} has been approved. \r\nYou may now login by clicking here: {1}";
        }

        private string SecurityCodePlainTextTemplate()
        {
            return "Your security code is: {1} ";
        }

        private string LastResortPlainTextTemplate()
        {
            return "{0}";
        }

    }
}
