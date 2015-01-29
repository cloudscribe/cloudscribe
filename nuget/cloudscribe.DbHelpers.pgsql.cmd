mkdir nupkgs
mkdir cloudscribe.DbHelpers.MySql\lib
mkdir cloudscribe.DbHelpers.MySql\lib\net45

copy ..\src\cloudscribe.DbHelpers.MySql\bin\Release\cloudscribe.DbHelpers.MySql.dll cloudscribe.DbHelpers.MySql\lib\net45
copy ..\src\cloudscribe.DbHelpers.MySql\bin\Release\cloudscribe.DbHelpers.MySql.pdb cloudscribe.DbHelpers.MySql\lib\net45

NuGet.exe pack cloudscribe.DbHelpers.MySql\cloudscribe.DbHelpers.MySql.nuspec -OutputDirectory "nupkgs"
