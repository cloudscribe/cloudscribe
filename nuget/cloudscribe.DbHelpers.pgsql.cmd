mkdir nupkgs
mkdir cloudscribe.DbHelpers.pgsql\lib
mkdir cloudscribe.DbHelpers.pgsql\lib\net45

xcopy ..\src\cloudscribe.DbHelpers.pgsql\bin\Release\cloudscribe.DbHelpers.pgsql.dll cloudscribe.DbHelpers.pgsql\lib\net45 /y
xcopy ..\src\cloudscribe.DbHelpers.pgsql\bin\Release\cloudscribe.DbHelpers.pgsql.pdb cloudscribe.DbHelpers.pgsql\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.DbHelpers.pgsql\cloudscribe.DbHelpers.pgsql.nuspec -Version %pversion% -OutputDirectory "nupkgs"
