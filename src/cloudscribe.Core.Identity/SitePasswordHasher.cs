// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-10-22
// Last Modified:		    2015-06-09
// 
//
// You must not remove this notice, or any other, from this software.


using Microsoft.AspNetCore.Identity;
using System;
using cloudscribe.Core.Models;

#if DNX451
using System.Security.Cryptography;
using System.Text;
#endif


namespace cloudscribe.Core.Identity
{
    /// <summary>
    /// unfortunately conversion of pre-exisitng mojoportal accounts
    /// is only going to work on the desktop framework
    /// 
    /// </summary>
    public class SitePasswordHasher<TUser> : PasswordHasher<TUser> where TUser : SiteUser
    {
        //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/PasswordHasher.cs

        /// <summary>
        /// any update to the password results in switching to the base implementation
        /// ie clear text, and encrypted are not supported, only hashed is used going forward
        /// we are supporting migration to the new Identity System, not backward compatibility
        /// with previous password formats
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public override string HashPassword(TUser user, string password)
        {
            return base.HashPassword(user, password);
        }

        public override PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            string[] passwordProperties = hashedPassword.Split('|');
            if (passwordProperties.Length != 3)
            {
                return base.VerifyHashedPassword(user, hashedPassword, providedPassword);
            }
            else
            {
                string passwordHash = passwordProperties[0];
                string salt = passwordProperties[1];

                // 0 = clear, 1 = hashed 2 = encrypted
                int passwordformat = Convert.ToInt32(passwordProperties[2]);

                switch (passwordformat)
                {


#if DNX451
                    case 2: //encrypted

                        if (String.Equals(
                            EncryptPassword(providedPassword, salt),
                            passwordHash, StringComparison.CurrentCultureIgnoreCase)
                            )
                        {
                            return PasswordVerificationResult.SuccessRehashNeeded;
                        }
                        else
                        {
                            return PasswordVerificationResult.Failed;
                        }



                    case 1: //hashed
                    

                        if (String.Equals(
                            GetSHA512Hash(providedPassword + salt),
                            passwordHash, StringComparison.CurrentCultureIgnoreCase)
                            )
                        {
                            return PasswordVerificationResult.SuccessRehashNeeded;
                        }
                        else
                        {
                            return PasswordVerificationResult.Failed;
                        }

#endif

                    case 0: //clear
                    default:
                        if (String.Equals(
                            providedPassword,
                            passwordHash,
                            StringComparison.CurrentCultureIgnoreCase)
                            )
                        {
                            return PasswordVerificationResult.SuccessRehashNeeded;
                        }
                        else
                        {
                            return PasswordVerificationResult.Failed;
                        }

                }


            }
        }

#if DNX451

        private string EncryptPassword(string providedPassword, string salt)
        {
            FakeMembershipProvider p = new FakeMembershipProvider();

            byte[] bIn = Encoding.Unicode.GetBytes(salt + providedPassword);
            byte[] bRet = p.EncryptPasswordBytes(bIn);

            string encryptedPassword = Convert.ToBase64String(bRet);
            return encryptedPassword;
        }


        

        /// <summary>
        /// this is what we were using in mojoPortal
        /// using it here instead of the base implementation for migration compatibility
        /// </summary>
        /// <param name="cleanText"></param>
        /// <returns></returns>
        private string GetSHA512Hash(string cleanText)
        {
            if (string.IsNullOrEmpty(cleanText)) { return string.Empty; }

            using (SHA512CryptoServiceProvider hasher = new SHA512CryptoServiceProvider())
            {
                Byte[] clearBytes = new UnicodeEncoding().GetBytes(cleanText);
                Byte[] hashedBytes = hasher.ComputeHash(clearBytes);

                return BitConverter.ToString(hashedBytes);

            }

        }

#endif
        //This is copied from the existing SQL providers and is provided only for back-compat.
        //private string EncryptPassword(string pass, int passwordFormat, string salt)
        //{
        //    if (passwordFormat == 0) // MembershipPasswordFormat.Clear
        //        return pass;

        //    byte[] bIn = Encoding.Unicode.GetBytes(pass);
        //    //byte[] bSalt = Convert.FromBase64String(salt);
        //    byte[] bSalt = Encoding.Unicode.GetBytes(salt);
        //    byte[] bRet = null;

        //    if (passwordFormat == 1)
        //    { // MembershipPasswordFormat.Hashed 
        //        HashAlgorithm hm = HashAlgorithm.Create("SHA1");
        //        if (hm is KeyedHashAlgorithm)
        //        {
        //            KeyedHashAlgorithm kha = (KeyedHashAlgorithm)hm;
        //            if (kha.Key.Length == bSalt.Length)
        //            {
        //                kha.Key = bSalt;
        //            }
        //            else if (kha.Key.Length < bSalt.Length)
        //            {
        //                byte[] bKey = new byte[kha.Key.Length];
        //                Buffer.BlockCopy(bSalt, 0, bKey, 0, bKey.Length);
        //                kha.Key = bKey;
        //            }
        //            else
        //            {
        //                byte[] bKey = new byte[kha.Key.Length];
        //                for (int iter = 0; iter < bKey.Length; )
        //                {
        //                    int len = Math.Min(bSalt.Length, bKey.Length - iter);
        //                    Buffer.BlockCopy(bSalt, 0, bKey, iter, len);
        //                    iter += len;
        //                }
        //                kha.Key = bKey;
        //            }
        //            bRet = kha.ComputeHash(bIn);
        //        }
        //        else
        //        {
        //            byte[] bAll = new byte[bSalt.Length + bIn.Length];
        //            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
        //            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
        //            bRet = hm.ComputeHash(bAll);
        //        }
        //    }

        //    return Convert.ToBase64String(bRet);
        //}

    }
}
