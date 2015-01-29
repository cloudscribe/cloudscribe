mkdir nupkgs
mkdir cloudscribe.Core.Repositories.SqlCe\lib
mkdir cloudscribe.Core.Repositories.SqlCe\lib\net45

copy ..\src\cloudscribe.Core.Repositories.SqlCe\bin\Release\cloudscribe.Core.Repositories.SqlCe.dll cloudscribe.Core.Repositories.SqlCe\lib\net45
copy ..\src\cloudscribe.Core.Repositories.SqlCe\bin\Release\cloudscribe.Core.Repositories.SqlCe.pdb cloudscribe.Core.Repositories.SqlCe\lib\net45

NuGet.exe pack cloudscribe.Core.Repositories.SqlCe\cloudscribe.Core.Repositories.SqlCe.nuspec -OutputDirectory "nupkgs"
