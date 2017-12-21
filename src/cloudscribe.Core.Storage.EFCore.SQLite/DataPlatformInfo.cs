using cloudscribe.Core.Models;

namespace cloudscribe.Core.Storage.EFCore.SQLite
{
    public class DataPlatformInfo : IDataPlatformInfo
    {
        public string DBPlatform
        {
            get { return "Entity Framework with SQLite"; }
        }
    }
}
