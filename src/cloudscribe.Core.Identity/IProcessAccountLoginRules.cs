using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public interface IProcessAccountLoginRules
    {
        Task ProcessAccountLoginRules(LoginResultTemplate template);
    }
}
