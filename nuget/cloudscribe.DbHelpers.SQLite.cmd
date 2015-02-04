mkdir nupkgs
mkdir cloudscribe.DbHelpers.SQLite\lib
mkdir cloudscribe.DbHelpers.SQLite\lib\net45

xcopy ..\src\cloudscribe.DbHelpers.SQLite\bin\Release\cloudscribe.DbHelpers.SQLite.dll cloudscribe.DbHelpers.SQLite\lib\net45 /y
xcopy ..\src\cloudscribe.DbHelpers.SQLite\bin\Release\cloudscribe.DbHelpers.SQLite.pdb cloudscribe.DbHelpers.SQLite\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.DbHelpers.SQLite\cloudscribe.DbHelpers.SQLite.nuspec -Version %pversion% -OutputDirectory "nupkgs"
