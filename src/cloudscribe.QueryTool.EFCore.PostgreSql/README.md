﻿# cloudscribe.QueryTool.EFCore.PostgreSql

## Dev Notes

### How to generate migrations

open a command/powershell window on the project folder

Since this project is a netstandard20 class library it is not executable, therefore you have to pass in the --startup-project that is executable

~~~
dotnet ef --startup-project ../sourceDev.WebApp migrations add  --context cloudscribe.QueryTool.EFCore.PostgreSql.QueryToolDbContext migration_name
~~~
