using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Infrastructure
{
    public interface ICorsPathValidator
    {
        bool IsPathAllowed(PathString path, CorsOptions options);
    }

    public class DefaultCorsPathValidator : ICorsPathValidator
    {
        public bool IsPathAllowed(PathString path, CorsOptions options)
        {
            return options.CorsPaths.Any(x => path == x);
        }
    }
}
