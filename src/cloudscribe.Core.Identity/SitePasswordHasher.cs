// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-10-22
// Last Modified:		    2019-06-10
// 


using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;


namespace cloudscribe.Core.Identity
{

    public class SitePasswordHasher<TUser> : PasswordHasher<TUser> where TUser : SiteUser
    {
        public SitePasswordHasher(
            IFallbackPasswordHashValidator<TUser> fallbackPasswordHashValidator,
            IOptions<PasswordHasherOptions> optionsAccessor = null
            ):base(optionsAccessor)
        {
            _fallbackPasswordHashValidator = fallbackPasswordHashValidator;
        }

        private readonly IFallbackPasswordHashValidator<TUser> _fallbackPasswordHashValidator;

        //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/PasswordHasher.cs

        public override PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            PasswordVerificationResult result;
            try
            {
                result = base.VerifyHashedPassword(user, hashedPassword, providedPassword);
            }
            catch(FormatException) 
            {
                result = PasswordVerificationResult.Failed;
            }


            if(result == PasswordVerificationResult.Failed)
            {
                //try again with clear text which will be migrated to hashed
                result = ValidateClearTextFromKnownFormat(hashedPassword, providedPassword);

                if(result == PasswordVerificationResult.Failed)
                {
                    //try again with fallback implementation which can be used to migrate from other hash formats if implemented and injected
                    result = _fallbackPasswordHashValidator.VerifyHashedPassword(user, hashedPassword, providedPassword);
                }
            }

            return result;


        }


        /// <summary>
        /// this is used for the initial admin user created with password admin||0
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <param name="providedPassword"></param>
        /// <returns></returns>
        private PasswordVerificationResult ValidateClearTextFromKnownFormat(string hashedPassword, string providedPassword)
        {
            string[] passwordProperties = hashedPassword.Split('|');

            if (passwordProperties.Length == 3) //expected admin||0
            {
                string passwordHash = passwordProperties[0];
                //string salt = passwordProperties[1];

                // 0 = clear, 1 = hashed 2 = encrypted
                //int passwordformat = Convert.ToInt32(passwordProperties[2]);
                if (string.Equals(
                            providedPassword,
                            passwordHash,
                            StringComparison.CurrentCultureIgnoreCase)
                            )
                {
                    return PasswordVerificationResult.SuccessRehashNeeded;
                }

            }

            return PasswordVerificationResult.Failed;
        }


    }
}
