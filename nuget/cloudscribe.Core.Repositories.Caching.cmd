mkdir nupkgs
mkdir cloudscribe.Core.Repositories.Caching\lib
mkdir cloudscribe.Core.Repositories.Caching\lib\net45

xcopy ..\src\cloudscribe.Core.Repositories.Caching\bin\Release\cloudscribe.Core.Repositories.Caching.dll cloudscribe.Core.Repositories.Caching\lib\net45 /y
xcopy ..\src\cloudscribe.Core.Repositories.Caching\bin\Release\cloudscribe.Core.Repositories.Caching.pdb cloudscribe.Core.Repositories.Caching\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.Core.Repositories.Caching\cloudscribe.Core.Repositories.Caching.nuspec  -Version %pversion% -OutputDirectory "nupkgs"
