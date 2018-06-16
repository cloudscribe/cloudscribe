using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleUserRemovedFromRole
    {
        Task Handle(ISiteUser user, ISiteRole role);
    }
}
