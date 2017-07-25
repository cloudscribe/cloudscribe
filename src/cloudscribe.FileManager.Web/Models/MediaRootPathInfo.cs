namespace cloudscribe.FileManager.Web.Models
{
    public class MediaRootPathInfo
    {
        public MediaRootPathInfo(string virtualRoot, string fileSystemRoot)
        {
            RootFileSystemPath = fileSystemRoot;
            RootVirtualPath = virtualRoot;

        }
            
        public string RootVirtualPath { get; private set; }

        public string RootFileSystemPath { get; private set; }
    }
}
