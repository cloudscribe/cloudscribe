mkdir nupkgs
mkdir cloudscribe.DbHelpers.Firebird\lib
mkdir cloudscribe.DbHelpers.Firebird\lib\net45

xcopy ..\src\cloudscribe.DbHelpers.Firebird\bin\Release\cloudscribe.DbHelpers.Firebird.dll cloudscribe.DbHelpers.Firebird\lib\net45 /y
xcopy ..\src\cloudscribe.DbHelpers.Firebird\bin\Release\cloudscribe.DbHelpers.Firebird.pdb cloudscribe.DbHelpers.Firebird\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.DbHelpers.Firebird\cloudscribe.DbHelpers.Firebird.nuspec -Version %pversion% -OutputDirectory "nupkgs"
