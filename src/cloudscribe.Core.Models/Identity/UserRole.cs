
using Newtonsoft.Json;
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
        public Guid RoleGuid { get; set; }

        // need to verify if these object properties are required for EF
        // would rather not have them
        // ignore these properties if serializing to json ie using NoDb for storage
        //[JsonIgnore]
        //public SiteRole Role { get; set; }

        public Guid UserGuid { get; set; }

        //[JsonIgnore]
        //public SiteUser User { get; set; }
    }
}
