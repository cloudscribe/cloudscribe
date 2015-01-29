mkdir nupkgs
mkdir cloudscribe.Core.Repositories.Firebird\lib
mkdir cloudscribe.Core.Repositories.Firebird\lib\net45

copy ..\src\cloudscribe.Core.Repositories.Firebird\bin\Release\cloudscribe.Core.Repositories.Firebird.dll cloudscribe.Core.Repositories.Firebird\lib\net45
copy ..\src\cloudscribe.Core.Repositories.Firebird\bin\Release\cloudscribe.Core.Repositories.Firebird.pdb cloudscribe.Core.Repositories.Firebird\lib\net45

NuGet.exe pack cloudscribe.Core.Repositories.Firebird\cloudscribe.Core.Repositories.Firebird.nuspec -OutputDirectory "nupkgs"
