namespace cloudscribe.Core.Storage.EFCore.Common
{
    public interface ICoreDbContextFactory
    {
        ICoreDbContext CreateContext();
    }
}
