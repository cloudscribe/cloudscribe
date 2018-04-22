using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DataProtection
    {
        public static IServiceCollection SetupDataProtection(
            this IServiceCollection services,
            IConfiguration config,
            IHostingEnvironment environment
            )
        {

            // since we are protecting some data such as social auth secrets in the db
            // we need our data protection keys to be located on disk where we can find them if 
            // we need to move to different hosting, without those key on the new host it would not be possible to decrypt
            // but it is perhaps a little risky storing these keys below the appRoot folder
            // for your own production envrionments store them outside of that if possible
            string pathToCryptoKeys = Path.Combine(environment.ContentRootPath, "dp_keys");
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(pathToCryptoKeys));

            return services;
        }

    }
}
