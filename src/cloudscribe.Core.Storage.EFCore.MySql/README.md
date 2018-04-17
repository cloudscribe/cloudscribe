# cloudscribe.Core.Storage.EFCore.MySql

## Dev Notes

### How to generate migrations

open a command/powershell window on the project folder

Since this project is a netstandard20 class library it is not executable, therefore you have to pass in the --startup-project that is executable

dotnet ef --startup-project ../sourceDev.WebApp.WebApp migrations add  --context cloudscribe.Core.Storage.EFCore.MySql.CoreDbContext [Migration Name]
