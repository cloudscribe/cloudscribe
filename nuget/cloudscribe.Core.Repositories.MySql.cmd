mkdir nupkgs
mkdir cloudscribe.Core.Repositories.MySql\lib
mkdir cloudscribe.Core.Repositories.MySql\lib\net45

mkdir cloudscribe.Core.Repositories.MySql\content
mkdir cloudscribe.Core.Repositories.MySql\content\Config
mkdir cloudscribe.Core.Repositories.MySql\content\Config\applications
mkdir cloudscribe.Core.Repositories.MySql\content\Config\applications\cloudscribe-core
mkdir cloudscribe.Core.Repositories.MySql\content\Config\applications\cloudscribe-core\install
mkdir cloudscribe.Core.Repositories.MySql\content\Config\applications\cloudscribe-core\install\mysql

mkdir cloudscribe.Core.Repositories.MySql\content\Config\applications\cloudscribe-core\upgrade
mkdir cloudscribe.Core.Repositories.MySql\content\Config\applications\cloudscribe-core\upgrade\mysql

xcopy  ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\install\mysql\* ..\nuget\cloudscribe.Core.Repositories.MySql\content\Config\applications\cloudscribe-core\install\mysql\ /s /y /d

xcopy ..\src\cloudscribe.WebHost\Config\applications\cloudscribe-core\upgrade\mysql\* cloudscribe.Core.Repositories.MySql\content\Config\applications\cloudscribe-core\upgrade\mysql\ /s /y /d

xcopy ..\src\cloudscribe.Core.Repositories.MySql\bin\Release\cloudscribe.Core.Repositories.MySql.dll cloudscribe.Core.Repositories.MySql\lib\net45 /y

xcopy ..\src\cloudscribe.Core.Repositories.MySql\bin\Release\cloudscribe.Core.Repositories.MySql.pdb cloudscribe.Core.Repositories.MySql\lib\net45 /y

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.Core.Repositories.MySql\cloudscribe.Core.Repositories.MySql.nuspec -Version %pversion% -OutputDirectory "nupkgs"
