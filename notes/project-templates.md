
http://www.natemcmaster.com/blog/2018/02/02/dotnet-global-tool/

https://packagesearch.azurewebsites.net/

https://github.com/dotnet/templating/wiki/Post-Action-Registry

https://www.cloudscribe.com/docs/advanced-client-side-development-with-webpack?utm_source=projecttemplate&utm_medium=referral&utm_campaign=newproject-vsix


https://marketplace.visualstudio.com/items?itemName=joeaudette.cloudscribeProjectTemplate
https://www.nuget.org/packages/cloudscribe.templates/1.0.0


## Commands to install and unintsall local project tempaltes

dotnet new --install C:\_c\cloudscribe.templates\Content\Solution\WebApp
dotnet new --install C:\_c\cloudscribe.templates\Content\WebApp
dotnet new --debug:reinit


## Commands to install/uninstall nuget dotnet new project templates

dotnet new -i "cloudscribe.templates::*"
dotnet new -u cloudscribe

# Command to pack the local project template into a nuget package

.\packtemplate.cmd


# Usage Samples

dotnet new cloudscribe -D MSSQL -I true -C true -K true

dotnet new cloudscribe -D MSSQL -I true -S z


https://github.com/dotnet/templating

https://github.com/dotnet/templating/wiki/Available-templates-for-dotnet-new

vs integration
Announcement: adding Template Engine templates to VS2017
https://github.com/dotnet/templating/issues/1209
https://github.com/ligershark/sidewafflev2
https://youtu.be/g6az_N95dVM

https://github.com/ligershark/sidewafflev2/issues/17#issuecomment-327677764
https://github.com/sayedihashimi/sayed-samples/tree/master/dotnet-templates/PassParameterFromWizard

https://docs.microsoft.com/en-us/visualstudio/extensibility/how-to-use-wizards-with-project-templates

Visual Studio Template Schema Reference
https://msdn.microsoft.com/en-us/library/xwkxbww4.aspx
http://www.windowsdevcenter.com/pub/a/windows/2007/06/06/developing-visual-studio-project-wizards.html
http://www.neovolve.com/2011/07/19/pitfalls-of-cancelling-a-vsix-project-template-in-an-iwizard/
Walkthrough: Publishing a Visual Studio Extension
https://msdn.microsoft.com/en-us/library/ff728613.aspx

https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new

https://blogs.msdn.microsoft.com/dotnet/2017/04/02/how-to-create-your-own-templates-for-dotnet-new/

https://github.com/dotnet/templating/wiki/%22Runnable-Project%22-Templates

template engine
https://github.com/dustinmoris/Giraffe/tree/master/template

template samples
https://github.com/dotnet/dotnet-template-samples

asp.net templates 
https://github.com/aspnet/Templates

spa templates 
https://github.com/aspnet/JavaScriptServices/tree/dev/templates

https://rehansaeed.com/custom-project-templates-using-dotnet-new/

https://rehansaeed.com/dotnet-new-feature-selection/

https://github.com/dotnet/templating/blob/cb9edbfe02c038a306fbcb6bbe162462d5fb59f0/src/Microsoft.TemplateEngine.Orchestrator.RunnableProjects/Config/ConditionalConfig.cs



dotnet new --install /Users/sayedhashimi/Documents/mycode/dotnet-new-samples/01-basic-template/SayedHa.StarterWeb


PS C:\_c\template-tests> dotnet new cloudscribe --help
Usage: new [options]

Options:
  -h, --help          Displays help for this command.
  -l, --list          Lists templates containing the specified name. If no name is specified, lists all templates.
  -n, --name          The name for the output being created. If no name is specified, the name of the current directory is used.
  -o, --output        Location to place the generated output.
  -i, --install       Installs a source or a template pack.
  -u, --uninstall     Uninstalls a source or a template pack.
  --type              Filters templates based on available types. Predefined values are "project", "item" or "other".
  --force             Forces content to be generated even if it would change existing files.
  -lang, --language   Specifies the language of the template to create.


cloudscribe project template (C#)
Author: Joe Audette
Options:
  -T|--Title           The name of the project which determines the assembly product name.
                       string - Optional
                       Default: Project Title

  -S|--SimpleContent   Include cloudscribe SimpleContent blog and content engine.
                       bool - Optional
                       Default: true

  -I|--IdentityServer  Include IdentityServer4 (fork) integration.
                       bool - Optional
                       Default: false

  -L|--Logging         Include cloudscribe logging and log viewer UI.
                       bool - Optional
                       Default: true

  -D|--DataStorage     The data storage platform you wish to use.
                           NoDb     - A no-database file system storage
                           MSSQL    - Microsoft SqlServer storage using Entity Framework Core
                           pgsql    - PostgreSql storage using Entity Framework Core
                           MySql    - MySql storage using Entity Framework Core
                       Default: MSSQL


PS C:\_c\template-tests>