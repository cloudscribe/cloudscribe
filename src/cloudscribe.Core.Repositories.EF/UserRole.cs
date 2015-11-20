

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.EF
{
    /// <summary>
    /// this class is needed to map the join table mp_UserRoles in EF
    /// 
    /// https://github.com/aspnet/EntityFramework/issues/1368
    /// http://stackoverflow.com/questions/29442493/how-to-create-a-many-to-many-relationship-with-latest-nightly-builds-of-ef7
    /// </summary>
    public class UserRole
    {
        public int Id { get; set; } = -1;
        public int UserId { get; set; } = -1;
        public Guid UserGuid { get; set; } = Guid.Empty;

        public int RoleId { get; set; } = -1;
        public Guid RoleGuid { get; set; } = Guid.Empty;
    }
}
