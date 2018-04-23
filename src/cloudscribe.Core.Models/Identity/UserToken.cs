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
            UserToken l = new UserToken
            {
                LoginProvider = i.LoginProvider,
                Name = i.Name,
                Value = i.Value,
                SiteId = i.SiteId,
                UserId = i.UserId
            };

            return l;
        }

    }
}
