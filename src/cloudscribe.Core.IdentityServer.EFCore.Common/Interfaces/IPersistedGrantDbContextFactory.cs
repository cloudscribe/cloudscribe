namespace cloudscribe.Core.IdentityServer.EFCore.Interfaces
{
    public interface IPersistedGrantDbContextFactory
    {
        IPersistedGrantDbContext CreateContext();
    }
}
