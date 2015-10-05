# cloudscibe.Core.Web

#### Why Start From Scratch?

Every web application or website project tends to need a certain amount of basic functionality, why build this over and over. When I start a new web project I want to use this functionality that is already built so I can jump right in on the specific feature I want to implement. 

If you want to give it a test drive see 
[Working with the source code](https://github.com/joeaudette/cloudscribe/wiki/Working-with-the-source-Code) 

This project is in early stages, not yet production ready, we are building on a new framework from  Microsoft that is itself still in beta and changing frequently. Primarily we are targeting the new lightweight cross platform [Core .NET Framework](https://github.com/dotnet/core) aka dnxcore50 so the goal of this project is to work on Windows, Linux, or Mac. We are also targeting dnx451 aka the full desktop framework but we are avoiding taking dependencies on things that only work in dnx451.

We are collecting email addresses for a potential newsletter in the future, depending on whether this project becomes popular. If you would like to subscribe to this possible future newsletter, please send an email to subscribe [at] cloudscribe.com with the subject line "subscribe"

##### Currently Working Features:
* Setup/Upgrade System for running versioned install and upgrade sql scripts
* Site Definitions - multi-tenant based on either first folder segment or host names, default is by folder segment for now mainly because it is easier to try that without DNS, it is just a config option so it is easy to change
* Site is a conceptual container for users, permissions, and content. 
* Related sites mode setting allows shared users and roles across sites while still allowing permissions to be segmented/siloed by site.
* Authentication/Authorization Framework - Mutli-Tenant Implementation of aspnet Identity without Entity Framework
* Multi-Tenant Social Login middleware
* Multi-Tenant User and Role Management
* System Logging - implementation of ILogger that logs to the database, and a UI for viewing the log
* [Custom RazorViewEngine view resolver, conventions](https://github.com/joeaudette/cloudscribe/wiki/Customizing-Views-and-Display-Templates)
* TagHelper for bootstrap modal
* TagHelpers/HtmlHelpers for navigation menus, breadcrumbs and pagination - via cloudscribe.Web.Navigation project which will be moved to it's own source code repository soon after aspnet beta8 when we can include views in the nuget built for a class library project
* Unobtrusive js for CKeditor
* async all the way down - vast majority of the data access is async except with file based databases such as SQLite and sqlce
* Currently the project has repository implementations for MSSQL, MySql, PostgreSql, Firebird, SQLCe, and SQLite, however only MSSQL is currently supported under dnxcore50 because current ADO drivers for the other db platforms are only working in dnx451. Most of the database shcema and data access code was re-purposed and refactored from the mojoportal project, there is still some refactoring and cleanup to be done. There are some data fields that may be removed or moved to new tables later.
* Possibly later we could implement repositories based on Entity Framework 7 but a goal of the project for me is to not force a specific ORM on anyone.

##### Planned Features:
* Localization Support - waiting for runtime and tooling support which may be in beta8 of asp.net 5
* Caching - memory  cache and distributed cache options
* TagHelpers to do data- annotations in support of our unobtrusive js for ckeditor
* System Profiling (via Glimpse) - pending news from the Glimpse team who are working now at Microsoft
* Lots of miscellaneous smaller stuff

##### Other things we'd like to see:
* If this project were to become popular then it would provide a way for many people to build things on top of it that are compatible with each other, making it easier to assemble various functionality within a site
* cloudscribe.Cms? I am giving serious consideration to building such a thing, but at the moment feel I need to work on some projects that will generate income for me, and there is still a lot of work to do in this project.
