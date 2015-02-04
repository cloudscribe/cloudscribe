mkdir nupkgs
mkdir cloudscribe.Resources\lib
mkdir cloudscribe.Resources\lib\net45

xcopy ..\src\cloudscribe.Resources\bin\Release\cloudscribe.Resources.dll cloudscribe.Resources\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.Resources\cloudscribe.Resources.nuspec -Version %pversion% -OutputDirectory "nupkgs"
