mkdir nupkgs
mkdir cloudscribe.Core.Repositories.MSSQL\lib
mkdir cloudscribe.Core.Repositories.MSSQL\lib\net45

copy ..\src\cloudscribe.Core.Repositories.MSSQL\bin\Release\cloudscribe.Core.Repositories.MSSQL.dll cloudscribe.Core.Repositories.MSSQL\lib\net45
copy ..\src\cloudscribe.Core.Repositories.MSSQL\bin\Release\cloudscribe.Core.Repositories.MSSQL.pdb cloudscribe.Core.Repositories.MSSQL\lib\net45

NuGet.exe pack cloudscribe.Core.Repositories.MSSQL\cloudscribe.Core.Repositories.MSSQL.nuspec -OutputDirectory "nupkgs"
