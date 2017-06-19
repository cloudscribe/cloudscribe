using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityModel;

namespace sourceDev.WebApp.Configuration
{
    public class IdServerResources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                // some standard scopes from the OIDC spec
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),

                // custom identity resource with some consolidated claims
                new IdentityResource("custom.profile", new[] { JwtClaimTypes.Name, JwtClaimTypes.Email, "location" })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                // simple version with ctor
                new ApiResource("api1", "Your API 1")
                {
                    // this is needed for introspection when using reference tokens
                    ApiSecrets = { new Secret("secret".Sha256()) }
                },
                
                // expanded version if more control is needed
                new ApiResource
                {
                    Name = "api2",
                    DisplayName = "Your API 2",
                    Description = "Something interesting",

                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    UserClaims =
                    {
                        JwtClaimTypes.Name,
                    },

                    Scopes =
                    {
                        new Scope()
                        {
                            Name = "api2.full_access",
                            DisplayName = "Full access to API 2",
                            UserClaims = { "email" }
                        },
                        new Scope
                        {
                            Name = "api2.read_only",
                            DisplayName = "Read only access to API 2"
                        }
                    }
                }
            };
        }

        // test data for identityserver integration

        // scopes define the resources in your system
        //private IEnumerable<Scope> GetScopes()
        //{
        //    return new List<Scope>
        //    {
        //        StandardScopes.OpenId,
        //        StandardScopes.Profile,
        //        StandardScopes.OfflineAccess,

        //        new Scope
        //        {
        //            Name = "s2",
        //            DisplayName = "Site 2 api access",
        //            Description = "Site2 APIs"
        //        }
        //    };
        //}
    }
}
