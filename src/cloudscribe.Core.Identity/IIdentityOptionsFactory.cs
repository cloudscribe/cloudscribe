using Microsoft.AspNetCore.Identity;

namespace cloudscribe.Core.Identity
{

    public interface IIdentityOptionsFactory
    {
        IdentityOptions CreateOptions();
    }

    public class DefaultIdentityOptionsFactory : IIdentityOptionsFactory
    {
        public IdentityOptions CreateOptions()
        {
            return new IdentityOptions(); 
        }
    }
        
}
