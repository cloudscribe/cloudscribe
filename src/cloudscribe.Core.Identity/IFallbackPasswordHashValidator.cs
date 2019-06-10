using Microsoft.AspNetCore.Identity;

namespace cloudscribe.Core.Identity
{
    /// <summary>
    /// if you migrate users from other systems including a different hashed password you can implement and inject this to migrate from the old hash
    /// if you can validate the old hash is correct then return PasswordVerificationResult.SuccessRehashNeeded
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public interface IFallbackPasswordHashValidator<TUser> where TUser : class
    {
        PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword);

    }

    public class DefaultFallbackPasswordHashValidator<TUser> : IFallbackPasswordHashValidator<TUser> where TUser : class
    {
        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            return PasswordVerificationResult.Failed;
        }
    }

}
