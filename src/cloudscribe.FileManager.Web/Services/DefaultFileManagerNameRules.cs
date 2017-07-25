using cloudscribe.FileManager.Web.Models;

namespace cloudscribe.FileManager.Web.Services
{
    public class DefaultFileManagerNameRules : IFileManagerNameRules
    {
        public string GetCleanFileName(string providedFileName)
        {
            return providedFileName.ToCleanFileName();
        }

        public string GetCleanFolderName(string providedFolderName)
        {
            return providedFolderName.ToCleanFolderName();
        }

        
    }
}
