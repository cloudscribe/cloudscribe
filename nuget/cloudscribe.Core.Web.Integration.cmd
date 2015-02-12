mkdir nupkgs

mkdir cloudscribe.Core.Web.Integration\content
mkdir cloudscribe.Core.Web.Integration\content\App_Start
mkdir cloudscribe.Core.Web.Integration\content\Content
mkdir cloudscribe.Core.Web.Integration\content\Controllers
mkdir cloudscribe.Core.Web.Integration\content\Views


mkdir cloudscribe.Core.Web.Integration\content\DI
mkdir cloudscribe.Core.Web.Integration\content\DI\Autofac
mkdir cloudscribe.Core.Web.Integration\content\DI\Autofac\Modules

xcopy ..\src\cloudscribe.WebHost\Web.AppSettings.config.sample cloudscribe.Core.Web.Integration\content /y

del cloudscribe.Core.Web.Integration\content\Web.AppSettings.config
ren cloudscribe.Core.Web.Integration\content\Web.AppSettings.config.sample Web.AppSettings.config

xcopy ..\src\cloudscribe.WebHost\site.sitemap cloudscribe.Core.Web.Integration\content /y
xcopy ..\src\cloudscribe.WebHost\log4net.config cloudscribe.Core.Web.Integration\content /y
xcopy ..\src\cloudscribe.WebHost\Content\Site.css cloudscribe.Core.Web.Integration\content\Content /y

xcopy content-src\Root.Web.config.install.xdt cloudscribe.Core.Web.Integration\content /y

del cloudscribe.Core.Web.Integration\content\Web.config.install.xdt
ren cloudscribe.Core.Web.Integration\content\Root.Web.config.install.xdt Web.config.install.xdt

xcopy content-src\Views.Web.config.install.xdt cloudscribe.Core.Web.Integration\content\Views /y

del cloudscribe.Core.Web.Integration\content\Views\Web.config.install.xdt
ren cloudscribe.Core.Web.Integration\content\Views\Views.Web.config.install.xdt Web.config.install.xdt

::xcopy ..\src\cloudscribe.WebHost\Startup.cs cloudscribe.Core.Web.Integration\content /y
call BatchSubstitute.cmd "cloudscribe.WebHost" "$rootnamespace$" ..\src\cloudscribe.WebHost\Startup.cs>cloudscribe.Core.Web.Integration\content\Startup.cs.pp

echo on
::ECHO.%ERRORLEVEL%

::xcopy ..\src\cloudscribe.WebHost\Global.asax cloudscribe.Core.Web.Integration\content /y
call BatchSubstitute.cmd "cloudscribe.WebHost" "$rootnamespace$" ..\src\cloudscribe.WebHost\Global.asax>cloudscribe.Core.Web.Integration\content\Global.asax.pp

echo on
::ECHO.%ERRORLEVEL%

::xcopy ..\src\cloudscribe.WebHost\Global.asax.cs cloudscribe.Core.Web.Integration\content /y
call BatchSubstitute.cmd "cloudscribe.WebHost" "$rootnamespace$" ..\src\cloudscribe.WebHost\Global.asax.cs>cloudscribe.Core.Web.Integration\content\Global.asax.cs.pp

echo on
::ECHO.%ERRORLEVEL%

::xcopy ..\src\cloudscribe.WebHost\App_Start\* cloudscribe.Core.Web.Integration\content\App_Start /s /y /d

call BatchSubstitute.cmd "cloudscribe.WebHost" "$rootnamespace$" ..\src\cloudscribe.WebHost\App_Start\BundleConfig.cs>cloudscribe.Core.Web.Integration\content\App_Start\BundleConfig.cs.pp

echo on
::ECHO.%ERRORLEVEL%

call BatchSubstitute.cmd "cloudscribe.WebHost" "$rootnamespace$" ..\src\cloudscribe.WebHost\App_Start\FilterConfig.cs>cloudscribe.Core.Web.Integration\content\App_Start\FilterConfig.cs.pp

echo on
::ECHO.%ERRORLEVEL%

call BatchSubstitute.cmd "cloudscribe.WebHost" "$rootnamespace$" ..\src\cloudscribe.WebHost\App_Start\RouteConfig.cs>cloudscribe.Core.Web.Integration\content\App_Start\RouteConfig.cs.pp

echo on
::ECHO.%ERRORLEVEL%

call BatchSubstitute.cmd "cloudscribe.WebHost" "$rootnamespace$" ..\src\cloudscribe.WebHost\App_Start\Startup.Auth.cs>cloudscribe.Core.Web.Integration\content\App_Start\Startup.Auth.cs.pp

echo on
::ECHO.%ERRORLEVEL%

call BatchSubstitute.cmd "cloudscribe.WebHost" "$rootnamespace$" ..\src\cloudscribe.WebHost\App_Start\WebApiConfig.cs>cloudscribe.Core.Web.Integration\content\App_Start\WebApiConfig.cs.pp

echo on
::ECHO.%ERRORLEVEL%

::xcopy ..\src\cloudscribe.WebHost\Controllers\* cloudscribe.Core.Web.Integration\content\Controllers /s /y /d

call BatchSubstitute.cmd "cloudscribe.WebHost" "$rootnamespace$" ..\src\cloudscribe.WebHost\Controllers\HomeController.cs>cloudscribe.Core.Web.Integration\content\Controllers\HomeController.cs.pp

echo on
::ECHO.%ERRORLEVEL%

xcopy ..\src\cloudscribe.WebHost\Views\* cloudscribe.Core.Web.Integration\content\Views /s /y /d /EXCLUDE:cloudscribe.Core.Web.Integration.ExcludedViews.txt

::xcopy ..\src\cloudscribe.WebHost\DI\* cloudscribe.Core.Web.Integration\content\DI /s /y /d
::copy ..\src\cloudscribe.WebHost\DI\Autofac\*.* cloudscribe.Core.Web.Integration\content\DI\Autofac

call BatchSubstitute.cmd "cloudscribe.WebHost" "$rootnamespace$" ..\src\cloudscribe.WebHost\DI\CommonConventions.cs>cloudscribe.Core.Web.Integration\content\DI\CommonConventions.cs.pp

echo on
::ECHO.%ERRORLEVEL%

call BatchSubstitute.cmd "cloudscribe.WebHost" "$rootnamespace$" ..\src\cloudscribe.WebHost\DI\Autofac\Modules\MvcModule.cs>cloudscribe.Core.Web.Integration\content\DI\Autofac\Modules\MvcModule.cs.pp

echo on
::ECHO.%ERRORLEVEL%

call BatchSubstitute.cmd "cloudscribe.WebHost" "$rootnamespace$" ..\src\cloudscribe.WebHost\DI\Autofac\Modules\MvcSiteMapProviderModule.cs>cloudscribe.Core.Web.Integration\content\DI\Autofac\Modules\MvcSiteMapProviderModule.cs.pp

echo on
::ECHO.%ERRORLEVEL%

call BatchSubstitute.cmd "cloudscribe.WebHost" "$rootnamespace$" ..\src\cloudscribe.WebHost\DI\Autofac\Modules\CloudscribeCoreModule.cs>cloudscribe.Core.Web.Integration\content\DI\Autofac\Modules\CloudscribeCoreModule.cs.pp

echo on
::ECHO.%ERRORLEVEL%

SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

NuGet.exe pack cloudscribe.Core.Web.Integration\cloudscribe.Core.Web.Integration.nuspec -Version %pversion% -OutputDirectory "nupkgs"
