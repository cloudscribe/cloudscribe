using System;

namespace cloudscribe.Core.Models.Identity
{
    /// <summary>
    /// this interface is used when creating new users to decide the display name
    /// the default implementation uses the part of the email before @
    /// </summary>
    public interface INewUserDisplayNameResolver
    {
        string ResolveDisplayName(SiteUser user);
    }

    public class DefaultNewUserDisplayNameResolver : INewUserDisplayNameResolver
    {
        public string ResolveDisplayName(SiteUser user)
        {
            if(user == null) { throw new ArgumentNullException("user must be provided"); }

            if(string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("you must set the email on the user before calling this method");
            }

            if (user.Email.IndexOf("@") == -1)
            {
                throw new ArgumentException("you must set a valid email with @ on the user before calling this method");
            }

            return user.Email.Substring(0, user.Email.IndexOf("@"));
        }
    }

}
