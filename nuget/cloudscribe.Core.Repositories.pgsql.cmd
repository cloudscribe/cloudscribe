mkdir nupkgs
mkdir cloudscribe.Core.Repositories.pgsql\lib
mkdir cloudscribe.Core.Repositories.pgsql\lib\net45

copy ..\src\cloudscribe.Core.Repositories.pgsql\bin\Release\cloudscribe.Core.Repositories.pgsql.dll cloudscribe.Core.Repositories.pgsql\lib\net45
copy ..\src\cloudscribe.Core.Repositories.pgsql\bin\Release\cloudscribe.Core.Repositories.pgsql.pdb cloudscribe.Core.Repositories.pgsql\lib\net45

NuGet.exe pack cloudscribe.Core.Repositories.pgsql\cloudscribe.Core.Repositories.pgsql.nuspec -OutputDirectory "nupkgs"
