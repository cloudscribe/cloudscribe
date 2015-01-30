mkdir nupkgs
mkdir cloudscribe.DbHelpers.pgsql\lib
mkdir cloudscribe.DbHelpers.pgsql\lib\net45

copy ..\src\cloudscribe.DbHelpers.pgsql\bin\Release\cloudscribe.DbHelpers.pgsql.dll cloudscribe.DbHelpers.pgsql\lib\net45
copy ..\src\cloudscribe.DbHelpers.pgsql\bin\Release\cloudscribe.DbHelpers.pgsql.pdb cloudscribe.DbHelpers.pgsql\lib\net45

NuGet.exe pack cloudscribe.DbHelpers.pgsql\cloudscribe.DbHelpers.pgsql.nuspec -OutputDirectory "nupkgs"
