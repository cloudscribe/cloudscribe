using cloudscribe.Core.Models;

namespace cloudscribe.Core.Storage.NoDb
{
    public class DataPlatformInfo : IDataPlatformInfo
    {
        public string DBPlatform
        {
            get { return "NoDb file system storage"; }
        }
    }
}
