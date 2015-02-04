mkdir nupkgs
mkdir cloudscribe.Core.Repositories.Firebird\lib
mkdir cloudscribe.Core.Repositories.Firebird\lib\net45
mkdir cloudscribe.Core.Repositories.Firebird\content
mkdir cloudscribe.Core.Repositories.Firebird\content\Config
mkdir cloudscribe.Core.Repositories.Firebird\content\Config\applications
mkdir cloudscribe.Core.Repositories.Firebird\content\Config\applications\cloudscribe-core
mkdir cloudscribe.Core.Repositories.Firebird\content\Config\applications\cloudscribe-core\SchemaInstallScripts
mkdir cloudscribe.Core.Repositories.Firebird\content\Config\applications\cloudscribe-core\SchemaInstallScripts\firebird


mkdir cloudscribe.Core.Repositories.Firebird\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts
mkdir cloudscribe.Core.Repositories.Firebird\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts\firebird


xcopy  ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\SchemaInstallScripts\firebird\* ..\nuget\cloudscribe.Core.Repositories.Firebird\content\Config\applications\cloudscribe-core\SchemaInstallScripts\firebird\ /s /y /d


xcopy ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\SchemaUpgradeScripts\firebird\* cloudscribe.Core.Repositories.Firebird\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts\firebird\ /s /y /d


xcopy ..\src\cloudscribe.Core.Repositories.Firebird\bin\Release\cloudscribe.Core.Repositories.Firebird.dll cloudscribe.Core.Repositories.Firebird\lib\net45 /y

xcopy ..\src\cloudscribe.Core.Repositories.Firebird\bin\Release\cloudscribe.Core.Repositories.Firebird.pdb cloudscribe.Core.Repositories.Firebird\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.Core.Repositories.Firebird\cloudscribe.Core.Repositories.Firebird.nuspec -Version %pversion% -OutputDirectory "nupkgs"
