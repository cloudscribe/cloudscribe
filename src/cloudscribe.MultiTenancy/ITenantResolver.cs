using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace cloudscribe.Multitenancy
{
    public interface ITenantResolver<TTenant>
    {
        Task<TenantContext<TTenant>> ResolveAsync(HttpContext context);
    }
}