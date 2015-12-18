

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.Entity.Design;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Core.Models;

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
        public int Id { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public Guid UserGuid { get; set; } = Guid.Empty;

       // [ForeignKey("UserId")]
      //  public SiteUser User { get; set; }

        public int RoleId { get; set; } = 0;
        public Guid RoleGuid { get; set; } = Guid.Empty;

      //  [ForeignKey("RoleId")]
       // public SiteRole Role { get; set; }
    }
}
