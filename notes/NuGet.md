
https://blog.nuget.org/20180522/Introducing-signed-package-submissions.html


http://blog.nuget.org/20170417/Package-identity-and-trust.html
https://github.com/NuGet/Home/issues/1882


https://docs.microsoft.com/en-us/nuget/create-packages/symbol-packages
www.symbolsource.org/Public
Package consumers can then add https://nuget.smbsrc.net/ to their symbol sources in Visual Studio. This allows consumers to step into your package code in the Visual Studio debugger.


Targets
https://github.com/dotnet/corefx/blob/master/Documentation/architecture/net-platform-standard.md
https://blogs.infosupport.com/net-platform-standard-and-the-magic-of-imports/

not true
http://blog.nuget.org/20160126/nuget-contentFiles-demystified.html
https://github.com/NuGet/Home/issues/2262
https://github.com/NuGet/Home/issues/1908

https://github.com/aspnet/Home/issues/1519#issuecomment-223031744

https://github.com/aspnet/Mvc/issues/3750

"files": {
            "mappings": {
                "content/Views/Foo/": "Views/**"
            }
        }

"publishOptions": {
        "includeFiles": [
            "content/Views/Foo/Index.cshtml"
        ],
        "mappings": {
            "Views/Foo/Index.cshtml": "content/Views/Foo/Index.cshtml"
        }
    },


https://blogs.msdn.microsoft.com/webdev/2016/05/16/announcing-asp-net-core-rc2/
RC2 breaking changes
https://github.com/aspnet/announcements/issues?q=is%3Aissue+milestone%3A1.0.0-rc2

Updates to project.json schema #175 RC2
https://github.com/aspnet/Announcements/issues/175

"The contentFiles section of the nuspec is consumed by the client during restore, not during pack time."
"Items inside metadata are used by the client, items outside such as files are used at pack time."


It seems in the latest nuget used with VS 2015 and beta8 tooling, we can now package content files in the .npkg file created by VS 2015 when a project is built.
However, it seems also that the content files do NOT currently get installed in the consuming web app project.
This is a problem because we want to package cloudscribe.Web.Navigation with some views that are needed to use it.
Currently we can package the files but they do not get added to the web project that takes a nuget dependency on the package.

These are related links:
http://stackoverflow.com/questions/31412210/how-to-include-views-in-asp-net-5-class-library-nuget-package

https://docs.nuget.org/Consume/ProjectJson-Intro

https://github.com/NuGet/Home/issues/1523
https://github.com/NuGet/Home/wiki/Bringing-back-content-support,-September-24th,-2015
https://github.com/NuGet/Home/issues/627

https://docs.nuget.org/create/versioning

https://stackoverflow.com/questions/34611919/how-to-package-a-portable-net-library-targeting-net-core


These notes are way out of date and written when I was doing early prototype work in MVC 5

now in ASP.NET 5/MVC 6 nuget packages are creating by simply building the solution.
coming in beta8 we should be able to include content files such as views in the nuget package for a class library.

---- old information below --------------------------------------------------------


The nuget folder has batch files that can build the nuget packages.
Each batch file can take the package version as a command argument.

Before packaging the solution must be built in Release mode.

Then all packages can be created using the command

all.cmd 1.0.0-alpha1

Note that nuget packages are meant to be immutable, that is once a package has been created it should not change, any update should be done using a newer version for the package. This can be a challenge when you are first working on packaging because you can end up needing to raise the version sooner than you really wanted to. It is possible when using a private feed to replace a package while still keeping the same version.
In VS you would need to go to Tools > NuGet Package Manager > Package Manager Settings to clear the package cache.
Then you may also go to Tools > NuGet Package Manager > Package Manager Settings > Package Sources
click on the custom package feed to highlight it, then click the update button which I "think" will make it reload the feed.

The batch commands copy some files for packaging and then call NuGet pack command pointing to the nuspec file for the project and passing in the version.
Each project has a sub folder under the nuget folder that contains its nuspec file. The nuspec file has meta data and dependency information for the package.

One strange problem I encountered: I wanted to package cloudscribe.Resources in its own nuget package but when I did that it never would copy the dll from the package into the bin folder of the target project where it was installed, and it also did not add a reference to the project references.
I then tried including the cloudscribe.Resources.dll inside the cloudscribe.Core.Web package, but it still failed to get copied to the bin or a reference. Finally when I also included cloudscribe.Resources.pdb for some reason that made it deploy the dll to the bin and add the reference. Then I tried to go back to a separate package for cloudscribe.Resources thinking that by adding the .pdb file to its own package it would work as it did when packaged as part of cloudscribe.Core.Web, but that experiment failed and I was back to the same problem, so currently I still have to package it as part of cloudscribe.Core.Web. I suspect this is an obscure bug in nuget. I know there will be changes in nuget coming in .net vnext so I can revisit that later. It would be ideal to be able to update the resources independently to add new translations.


below are articles I've bookmarked here while learning about nuget and how to make packages

http://docs.nuget.org/consume/command-line-reference

http://geekswithblogs.net/TimothyK/archive/2014/03/27/what-is-nuget.aspx

https://www.nuget.org/
https://www.myget.org/

http://docs.nuget.org/docs/creating-packages/creating-and-publishing-a-package

http://docs.nuget.org/docs/creating-packages/Using-a-GUI-to-build-packages

http://geekswithblogs.net/TimothyK/archive/2014/03/27/creating-nuget-packages.aspx

http://haacked.com/archive/2011/10/24/semver-nuget-nightly-builds.aspx/

http://haacked.com/archive/2014/04/15/nuget-build-dependencies/

http://stackoverflow.com/questions/15882770/creating-one-nuget-package-from-multiple-projects-in-one-solution

http://blog.davidebbo.com/2011/04/easy-way-to-publish-nuget-packages-with.html

http://docs.nuget.org/docs/creating-packages/hosting-your-own-nuget-feeds

not sure I agree with this but interesting
https://blog.ometer.com/2017/01/10/dear-package-managers-dependency-resolution-results-should-be-in-version-control/
