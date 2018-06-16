using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleUserAddedToRole
    {
        Task Handle(ISiteUser user, ISiteRole role);
    }
}
