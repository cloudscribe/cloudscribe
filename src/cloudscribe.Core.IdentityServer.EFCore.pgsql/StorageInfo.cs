using cloudscribe.Core.IdentityServerIntegration;

namespace cloudscribe.Core.IdentityServer.EFCore.pgsql
{
    public class StorageInfo : IStorageInfo
    {
        public string StoragePlatform { get { return "Entity Framework with PostgreSql"; } }
    }
}
