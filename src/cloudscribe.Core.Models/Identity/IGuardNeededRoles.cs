using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    /// <summary>
    /// Role administration depends on an IEnumerable of these so any number of them can be injected to guard roles they care about.
    /// Features may depend on specific roles and need to block editing of the needed roles.
    /// </summary>
    public interface IGuardNeededRoles
    {
        /// <summary>
        /// Implmentations should return null if they do not object to the roll being renamed or deleted.
        /// If a non empty string is returned by any implementation the the edit or delete will not be allowed.
        /// The reason will be displayed to the user.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<string> GetEditRejectReason(Guid siteId, string role);
    }
}
