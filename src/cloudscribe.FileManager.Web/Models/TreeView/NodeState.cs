using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.Web.Models.TreeView
{
    public class NodeState
    {
        public bool Checked { get; set; }
        public bool Disabled { get; set; }
        public bool Expanded { get; set; }
        public bool Selected { get; set; }
    }
}
