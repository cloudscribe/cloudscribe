mkdir nupkgs
mkdir cloudscribe.Core.Repositories.MySql\lib
mkdir cloudscribe.Core.Repositories.MySql\lib\net45

copy ..\src\cloudscribe.Core.Repositories.MySql\bin\Release\cloudscribe.Core.Repositories.MySql.dll cloudscribe.Core.Repositories.MySql\lib\net45
copy ..\src\cloudscribe.Core.Repositories.MySql\bin\Release\cloudscribe.Core.Repositories.MySql.pdb cloudscribe.Core.Repositories.MySql\lib\net45

NuGet.exe pack cloudscribe.Core.Repositories.MySql\cloudscribe.Core.Repositories.MySql.nuspec -OutputDirectory "nupkgs"
