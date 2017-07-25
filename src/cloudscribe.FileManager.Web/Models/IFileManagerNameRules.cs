namespace cloudscribe.FileManager.Web.Models
{
    public interface IFileManagerNameRules
    {
        string GetCleanFileName(string providedFileName);
        string GetCleanFolderName(string providedFolderName);
    }
}
