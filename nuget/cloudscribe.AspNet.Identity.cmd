mkdir nupkgs
mkdir cloudscribe.AspNet.Identity\lib
mkdir cloudscribe.AspNet.Identity\lib\net45

xcopy ..\src\cloudscribe.AspNet.Identity\bin\Release\cloudscribe.AspNet.Identity.dll cloudscribe.AspNet.Identity\lib\net45 /y
xcopy ..\src\cloudscribe.AspNet.Identity\bin\Release\cloudscribe.AspNet.Identity.pdb cloudscribe.AspNet.Identity\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.AspNet.Identity\cloudscribe.AspNet.Identity.nuspec -Version %pversion% -OutputDirectory "nupkgs"
