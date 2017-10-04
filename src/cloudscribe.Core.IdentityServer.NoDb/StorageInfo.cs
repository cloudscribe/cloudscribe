using cloudscribe.Core.IdentityServerIntegration;

namespace cloudscribe.Core.IdentityServer.NoDb
{
    public class StorageInfo : IStorageInfo
    {
        public string StoragePlatform { get { return "NoDb file system storage"; } }
    }
}
