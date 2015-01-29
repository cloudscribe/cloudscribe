mkdir nupkgs
mkdir cloudscribe.DbHelpers.Firebird\lib
mkdir cloudscribe.DbHelpers.Firebird\lib\net45

copy ..\src\cloudscribe.DbHelpers.Firebird\bin\Release\cloudscribe.DbHelpers.Firebird.dll cloudscribe.DbHelpers.Firebird\lib\net45
copy ..\src\cloudscribe.DbHelpers.Firebird\bin\Release\cloudscribe.DbHelpers.Firebird.pdb cloudscribe.DbHelpers.Firebird\lib\net45

NuGet.exe pack cloudscribe.DbHelpers.Firebird\cloudscribe.DbHelpers.Firebird.nuspec -OutputDirectory "nupkgs"
