mkdir nupkgs
mkdir cloudscribe.DbHelpers.MSSQL\lib
mkdir cloudscribe.DbHelpers.MSSQL\lib\net45

copy ..\src\cloudscribe.DbHelpers.MSSQL\bin\Release\cloudscribe.DbHelpers.MSSQL.dll cloudscribe.DbHelpers.MSSQL\lib\net45
copy ..\src\cloudscribe.DbHelpers.MSSQL\bin\Release\cloudscribe.DbHelpers.MSSQL.pdb cloudscribe.DbHelpers.MSSQL\lib\net45

NuGet.exe pack cloudscribe.DbHelpers.MSSQL\cloudscribe.DbHelpers.MSSQL.nuspec -OutputDirectory "nupkgs"
