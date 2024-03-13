namespace cloudscribe.QueryTool.EFCore.Common
{
    public interface IQueryToolDbContextFactory
    {
        IQueryToolDbContext CreateContext();
    }
}