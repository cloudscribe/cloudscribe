mkdir nupkgs
mkdir cloudscribe.Core.Web\lib
mkdir cloudscribe.Core.Web\lib\net45
mkdir cloudscribe.Core.Web\content
mkdir cloudscribe.Core.Web\content\App_Start
mkdir cloudscribe.Core.Web\content\Config
mkdir cloudscribe.Core.Web\content\Controllers
mkdir cloudscribe.Core.Web\content\Views
mkdir cloudscribe.Core.Web\content\Views\Home
mkdir cloudscribe.Core.Web\content\Views\Shared
mkdir cloudscribe.Core.Web\content\Views\Shared\DisplayTemplates
mkdir cloudscribe.Core.Web\content\Views\Sys
mkdir cloudscribe.Core.Web\content\Views\Sys\Account
mkdir cloudscribe.Core.Web\content\Views\Sys\CoreData
mkdir cloudscribe.Core.Web\content\Views\Sys\Manage
mkdir cloudscribe.Core.Web\content\Views\Sys\RoleAdmin
mkdir cloudscribe.Core.Web\content\Views\Sys\Shared
mkdir cloudscribe.Core.Web\content\Views\Sys\Shared\DisplayTemplates
mkdir cloudscribe.Core.Web\content\Views\Sys\SiteAdmin
mkdir cloudscribe.Core.Web\content\Views\Sys\UserAdmin

mkdir cloudscribe.Core.Web\content\DI
mkdir cloudscribe.Core.Web\content\DI\Autofac



copy ..\src\cloudscribe.Core.Web\bin\Release\cloudscribe.Core.Web.dll cloudscribe.Core.Web\lib\net45
copy ..\src\cloudscribe.Core.Web\bin\Release\cloudscribe.Core.Web.pdb cloudscribe.Core.Web\lib\net45


copy ..\src\cloudscribe.WebHost\Startup.cs cloudscribe.Core.Web\content
copy ..\src\cloudscribe.WebHost\Web.AppSettings.config cloudscribe.Core.Web\content
copy ..\src\cloudscribe.WebHost\site.sitemap cloudscribe.Core.Web\content
copy ..\src\cloudscribe.WebHost\Global.asax cloudscribe.Core.Web\content
copy ..\src\cloudscribe.WebHost\Global.asax.cs cloudscribe.Core.Web\content



::copy ..\src\cloudscribe.WebHost\log4net.config cloudscribe.Core.Web\content

copy ..\src\cloudscribe.WebHost\App_Start\* cloudscribe.Core.Web\content\App_Start
copy ..\src\cloudscribe.WebHost\Config\* cloudscribe.Core.Web\content\Config
copy ..\src\cloudscribe.WebHost\Controllers\* cloudscribe.Core.Web\content\Controllers
copy ..\src\cloudscribe.WebHost\Views\* cloudscribe.Core.Web\content\Views
copy ..\src\cloudscribe.WebHost\Views\Home\* cloudscribe.Core.Web\content\Views\Home
copy ..\src\cloudscribe.WebHost\Views\Sys\Account\* cloudscribe.Core.Web\content\Views\Sys\Account
copy ..\src\cloudscribe.WebHost\Views\Sys\CoreData\* cloudscribe.Core.Web\content\Views\Sys\CoreData
copy ..\src\cloudscribe.WebHost\Views\Sys\Manage\* cloudscribe.Core.Web\content\Views\Sys\Manage
copy ..\src\cloudscribe.WebHost\Views\Sys\RoleAdmin\* cloudscribe.Core.Web\content\Views\Sys\RoleAdmin
copy ..\src\cloudscribe.WebHost\Views\Sys\Shared\* cloudscribe.Core.Web\content\Views\Sys\Shared
copy ..\src\cloudscribe.WebHost\Views\Sys\Shared\DisplayTemplates\* cloudscribe.Core.Web\content\Views\Sys\Shared\DisplayTemplates
copy ..\src\cloudscribe.WebHost\Views\Sys\SiteAdmin\* cloudscribe.Core.Web\content\Views\Sys\SiteAdmin
copy ..\src\cloudscribe.WebHost\Views\Sys\UserAdmin\* cloudscribe.Core.Web\content\Views\Sys\UserAdmin

copy ..\src\cloudscribe.WebHost\DI\* cloudscribe.Core.Web\content\DI
copy ..\src\cloudscribe.WebHost\DI\Autofac\*.* cloudscribe.Core.Web\content\DI\Autofac

NuGet.exe pack cloudscribe.Core.Web\cloudscribe.Core.Web.nuspec -OutputDirectory "nupkgs"
