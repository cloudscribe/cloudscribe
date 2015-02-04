mkdir nupkgs

mkdir cloudscribe.Core.Web.Integration\content
mkdir cloudscribe.Core.Web.Integration\content\App_Start
mkdir cloudscribe.Core.Web.Integration\content\Controllers
mkdir cloudscribe.Core.Web.Integration\content\Views
::mkdir cloudscribe.Core.Web.Integration\content\Views\Home
::mkdir cloudscribe.Core.Web.Integration\content\Views\Shared

mkdir cloudscribe.Core.Web.Integration\content\DI
mkdir cloudscribe.Core.Web.Integration\content\DI\Autofac


xcopy ..\src\cloudscribe.WebHost\Startup.cs cloudscribe.Core.Web.Integration\content /y
xcopy ..\src\cloudscribe.WebHost\Web.AppSettings.config.sample cloudscribe.Core.Web.Integration\content /y
xcopy ..\src\cloudscribe.WebHost\site.sitemap cloudscribe.Core.Web.Integration\content /y
xcopy ..\src\cloudscribe.WebHost\Global.asax cloudscribe.Core.Web.Integration\content /y
xcopy ..\src\cloudscribe.WebHost\Global.asax.cs cloudscribe.Core.Web.Integration\content /y



xcopy ..\src\cloudscribe.WebHost\log4net.config cloudscribe.Core.Web.Integration\content /y

xcopy ..\src\cloudscribe.WebHost\App_Start\* cloudscribe.Core.Web.Integration\content\App_Start /s /y /d

xcopy ..\src\cloudscribe.WebHost\Controllers\* cloudscribe.Core.Web.Integration\content\Controllers /s /y /d

xcopy ..\src\cloudscribe.WebHost\Views\* cloudscribe.Core.Web.Integration\content\Views /s /y /d /EXCLUDE:cloudscribe.Core.Web.Integration.ExcludedViews.txt

xcopy ..\src\cloudscribe.WebHost\DI\* cloudscribe.Core.Web.Integration\content\DI /s /y /d
::copy ..\src\cloudscribe.WebHost\DI\Autofac\*.* cloudscribe.Core.Web.Integration\content\DI\Autofac


SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.Core.Web.Integration\cloudscribe.Core.Web.Integration.nuspec -Version %pversion% -OutputDirectory "nupkgs"
