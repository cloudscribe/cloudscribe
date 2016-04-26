using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.NoDb
{
    public class ProjectIndex
    {
        public ProjectIndex()
        {
            Projects = new List<ProjectMapping>();
        }

        public List<ProjectMapping> Projects { get; private set; }
    }
}
