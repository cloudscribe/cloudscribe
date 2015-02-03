mkdir nupkgs
mkdir cloudscribe.Core.Repositories.pgsql\lib
mkdir cloudscribe.Core.Repositories.pgsql\lib\net45

mkdir cloudscribe.Core.Repositories.pgsql\content
mkdir cloudscribe.Core.Repositories.pgsql\content\Config
mkdir cloudscribe.Core.Repositories.pgsql\content\Config\applications
mkdir cloudscribe.Core.Repositories.pgsql\content\Config\applications\cloudscribe-core
mkdir cloudscribe.Core.Repositories.pgsql\content\Config\applications\cloudscribe-core\SchemaInstallScripts
mkdir cloudscribe.Core.Repositories.pgsql\content\Config\applications\cloudscribe-core\SchemaInstallScripts\pgsql

mkdir cloudscribe.Core.Repositories.pgsql\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts
mkdir cloudscribe.Core.Repositories.pgsql\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts\pgsql

xcopy  ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\SchemaInstallScripts\pgsql\* ..\nuget\cloudscribe.Core.Repositories.pgsql\content\Config\applications\cloudscribe-core\SchemaInstallScripts\pgsql\ /s /y /d

xcopy ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\SchemaUpgradeScripts\pgsql\* cloudscribe.Core.Repositories.pgsql\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts\pgsql\ /s /y /d

xcopy ..\src\cloudscribe.Core.Repositories.pgsql\bin\Release\cloudscribe.Core.Repositories.pgsql.dll cloudscribe.Core.Repositories.pgsql\lib\net45 /y

xcopy ..\src\cloudscribe.Core.Repositories.pgsql\bin\Release\cloudscribe.Core.Repositories.pgsql.pdb cloudscribe.Core.Repositories.pgsql\lib\net45 /y

NuGet.exe pack cloudscribe.Core.Repositories.pgsql\cloudscribe.Core.Repositories.pgsql.nuspec -OutputDirectory "nupkgs"
