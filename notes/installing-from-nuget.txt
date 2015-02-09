
The use of NuGet as a delivery mechanism will be a huge advantage for cloudscribe that was not the case with mojoPortal or other older projects.

To understand the value proposition for cloudscribe.Core.Web, compare what you get vs just creating a new ASP.NET project and choosing the MVC template with web api. In that case you get some Entity Framework implementation of ASP.NET identity, you are able to register on the site and login but there is nothing provided for user or role management and no way to make your new user account an administrator. There is also no concept of multi tenancy, no concept of sites, or of users attached to sites; you would have to build that yourself.

Starting with an empty project and installing cloudscribe.Core.Web.Integration, you get an implementation of ASP.NET identity based on ADO.NET with working repository implementations for 6 different database platforms. There is built in management for multi-tenant sites, users and roles.

Create a new ASP.NET Web Application (Visual C#) project in Visual Studio (VS).
In the dialog for the project template choose Empty, don't check any of the checkboxes, and click OK.

In VS go to Tools > NuGet Package Manager > Package Manager Settings
In the left pane click on Package Sources.
In the top right click the "plus" to add a source.
At the bottom set the name as cloudscribe-test and set the source as
http://download.mojoportal.com/nug/nuget
Click OK to save and close the dialog.

Right click the empty web application project node in Solution Explorer, and choose Manage NuGet Packages...

In the left pane of Package Manager click Online, then click the feed for cloudscribe-test.
In the dropdown list at the top select "Include Prerelease".
You should see a lot of packages:- the one you want is named cloudscribe.Core.Web.Integration; it is on the second page.
Click the cloudscribe.Core.Web.Integration package to highlight it, then click Install.

It takes a while to install, you will be prompted to accept the license agreement for some of the packages, then it will take another while as it installs.

A readme file will be shown after successful installation that provides a few steps to complete the setup.
Basically you need to compile the solution, create an MSSQL database and set the connection string.
Or, if you prefer to use SQLCE, add this in Web.App.Settings.config
<add key="DbPlatform" value="sqlce"/>
then click your web project node in VS Solution Explorer and choose Add > Add ASP.NET Folder > App_Data
This will add the special App_Data folder to your project. The SqlCe database will be created there by the /Setup page.
