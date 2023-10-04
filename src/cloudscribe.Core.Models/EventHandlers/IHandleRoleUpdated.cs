using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleRoleUpdated
    {
        Task Handle(ISiteRole role, string oldRoleName);
    }
}
