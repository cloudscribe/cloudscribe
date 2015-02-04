mkdir nupkgs
mkdir cloudscribe.DbHelpers.MySql\lib
mkdir cloudscribe.DbHelpers.MySql\lib\net45


xcopy ..\src\cloudscribe.DbHelpers.MySql\bin\Release\cloudscribe.DbHelpers.MySql.dll cloudscribe.DbHelpers.MySql\lib\net45 /y
xcopy ..\src\cloudscribe.DbHelpers.MySql\bin\Release\cloudscribe.DbHelpers.MySql.pdb cloudscribe.DbHelpers.MySql\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.DbHelpers.MySql\cloudscribe.DbHelpers.MySql.nuspec -Version %pversion% -OutputDirectory "nupkgs"
