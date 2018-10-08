# cloudscribe.Core.Storage.EFCore.PostgreSql

## Dev Notes

Note the other library cloudscribe.Core.Storage.EFCore.pgsql uses mixed case tables and columns which requires quotes when writing queries. It can't be changed since that would break it, therefore this library was created as an alternative for newe projects to use snake case tables and columns.

### How to generate migrations

open a command/powershell window on the project folder

Since this project is a netstandard20 class library it is not executable, therefore you have to pass in the --startup-project that is executable

dotnet ef --startup-project ../sourceDev.WebApp migrations add  --context cloudscribe.Core.Storage.EFCore.PostgreSql.CoreDbContext cs-core-yyyymmdd
