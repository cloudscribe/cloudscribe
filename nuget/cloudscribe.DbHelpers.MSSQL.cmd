mkdir nupkgs
mkdir cloudscribe.DbHelpers.MSSQL\lib
mkdir cloudscribe.DbHelpers.MSSQL\lib\net45

xcopy ..\src\cloudscribe.DbHelpers.MSSQL\bin\Release\cloudscribe.DbHelpers.MSSQL.dll cloudscribe.DbHelpers.MSSQL\lib\net45 /y
xcopy ..\src\cloudscribe.DbHelpers.MSSQL\bin\Release\cloudscribe.DbHelpers.MSSQL.pdb cloudscribe.DbHelpers.MSSQL\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.DbHelpers.MSSQL\cloudscribe.DbHelpers.MSSQL.nuspec -Version %pversion% -OutputDirectory "nupkgs"
