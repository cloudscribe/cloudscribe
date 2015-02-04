mkdir nupkgs
mkdir cloudscribe.Core.Repositories.MSSQL\lib
mkdir cloudscribe.Core.Repositories.MSSQL\lib\net45

mkdir cloudscribe.Core.Repositories.MSSQL\content
mkdir cloudscribe.Core.Repositories.MSSQL\content\Config
mkdir cloudscribe.Core.Repositories.MSSQL\content\Config\applications
mkdir cloudscribe.Core.Repositories.MSSQL\content\Config\applications\cloudscribe-core
mkdir cloudscribe.Core.Repositories.MSSQL\content\Config\applications\cloudscribe-core\SchemaInstallScripts
mkdir cloudscribe.Core.Repositories.MSSQL\content\Config\applications\cloudscribe-core\SchemaInstallScripts\mssql


mkdir cloudscribe.Core.Repositories.MSSQL\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts
mkdir cloudscribe.Core.Repositories.MSSQL\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts\mssql


xcopy  ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\SchemaInstallScripts\mssql\* ..\nuget\cloudscribe.Core.Repositories.MSSQL\content\Config\applications\cloudscribe-core\SchemaInstallScripts\mssql\ /s /y /d


xcopy ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\SchemaUpgradeScripts\mssql\* cloudscribe.Core.Repositories.MSSQL\content\Config\applications\cloudscribe-core\SchemaUpgradeScripts\mssql\ /s /y /d

xcopy ..\src\cloudscribe.Core.Repositories.MSSQL\bin\Release\cloudscribe.Core.Repositories.MSSQL.dll cloudscribe.Core.Repositories.MSSQL\lib\net45 /y
xcopy ..\src\cloudscribe.Core.Repositories.MSSQL\bin\Release\cloudscribe.Core.Repositories.MSSQL.pdb cloudscribe.Core.Repositories.MSSQL\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.Core.Repositories.MSSQL\cloudscribe.Core.Repositories.MSSQL.nuspec -Version %pversion% -OutputDirectory "nupkgs"
