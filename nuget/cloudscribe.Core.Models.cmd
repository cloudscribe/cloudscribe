
mkdir nupkgs
mkdir cloudscribe.Core.Models\lib
mkdir cloudscribe.Core.Models\lib\net45

xcopy ..\src\cloudscribe.Core.Models\bin\Release\cloudscribe.Core.Models.dll cloudscribe.Core.Models\lib\net45 /y
xcopy ..\src\cloudscribe.Core.Models\bin\Release\cloudscribe.Core.Models.pdb cloudscribe.Core.Models\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.Core.Models\cloudscribe.Core.Models.nuspec -Version %pversion% -OutputDirectory "nupkgs"
