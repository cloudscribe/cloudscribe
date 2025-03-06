using System;

namespace cloudscribe.FileManager.Web.Models.TreeView
{
    /// <summary>
    /// a model for this: https://github.com/patternfly/patternfly-bootstrap-treeview
    /// we probably don't need all these properties and should remove the ones we ar enot using to reduce the json payload
    /// </summary>
    public class Node
    {
        public Node()
        {
        }
        public string Text { get; set; }

        /// <summary>
        /// glyphicon glyphicon-stop
        /// </summary>
        public string Icon { get; set; }

        public string ExpandedIcon { get; set; }

        /// <summary>
        /// something.png
        /// </summary>
        public string Image { get; set; }

        public bool Selectable { get; set; } = true;

        public string Id { get; set; }

        public string Class { get; set; }

        public string Tooltip { get; set; }

        public bool LazyLoad { get; set; }

        public string Type { get; set; }

        public string MediaType { get; set; }
        public string MimeType { get; set; }

        public bool CanPreview { get; set; }

        public string VirtualPath { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Modified { get; set; } = DateTime.UtcNow;

        public long Size { get; set; }


    }
}
