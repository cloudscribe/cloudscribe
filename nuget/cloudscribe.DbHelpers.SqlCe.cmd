mkdir nupkgs
mkdir cloudscribe.DbHelpers.SqlCe\lib
mkdir cloudscribe.DbHelpers.SqlCe\lib\net45

xcopy ..\src\cloudscribe.DbHelpers.SqlCe\bin\Release\cloudscribe.DbHelpers.SqlCe.dll cloudscribe.DbHelpers.SqlCe\lib\net45 /y
xcopy ..\src\cloudscribe.DbHelpers.SqlCe\bin\Release\cloudscribe.DbHelpers.SqlCe.pdb cloudscribe.DbHelpers.SqlCe\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.DbHelpers.SqlCe\cloudscribe.DbHelpers.SqlCe.nuspec -Version %pversion% -OutputDirectory "nupkgs"
