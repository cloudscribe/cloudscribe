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
::mkdir cloudscribe.Core.Web\content\Views\Sys\Account
::mkdir cloudscribe.Core.Web\content\Views\Sys\CoreData
::mkdir cloudscribe.Core.Web\content\Views\Sys\Manage
::mkdir cloudscribe.Core.Web\content\Views\Sys\RoleAdmin
::mkdir cloudscribe.Core.Web\content\Views\Sys\Shared
::mkdir cloudscribe.Core.Web\content\Views\Sys\Shared\DisplayTemplates
::mkdir cloudscribe.Core.Web\content\Views\Sys\SiteAdmin
::mkdir cloudscribe.Core.Web\content\Views\Sys\UserAdmin



copy ..\src\cloudscribe.Core.Web\bin\Release\cloudscribe.Core.Web.dll cloudscribe.Core.Web\lib\net45
copy ..\src\cloudscribe.Core.Web\bin\Release\cloudscribe.Core.Web.pdb cloudscribe.Core.Web\lib\net45


xcopy ..\src\cloudscribe.WebHost\Config\Setup\* cloudscribe.Core.Web\content\Config\Setup /s /y /d

xcopy ..\src\cloudscribe.WebHost\Config\CodeVersionProviders\cloudscribe-core.config cloudscribe.Core.Web\content\Config\CodeVersionProviders /y

xcopy ..\src\cloudscribe.WebHost\Config\RouteRegistrars\ExampleRoutes.config cloudscribe.Core.Web\content\Config\RouteRegistrars /y


xcopy ..\src\cloudscribe.WebHost\Views\Sys\* cloudscribe.Core.Web\content\Views\Sys /s /y /d

::copy ..\src\cloudscribe.WebHost\Views\Sys\Account\* cloudscribe.Core.Web\content\Views\Sys\Account
::copy ..\src\cloudscribe.WebHost\Views\Sys\CoreData\* cloudscribe.Core.Web\content\Views\Sys\CoreData
::copy ..\src\cloudscribe.WebHost\Views\Sys\Manage\* cloudscribe.Core.Web\content\Views\Sys\Manage
::copy ..\src\cloudscribe.WebHost\Views\Sys\RoleAdmin\* cloudscribe.Core.Web\content\Views\Sys\RoleAdmin
::copy ..\src\cloudscribe.WebHost\Views\Sys\Shared\* cloudscribe.Core.Web\content\Views\Sys\Shared
::copy ..\src\cloudscribe.WebHost\Views\Sys\Shared\DisplayTemplates\* cloudscribe.Core.Web\content\Views\Sys\Shared\DisplayTemplates
::copy ..\src\cloudscribe.WebHost\Views\Sys\SiteAdmin\* cloudscribe.Core.Web\content\Views\Sys\SiteAdmin
::copy ..\src\cloudscribe.WebHost\Views\Sys\UserAdmin\* cloudscribe.Core.Web\content\Views\Sys\UserAdmin


NuGet.exe pack cloudscribe.Core.Web\cloudscribe.Core.Web.nuspec -OutputDirectory "nupkgs"
