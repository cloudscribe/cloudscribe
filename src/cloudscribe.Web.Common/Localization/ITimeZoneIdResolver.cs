using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common
{
    public interface ITimeZoneIdResolver
    {
        Task<string> GetUserTimeZoneId(CancellationToken cancellationToken = default(CancellationToken));
        Task<string> GetSiteTimeZoneId(CancellationToken cancellationToken = default(CancellationToken));
    }
}
