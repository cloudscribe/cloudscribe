using cloudscribe.Core.IdentityServerIntegration;

namespace cloudscribe.Core.IdentityServer.EFCore.MSSQL
{
    public class StorageInfo : IStorageInfo
    {
        public string StoragePlatform { get { return "Entity Framework with Microsoft SqlServer"; } }
    }
}
