using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.EventHandlers
{
    public interface IHandleRoleDeleted
    {
        Task Handle(ISiteRole role);
    }
}
