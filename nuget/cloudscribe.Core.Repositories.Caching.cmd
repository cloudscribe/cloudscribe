mkdir nupkgs
mkdir cloudscribe.Core.Repositories.Caching\lib
mkdir cloudscribe.Core.Repositories.Caching\lib\net45

copy ..\src\cloudscribe.Core.Repositories.Caching\bin\Release\cloudscribe.Core.Repositories.Caching.dll cloudscribe.Core.Repositories.Caching\lib\net45
copy ..\src\cloudscribe.Core.Repositories.Caching\bin\Release\cloudscribe.Core.Repositories.Caching.pdb cloudscribe.Core.Repositories.Caching\lib\net45

NuGet.exe pack cloudscribe.Core.Repositories.Caching\cloudscribe.Core.Repositories.Caching.nuspec -OutputDirectory "nupkgs"
