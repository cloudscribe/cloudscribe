
mkdir nupkgs
mkdir cloudscribe.Configuration\lib
mkdir cloudscribe.Configuration\lib\net45

xcopy ..\src\cloudscribe.Configuration\bin\Release\cloudscribe.Configuration.dll cloudscribe.Configuration\lib\net45 /y
xcopy ..\src\cloudscribe.Configuration\bin\Release\cloudscribe.Configuration.pdb cloudscribe.Configuration\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.Configuration\cloudscribe.Configuration.nuspec -Version %pversion% -OutputDirectory "nupkgs"
