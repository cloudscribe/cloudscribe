using cloudscribe.Core.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;

namespace cloudscribe.MigrationHelper.mojoPortal
{
    /// <summary>
    /// if you migrate users from mojoportal into cloudscribe via the db
    /// and you were using hashed passwords in mojoportal
    /// and you migrate into the cloudscribe cs_User PasswordHash field bringing in the the data in the format Pwd|PasswordSalt
    /// where Pwd and PasswordSalt are fields in the mojoportal user table
    /// Then this implementation can migrate your user password to the newer cloudscribe hash format
    /// on first login. This will be seemless to the user since his password does not change from what it was in mojoportal.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class HashedPasswordValidator<TUser> : IFallbackPasswordHashValidator<TUser> where TUser : class
    {

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            string[] passwordProperties = hashedPassword.Split('|'); 

            if (passwordProperties.Length == 2) //expected format = pwd|salt
            {
                var pwd = passwordProperties[0];
                var salt = passwordProperties[1];
                var sha512Hash = GetSHA512Hash(salt + providedPassword);
                var isValid = (sha512Hash == pwd);

                if(!isValid)
                {
                    var md5Hash = GetMD5Hash(salt + providedPassword);
                    isValid = (md5Hash == pwd);
                }

                if(isValid)
                {
                    return PasswordVerificationResult.SuccessRehashNeeded;
                }

            }

            return PasswordVerificationResult.Failed;
        }

        private string GetMD5Hash(string cleanText)
        {
            if (string.IsNullOrEmpty(cleanText)) { return string.Empty; }

            using (MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider())
            {
                Byte[] clearBytes = new UnicodeEncoding().GetBytes(cleanText);
                Byte[] hashedBytes = hasher.ComputeHash(clearBytes);

                return BitConverter.ToString(hashedBytes);

            }

        }

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


    }
}
