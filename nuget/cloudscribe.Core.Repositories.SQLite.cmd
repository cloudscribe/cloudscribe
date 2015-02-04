mkdir nupkgs
mkdir cloudscribe.Core.Repositories.SQLite\lib
mkdir cloudscribe.Core.Repositories.SQLite\lib\net45

mkdir cloudscribe.Core.Repositories.SQLite\content
mkdir cloudscribe.Core.Repositories.SQLite\content\Config
mkdir cloudscribe.Core.Repositories.SQLite\content\Config\applications
mkdir cloudscribe.Core.Repositories.SQLite\content\Config\applications\cloudscribe-core
mkdir cloudscribe.Core.Repositories.SQLite\content\Config\applications\cloudscribe-core\SchemaInstallScripts
mkdir cloudscribe.Core.Repositories.SQLite\content\Config\applications\cloudscribe-core\SchemaInstallScripts\sqlite

mkdir cloudscribe.Core.Repositories.SQLite\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts
mkdir cloudscribe.Core.Repositories.SQLite\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts\sqlite

xcopy  ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\SchemaInstallScripts\sqlite\* ..\nuget\cloudscribe.Core.Repositories.SQLite\content\Config\applications\cloudscribe-core\SchemaInstallScripts\sqlite\ /s /y /d

xcopy ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\SchemaUpgradeScripts\sqlite\* cloudscribe.Core.Repositories.SQLite\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts\sqlite\ /s /y /d

xcopy ..\src\cloudscribe.Core.Repositories.SQLite\bin\Release\cloudscribe.Core.Repositories.SQLite.dll cloudscribe.Core.Repositories.SQLite\lib\net45 /y

xcopy ..\src\cloudscribe.Core.Repositories.SQLite\bin\Release\cloudscribe.Core.Repositories.SQLite.pdb cloudscribe.Core.Repositories.SQLite\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.Core.Repositories.SQLite\cloudscribe.Core.Repositories.SQLite.nuspec -Version %pversion% -OutputDirectory "nupkgs"
