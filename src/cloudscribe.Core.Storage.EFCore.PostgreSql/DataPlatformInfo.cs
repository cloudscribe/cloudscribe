using cloudscribe.Core.Models;

namespace cloudscribe.Core.Storage.EFCore.PostgreSql
{
    public class DataPlatformInfo : IDataPlatformInfo
    {
        public string DBPlatform
        {
            get { return "Entity Framework with PostgreSql"; }
        }
    }
}
