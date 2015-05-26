mkdir nupkgs
mkdir cloudscribe.Core.Repositories.SqlCe\lib
mkdir cloudscribe.Core.Repositories.SqlCe\lib\net45

mkdir cloudscribe.Core.Repositories.SqlCe\content
mkdir cloudscribe.Core.Repositories.SqlCe\content\Config
mkdir cloudscribe.Core.Repositories.SqlCe\content\Config\applications
mkdir cloudscribe.Core.Repositories.SqlCe\content\Config\applications\cloudscribe-core
mkdir cloudscribe.Core.Repositories.SqlCe\content\Config\applications\cloudscribe-core\install
mkdir cloudscribe.Core.Repositories.SqlCe\content\Config\applications\cloudscribe-core\install\sqlce

mkdir cloudscribe.Core.Repositories.SqlCe\content\Config\applications\cloudscribe-core\upgrade
mkdir cloudscribe.Core.Repositories.SqlCe\content\Config\applications\cloudscribe-core\upgrade\sqlce

xcopy  ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\install\sqlce\* ..\nuget\cloudscribe.Core.Repositories.SqlCe\content\Config\applications\cloudscribe-core\install\sqlce\ /s /y /d

xcopy ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\upgrade\sqlce\* cloudscribe.Core.Repositories.SqlCe\content\Config\applications\cloudscribe-core\upgrade\sqlce\ /s /y /d

xcopy ..\src\cloudscribe.Core.Repositories.SqlCe\bin\Release\cloudscribe.Core.Repositories.SqlCe.dll cloudscribe.Core.Repositories.SqlCe\lib\net45 /y

xcopy ..\src\cloudscribe.Core.Repositories.SqlCe\bin\Release\cloudscribe.Core.Repositories.SqlCe.pdb cloudscribe.Core.Repositories.SqlCe\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.Core.Repositories.SqlCe\cloudscribe.Core.Repositories.SqlCe.nuspec -Version %pversion% -OutputDirectory "nupkgs"
