
mkdir cloudscribe.Core.Models\lib
mkdir cloudscribe.Core.Models\lib\net45

copy ..\src\cloudscribe.Core.Models\bin\Release\cloudscribe.Core.Models.dll cloudscribe.Core.Models\lib\net45
copy ..\src\cloudscribe.Core.Models\bin\Release\cloudscribe.Core.Models.pdb cloudscribe.Core.Models\lib\net45

NuGet.exe pack cloudscribe.Core.Models\cloudscribe.Core.Models.nuspec
