mkdir nupkgs
mkdir cloudscribe.Core.Repositories.SQLite\lib
mkdir cloudscribe.Core.Repositories.SQLite\lib\net45

copy ..\src\cloudscribe.Core.Repositories.SQLite\bin\Release\cloudscribe.Core.Repositories.SQLite.dll cloudscribe.Core.Repositories.SQLite\lib\net45
copy ..\src\cloudscribe.Core.Repositories.SQLite\bin\Release\cloudscribe.Core.Repositories.SQLite.pdb cloudscribe.Core.Repositories.SQLite\lib\net45

NuGet.exe pack cloudscribe.Core.Repositories.SQLite\cloudscribe.Core.Repositories.SQLite.nuspec -OutputDirectory "nupkgs"
