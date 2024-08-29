using cloudscribe.Core.Storage.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;

namespace cloudscribe.Core.Identity
{
    public class AutoLogoutTime : IAutoLogoutTime
    {
        public AutoLogoutTime(ICoreDbContextFactory coreDbContextFactory)
        {
            _contextFactory = coreDbContextFactory;
        }

        private readonly ICoreDbContextFactory _contextFactory;

        public string? GetMaximumInactivityMinutes(Guid siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var _db = _contextFactory.CreateContext())
            {
                var query = from x in _db.Sites
                            where x.Id == siteId
                            select x.MaximumInactivityInMinutes;

                var results = query.SingleOrDefaultAsync<string>(cancellationToken).Result;

                return results;
            }
        }
    }
}
