using cloudscribe.Core.Models;
using System;
using System.Threading.Tasks;

namespace sourceDev.WebApp.Components
{
    public class DemoRoleGuard : IGuardNeededRoles
    {
        public Task<string> GetEditRejectReason(Guid siteId, string role)
        {
            string result = null;

            if (string.IsNullOrWhiteSpace(role)) return Task.FromResult(result);

            // more realistic example would lookup whether the role is used in the feature here
            // this hard coded example is just an example
            if(role == "Members")
            {
                // this could be localized using IStringLocalizer in a more realistic example
                result = "The members role cannot be edited or removed because it is in use in the demo feature";
            }

            return Task.FromResult(result);
        }
    }
}
