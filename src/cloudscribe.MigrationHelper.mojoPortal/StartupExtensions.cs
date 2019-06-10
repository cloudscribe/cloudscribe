using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;
using cloudscribe.MigrationHelper.mojoPortal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        /// <summary>
        /// This works if and only if you migrated users with hashed passwords from the mojoportal users table into the cloudscribe users table
        /// importing into the PasswordHash field in cloudscribe in the format Pwd|PasswordSalt where Pwd and PasswordSalt are from the mojoportal users table/
        /// On first login the passowrds will be updated to the newer asp.net identity password hash format.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMojoPortalPasswordMigration(this IServiceCollection services)
        {

            services.AddScoped<IFallbackPasswordHashValidator<SiteUser>, HashedPasswordValidator<SiteUser>>();

            return services;
        }
    }
}
