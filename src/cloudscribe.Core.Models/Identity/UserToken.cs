using System;

namespace cloudscribe.Core.Models
{
    public class UserToken : IUserToken
    {
        public Guid SiteId { get; set; }
        public Guid UserId { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public static UserToken FromIUserToken(IUserToken i)
        {
            UserToken l = new UserToken();

            l.LoginProvider = i.LoginProvider;
            l.Name = i.Name;
            l.Value = i.Value;
            l.SiteId = i.SiteId;
            l.UserId = i.UserId;

            return l;
        }

    }
}
