mkdir nupkgs
mkdir cloudscribe.DbHelpers.SQLite\lib
mkdir cloudscribe.DbHelpers.SQLite\lib\net45

copy ..\src\cloudscribe.DbHelpers.SQLite\bin\Release\cloudscribe.DbHelpers.SQLite.dll cloudscribe.DbHelpers.SQLite\lib\net45
copy ..\src\cloudscribe.DbHelpers.SQLite\bin\Release\cloudscribe.DbHelpers.SQLite.pdb cloudscribe.DbHelpers.SQLite\lib\net45

NuGet.exe pack cloudscribe.DbHelpers.SQLite\cloudscribe.DbHelpers.SQLite.nuspec -OutputDirectory "nupkgs"
