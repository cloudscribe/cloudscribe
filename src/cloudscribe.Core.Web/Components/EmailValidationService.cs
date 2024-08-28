using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Site;
using cloudscribe.Core.Web.ViewModels.Account;
using System.Collections.Generic;
using System.Linq;

namespace cloudscribe.Core.Web.Components
{
    public class EmailValidationService : IEmailValidationService
    {
        private ISiteContext _siteSettings = null;

        private string GetRegRestrictionTld
        {
            get { return _siteSettings.RegRestrictionTld; }
        }

        public EmailValidationService(SiteContext currentSite)
        {
            _siteSettings = currentSite;
        }

        public EmailValidation RegisterEmailValidation(RegisterViewModel model)
        {
            EmailValidation emailValidation = new EmailValidation();

            if (model.Email != null)
            {
                string regRestrictionTld = GetRegRestrictionTld;
                string fullEmailAddress = model.Email;
                string emailAddressTld = fullEmailAddress[(fullEmailAddress.LastIndexOf('@') + 1)..].ToLower();

                if (regRestrictionTld != null)
                {
                    List<string> regRestrictionList = regRestrictionTld.Split(',').ToList();

                    for (var i = 0; i < regRestrictionList.Count; i++)
                    {
                        regRestrictionList[i] = regRestrictionList[i].Trim().ToLower();
                    }

                    foreach (var item in regRestrictionList)
                    {
                        if (emailAddressTld == item)
                        {
                            emailValidation.ErrorMessage = string.Empty;
                            emailValidation.IsValid = true;
                            break;
                        }
                        else
                        {
                            emailValidation.ErrorMessage = "Registration Failed.";
                            emailValidation.IsValid = false;
                        }
                    }
                }
                else
                {
                    emailValidation.ErrorMessage = string.Empty;
                    emailValidation.IsValid = true;
                }
            }
            else
            {
                emailValidation.ErrorMessage = "No email supplied.";
                emailValidation.IsValid = false;
            }
             
            return emailValidation;
        }
    }
}
