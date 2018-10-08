# cloudscribe.Core.IdentityServer.EFCore.PostgreSql

## Dev Notes

Note the other library cloudscribe.Core.IdentityServer.EFCore.pgsql uses mixed case table names. We can't change that since it would be a breaking change so this new project was created to use snake case tables and column names.

### How to generate migrations

open a command/powershell window on the project folder

Since this project is a netstandard20 class library it is not executable, therefore you have to pass in the --startup-project that is executable

dotnet ef --startup-project ../sourceDev.WebApp migrations add  --context cloudscribe.Core.IdentityServer.EFCore.PostgreSql.ConfigurationDbContext cloudscribe-idserver-config-yyyymmdd

dotnet ef --startup-project ../sourceDev.WebApp migrations add  --context cloudscribe.Core.IdentityServer.EFCore.PostgreSql.PersistedGrantDbContext cloudscribe-idserver-grants-yyyymmdd