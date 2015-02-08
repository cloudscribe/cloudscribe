mkdir nupkgs
mkdir cloudscribe.Core.Web\lib
mkdir cloudscribe.Core.Web\lib\net45
mkdir cloudscribe.Core.Web\content

mkdir cloudscribe.Core.Web\content\Config
mkdir cloudscribe.Core.Web\content\Config\CodeVersionProviders
mkdir cloudscribe.Core.Web\content\Config\RouteRegistrars
mkdir cloudscribe.Core.Web\content\Config\Setup
mkdir cloudscribe.Core.Web\content\Views
mkdir cloudscribe.Core.Web\content\Views\Sys

xcopy ..\src\cloudscribe.Core.Web\bin\Release\cloudscribe.Core.Web.dll cloudscribe.Core.Web\lib\net45 /y
xcopy ..\src\cloudscribe.Core.Web\bin\Release\cloudscribe.Core.Web.pdb cloudscribe.Core.Web\lib\net45 /y
xcopy ..\src\cloudscribe.Resources\bin\Release\cloudscribe.Resources.dll cloudscribe.Core.Web\lib\net45 /y
xcopy ..\src\cloudscribe.Resources\bin\Release\cloudscribe.Resources.pdb cloudscribe.Core.Web\lib\net45 /y


xcopy ..\src\cloudscribe.WebHost\Config\Setup\* cloudscribe.Core.Web\content\Config\Setup /s /y /d

xcopy ..\src\cloudscribe.WebHost\Config\CodeVersionProviders\cloudscribe-core.config cloudscribe.Core.Web\content\Config\CodeVersionProviders /y

xcopy ..\src\cloudscribe.WebHost\Config\RouteRegistrars\ExampleRoutes.config cloudscribe.Core.Web\content\Config\RouteRegistrars /y

xcopy ..\src\cloudscribe.WebHost\Views\Sys\* cloudscribe.Core.Web\content\Views\Sys /s /y /d

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.Core.Web\cloudscribe.Core.Web.nuspec -Version %pversion% -OutputDirectory "nupkgs"
