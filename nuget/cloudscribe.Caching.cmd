
mkdir nupkgs
mkdir cloudscribe.Caching\lib
mkdir cloudscribe.Caching\lib\net45

copy ..\src\cloudscribe.Caching\bin\Release\cloudscribe.Caching.dll cloudscribe.Caching\lib\net45
copy ..\src\cloudscribe.Caching\bin\Release\cloudscribe.Caching.pdb cloudscribe.Caching\lib\net45

copy ..\src\packages-nonuget\Azure\Microsoft.ApplicationServer.Caching.Client.dll cloudscribe.Caching\lib\net45
copy ..\src\packages-nonuget\Azure\Microsoft.ApplicationServer.Caching.Core.dll cloudscribe.Caching\lib\net45

NuGet.exe pack cloudscribe.Caching\cloudscribe.Caching.nuspec -OutputDirectory "nupkgs"
