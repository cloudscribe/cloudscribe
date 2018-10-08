
namespace cloudscribe.Core.IdentityServer.EFCore.Interfaces
{
    public interface IConfigurationDbContextFactory
    {
        IConfigurationDbContext CreateContext();
    }
}
