
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    /// <summary>
    /// a model for mapping a join table to an entity
    /// to facilitate many to many between users and roles
    /// http://ef.readthedocs.io/en/latest/modeling/relationships.html#many-to-many
    /// http://stackoverflow.com/questions/29442493/how-to-create-a-many-to-many-relationship-with-latest-nightly-builds-of-ef7#
    /// </summary>
    public class UserRole
    {
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }

        
    }
}
