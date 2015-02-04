
mkdir nupkgs
mkdir cloudscribe.Caching\lib
mkdir cloudscribe.Caching\lib\net45

xcopy ..\src\cloudscribe.Caching\bin\Release\cloudscribe.Caching.dll cloudscribe.Caching\lib\net45 /y
xcopy ..\src\cloudscribe.Caching\bin\Release\cloudscribe.Caching.pdb cloudscribe.Caching\lib\net45 /y

xcopy ..\src\packages-nonuget\Azure\Microsoft.ApplicationServer.Caching.Client.dll cloudscribe.Caching\lib\net45 /y
xcopy ..\src\packages-nonuget\Azure\Microsoft.ApplicationServer.Caching.Core.dll cloudscribe.Caching\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.Caching\cloudscribe.Caching.nuspec -Version %pversion% -OutputDirectory "nupkgs"
