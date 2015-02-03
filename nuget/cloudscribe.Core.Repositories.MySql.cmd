mkdir nupkgs
mkdir cloudscribe.Core.Repositories.MySql\lib
mkdir cloudscribe.Core.Repositories.MySql\lib\net45

mkdir cloudscribe.Core.Repositories.MySql\content
mkdir cloudscribe.Core.Repositories.MySql\content\Config
mkdir cloudscribe.Core.Repositories.MySql\content\Config\applications
mkdir cloudscribe.Core.Repositories.MySql\content\Config\applications\cloudscribe-core
mkdir cloudscribe.Core.Repositories.MySql\content\Config\applications\cloudscribe-core\SchemaInstallScripts
mkdir cloudscribe.Core.Repositories.MySql\content\Config\applications\cloudscribe-core\SchemaInstallScripts\mysql

mkdir cloudscribe.Core.Repositories.MySql\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts
mkdir cloudscribe.Core.Repositories.MySql\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts\mysql

xcopy  ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\SchemaInstallScripts\mysql\* ..\nuget\cloudscribe.Core.Repositories.MySql\content\Config\applications\cloudscribe-core\SchemaInstallScripts\mysql\ /s /y /d

xcopy ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\SchemaUpgradeScripts\mysql\* cloudscribe.Core.Repositories.MySql\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts\mysql\ /s /y /d

xcopy ..\src\cloudscribe.Core.Repositories.MySql\bin\Release\cloudscribe.Core.Repositories.MySql.dll cloudscribe.Core.Repositories.MySql\lib\net45 /y

xcopy ..\src\cloudscribe.Core.Repositories.MySql\bin\Release\cloudscribe.Core.Repositories.MySql.pdb cloudscribe.Core.Repositories.MySql\lib\net45 /y

NuGet.exe pack cloudscribe.Core.Repositories.MySql\cloudscribe.Core.Repositories.MySql.nuspec -OutputDirectory "nupkgs"
