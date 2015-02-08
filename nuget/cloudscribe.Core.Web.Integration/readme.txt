Thanks for trying cloudscribe.Core!

This is an alpha release, not all planned features are implemented yet but some are, and we wanted to get our NuGet packaging worked out early on.

A couple of steps are remaining to get things running.

1. First build/compile the solution.

2. Next you need a database. This package shipped with support for both MSSQL and SqlCe.
By default it is configured for MSSQL/SqlAzure, and this is the best database platform to use for production use.
If you have a handy sql/sqlazure server available, create an empty database and set the connection string in the Web.AppSettings.config file

If you just want to kick the tires and play around on your dev machine, you will find it easier to just use SqlCe.
To do that just oopen the file Web.AppSettings.config and add this
<add key="DbPlatform" value="sqlce"/>
Next, right click your web project node in VS Solution Explorer and choose Add > Add ASP.NET Folder > App_Data
This will add the special App_Data folder to your project. The SqlCe database will be created there.


3. Launch the application in VS either debug mode or not, if it hits an error just click continue (it will normally hit errors since the db has not been setup yet). 

4. Once the web browser is open navigate to /Setup

5. After setup has completed you will see a link for the home page, click that, then you can login using
admin@admin.com and the password admin
Feel free to change the email and password but keep in mind things like password reset won't function until a mail server is enabled, so be careful not to forget the new password if you change it before that is working and tested with a different user than the admin user.

We also have support for other database platforms, you can install any of these additional items from NuGet:
cloudscribe.Core.Repositories.Firebird
cloudscribe.Core.Repositories.pgsql
cloudscribe.Core.Repositories.SQLite

To use one of those, install it, then edit the Startup.cs in the root of your project and change the dependency injection to use the chosen package for cloudscribe.Core repositories implementation.
Then set the appropriate connection string in the Web.AppSettings.config file

Note that for SqlCe and SQLite which are file based databases, the database will be created automatically by visiting the /Setup page the first time.
It is always safe to visit the /Setup page, it is also used for upgrades, so anytime you update to newer cloudscribe packages you should build again and visit the /Setup page.

Note that changes to the Web.AppSettings.config file are not automatically detected by ASP.NET but changes to Web.config are, so if you make changes in Web.AppSettings.config, you need to make a small edit also in Web.config (just types a space somewhere and save).

Note that the integration files added to the web project including global.asax, Startup.cs, and code in the DI, and App_Start folder are intended to be modifiable. For example you could wire up additional custom routes or change to use a different Dependency Injection. We used Autofac for dependency injection but in asp.net vnext there will be built in DI that we will probably change to use that, while still leaving it to you whether you want to use it or a different one of your own choosing. Also in vnext I think we will not use a gloabl.asax file at all as that is strongly tied to IIS and will not be part of the .NET Core (aka cloud optimized framework). In fact my understanding is the Core framework will not include System.Web nor System.Configuration so we will be making changes when vnext is ready to remove dependencies on those namespaces. There are new lighter weight and more flexible config options coming for example so the change from System.Configuration should not be painful, it will just be implementation changes in cloudscribe.Configuration