using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Authorization
    {
        public static AuthorizationOptions SetupAuthorizationPolicies(this AuthorizationOptions options)
        {
            options.AddCloudscribeCoreDefaultPolicies();
            options.AddCloudscribeLoggingDefaultPolicy();

            options.AddPolicy(
                "IdentityServerAdminPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("Administrators");
                });

            options.AddPolicy(
                "FileManagerPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("Administrators", "Content Administrators");
                });

            options.AddPolicy(
                "FileManagerDeletePolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("Administrators", "Content Administrators");
                });

            options.AddPolicy(
                "FakePolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("fake"); // no user has this role this policy is for verifying it fails
                    });

            return options;
        }

    }
}
