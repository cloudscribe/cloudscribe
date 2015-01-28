
mkdir cloudscribe.Configuration\lib
mkdir cloudscribe.Configuration\lib\net45

copy ..\src\cloudscribe.Configuration\bin\Release\cloudscribe.Configuration.dll cloudscribe.Configuration\lib\net45
copy ..\src\cloudscribe.Configuration\bin\Release\cloudscribe.Configuration.pdb cloudscribe.Configuration\lib\net45

NuGet.exe pack cloudscribe.Configuration\cloudscribe.Configuration.nuspec
