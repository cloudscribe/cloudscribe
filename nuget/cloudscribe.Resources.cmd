mkdir nupkgs
mkdir cloudscribe.Resources\lib
mkdir cloudscribe.Resources\lib\net45

copy ..\src\cloudscribe.Resources\bin\Release\cloudscribe.Resources.dll cloudscribe.Resources\lib\net45


NuGet.exe pack cloudscribe.Resources\cloudscribe.Resources.nuspec -OutputDirectory "nupkgs"
