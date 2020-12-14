using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components.Messaging;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class EmailChangeHandler : IEmailChangeHandler
    {
        public EmailChangeHandler(
            SiteContext                       currentSite,
            SiteUserManager<SiteUser>         userManager,
            IStringLocalizer<CloudscribeCore> localizer,
            ISiteMessageEmailSender           emailSender,
            ILogger<EmailChangeHandler>       logger
            )
        {
            CurrentSite     = currentSite;
            UserManager     = userManager;
            StringLocalizer = localizer;
            EmailSender     = emailSender;
            Log             = logger;
        }

        protected ISiteContext              CurrentSite     { get; private set; }
        protected SiteUserManager<SiteUser> UserManager     { get; private set; }
        protected IStringLocalizer          StringLocalizer { get; private set; }
        protected ISiteMessageEmailSender   EmailSender     { get; private set; }
        protected ILogger                   Log             { get; private set; }


        public async Task<bool> HandleEmailChangeWithoutUserConfirmation(ChangeUserEmailViewModel model, 
                                                                         SiteUser user, 
                                                                         string token, 
                                                                         string siteUrl)
        {
            // do it
            var result = await UserManager.ChangeEmailAsync(user, model.NewEmail, token);

            if (result.Succeeded)
            {
                Log.LogInformation($"User with ID {user.Id} changed email address successfully.");
                model.SuccessNotification = "Email address was changed successfully.";

                if (model.EmailIsConfigured)
                {
                    await EmailSender.SendEmailChangedNotificationEmailsAsync(
                        CurrentSite,
                        model.NewEmail,
                        model.CurrentEmail,
                        StringLocalizer["Email address successfully changed"],
                        siteUrl
                        );

                    model.SuccessNotification += " " + StringLocalizer["Notification emails will be sent to old and new addresses."];
                }

                model.CurrentEmail = model.NewEmail;
                model.NewEmail     = String.Empty;
            }
            else
            {
                model.SuccessNotification = StringLocalizer["Error - email could not be changed. Contact the site administrator for support."];
                var resultError = $"Error occurred changing email address for user ID '{user.Id}'";
                if (result?.Errors != null && result.Errors.Count() > 0)
                    resultError += result.Errors.First().Description;
                Log.LogError(resultError);
            }

            return result.Succeeded;
        }

        public async Task<bool> HandleEmailChangeWithUserConfirmation(ChangeUserEmailViewModel model, 
                                                                      SiteUser user, 
                                                                      string token, 
                                                                      string confirmationUrl, 
                                                                      string siteUrl)
        {

            if (!model.EmailIsConfigured)
            {
                Log.LogWarning($"Failed to send email change confirmation email to user with ID {user.Id} because Email not configured.");
                model.SuccessNotification = StringLocalizer["Error: Emails are not configured for this site."];
                return false;
            }

            await EmailSender.SendEmailChangedConfirmationEmailAsync(
                        CurrentSite,
                        model.NewEmail,
                        model.CurrentEmail,
                        StringLocalizer["Confirm email address change"],
                        siteUrl,
                        confirmationUrl,
                        token
                        );

            model.SuccessNotification = StringLocalizer["Please check your email inbox, we just sent you a link that you need to click to confirm your account."];
            return true;
        }

        public async Task<bool> HandleEmailChangeConfirmation(ChangeUserEmailViewModel model, 
                                                              SiteUser user,
                                                              string newEmail,
                                                              string token,
                                                              string siteUrl)
        {
            // do it
            var result = await UserManager.ChangeEmailAsync(user, newEmail, token);
            
            if (result.Succeeded)
            {
                Log.LogInformation($"User with ID {user.Id} changed email address successfully.");
                model.SuccessNotification = "Email address was changed successfully.";

                if (model.EmailIsConfigured)
                {
                    await EmailSender.SendEmailChangedNotificationEmailsAsync(
                        CurrentSite,
                        String.Empty,       // no need to re-notify new address
                        model.CurrentEmail, // notify old address
                        StringLocalizer["Email address successfully changed"],
                        siteUrl
                        );

                    model.SuccessNotification += " " + StringLocalizer["A notification email will be sent your old email address."];
                }

                model.CurrentEmail = model.NewEmail;
                model.NewEmail     = String.Empty;
            }
            else
            {
                model.SuccessNotification = StringLocalizer["Error - email could not be changed. Contact the site administrator for support."];
                var resultError = $"Error occurred changing email address for user ID '{user.Id}'";
                if (result?.Errors != null && result.Errors.Count() > 0)
                    resultError += result.Errors.First().Description;
                Log.LogError(resultError);
            }

            return result.Succeeded;
        }
    }
}
