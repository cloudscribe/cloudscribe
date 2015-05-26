mkdir nupkgs
mkdir cloudscribe.Core.Repositories.pgsql\lib
mkdir cloudscribe.Core.Repositories.pgsql\lib\net45

mkdir cloudscribe.Core.Repositories.pgsql\content
mkdir cloudscribe.Core.Repositories.pgsql\content\Config
mkdir cloudscribe.Core.Repositories.pgsql\content\Config\applications
mkdir cloudscribe.Core.Repositories.pgsql\content\Config\applications\cloudscribe-core
mkdir cloudscribe.Core.Repositories.pgsql\content\Config\applications\cloudscribe-core\install
mkdir cloudscribe.Core.Repositories.pgsql\content\Config\applications\cloudscribe-core\install\pgsql

mkdir cloudscribe.Core.Repositories.pgsql\content\Config\applications\cloudscribe-core\upgrade
mkdir cloudscribe.Core.Repositories.pgsql\content\Config\applications\cloudscribe-core\upgrade\pgsql

xcopy  ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\install\pgsql\* ..\nuget\cloudscribe.Core.Repositories.pgsql\content\Config\applications\cloudscribe-core\install\pgsql\ /s /y /d

xcopy ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\upgrade\pgsql\* cloudscribe.Core.Repositories.pgsql\content\Config\applications\cloudscribe-core\upgrade\pgsql\ /s /y /d

xcopy ..\src\cloudscribe.Core.Repositories.pgsql\bin\Release\cloudscribe.Core.Repositories.pgsql.dll cloudscribe.Core.Repositories.pgsql\lib\net45 /y

xcopy ..\src\cloudscribe.Core.Repositories.pgsql\bin\Release\cloudscribe.Core.Repositories.pgsql.pdb cloudscribe.Core.Repositories.pgsql\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.Core.Repositories.pgsql\cloudscribe.Core.Repositories.pgsql.nuspec -Version %pversion% -OutputDirectory "nupkgs"
