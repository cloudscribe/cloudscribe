mkdir nupkgs
mkdir cloudscribe.DbHelpers.SqlCe\lib
mkdir cloudscribe.DbHelpers.SqlCe\lib\net45

copy ..\src\cloudscribe.DbHelpers.SqlCe\bin\Release\cloudscribe.DbHelpers.SqlCe.dll cloudscribe.DbHelpers.SqlCe\lib\net45
copy ..\src\cloudscribe.DbHelpers.SqlCe\bin\Release\cloudscribe.DbHelpers.SqlCe.pdb cloudscribe.DbHelpers.SqlCe\lib\net45

NuGet.exe pack cloudscribe.DbHelpers.SqlCe\cloudscribe.DbHelpers.SqlCe.nuspec -OutputDirectory "nupkgs"
