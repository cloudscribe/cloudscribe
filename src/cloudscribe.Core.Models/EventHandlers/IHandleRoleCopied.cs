using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleRoleCopied
    {
        Task Handle(ISiteRole sourceRole, ISiteRole newRole);
    }
}
