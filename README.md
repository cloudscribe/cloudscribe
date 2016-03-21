# cloudscribe

cloudscribe is a related set of projects and components for building cross platform web applications on ASP.NET Core

The foundational set of projects in this repository, known as cloudscibe Core, provides support for single tenant or multi tenant management of sites, users, and roles. The other main cloudscribe projects are [cloudscribe SimpleContent](https://github.com/joeaudette/cloudscribe.SimpleContent), [cloudscribe Navigation](https://github.com/joeaudette/cloudscribe.Web.Navigation), and [cloudscribe Pagination](https://github.com/joeaudette/cloudscribe.Web.Pagination). You can find the [full list of projects here](https://github.com/joeaudette?tab=repositories).

If you have questions or just want to be social, say hello in our gitter chat room. I try to monitor that room on a regular basis while I'm working, but if I'm not around you can leave a message.

[![Join the chat at https://gitter.im/joeaudette/cloudscribe](https://badges.gitter.im/joeaudette/cloudscribe.svg)](https://gitter.im/joeaudette/cloudscribe?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

#### Why Start From Scratch?

Every web application or website project tends to need a certain amount of basic functionality, why build this over and over?

If you start a new web application project in Visual Studio 2015 using the standard project templates, what you get is just a basic implementation for user accounts via ASP.NET Identity. Those templates don't provide you any method for creating administrative users or creating roles or managing users and user role membership. You would typically have to implement that stuff yourself, and if you are like me, you don't want to have to implement that stuff again and again on every project. cloudscribe.Core aims to provide that for you with careful, well thought out implementations that adhere to [OWASP web security guidelines](https://www.owasp.org/index.php/Main_Page).

Eventually you will be able to start with an empty ASP.NET Core project and pull in nuget packages for cloudscribe, but for now if you want to give it a test drive see 
[Working with the source code](https://github.com/joeaudette/cloudscribe/wiki/Working-with-the-source-Code). You can download the latest version of cloudscribe Core by clicking the download button on the right side of the page or forking or cloning this repo. 

This project is in early stages, not yet ready for mission critical scenarios, but certainly ready for prototyping new web applications. We are building on a new framework from  Microsoft that is itself still in preview and changing frequently. Primarily we are targeting the new lightweight cross platform [Core .NET Framework](https://github.com/dotnet/core) aka dnxcore50 so the goal of this project is to work on Windows, Linux, or OSX. We are also targeting dnx451 aka the full desktop framework but we are avoiding taking dependencies on things that only work on the full desktop framework because we want to remain cross platform, and the full desktop .NET framework only works on Windows.

##### Currently Working Features:
* Site Definitions - multi-tenant based on either first folder segment or host names, default is by folder segment for now mainly because it is easier to try that without DNS, it is just a config option so it is easy to change
* Site is a conceptual container for users, permissions, and content. 
* Related sites mode setting allows shared users and roles across sites while still allowing permissions to be segmented/siloed by site.
* Authentication/Authorization Framework - Mutli-Tenant Implementation of aspnet Identity without Entity Framework
* Multi-Tenant User and Role Management
* System Logging - implementation of ILogger that logs to the database, and a UI for viewing the log
* [Custom RazorViewEngine view resolver, conventions](https://github.com/joeaudette/cloudscribe/wiki/Customizing-Views-and-Display-Templates)
* TagHelper for bootstrap modal
* TagHelper for pagination via [cloudscribe.Web.Pagination project](https://github.com/joeaudette/cloudscribe.Web.Pagination)
* TagHelpers for navigation menus, breadcrumbs - via [cloudscribe.Web.Navigation project](https://github.com/joeaudette/cloudscribe.Web.Navigation)
* Unobtrusive js for CKeditor
* For data access, going forward we are working primarily with Entity Framework Core which has many advantages for rapid development, but a goal of this project is to not impose a specific ORM (Object Relational Mapper) on anyone, so we also intend to provide a variety of other data access options including MongoDB as well as more traditional ADO.NET, and of course anyone is free to implement other data platforms since they are abstracted in a way that makes it easy to swap implementations. Currently, in addition to the Entity Framework data layer, the project has ADO.NET repository implementations for MSSQL, MySql, PostgreSql, Firebird, SQLCe, and Sqlite. Those implementations are being maintained in a separate code repository, and may be at various stages of completion. We really could use some help keeing those data access layers up to date so please see the [cloudscribe.Core.Data code repository](https://github.com/joeaudette/cloudscribe.Core.Data) if you are able to help. Most of the database schema and data access code was originally re-purposed and refactored from the mojoportal project, and therefore provide a possibility to migrate from mojoPortal.
* async all the way down - vast majority of the data access is async except with file based databases such as Sqlite and sqlce
* This project aims to follow the [OWASP](https://www.owasp.org/index.php/Main_Page) Guidelines for best practices in security

##### Planned Features:
* Implement options for Security Questions and Answers [per OWASP guidelines](https://www.owasp.org/index.php/Forgot_Password_Cheat_Sheet)
* Implement tracking of password hash history to support scenarios where re-using old passwords is not allowed
* MongoDB Support - I am interested in trying to implement the data repositories with MongoDB and would welcome help with that. 
* Localization Support - waiting for runtime and tooling support which may be in rc2 of ASP.NET Core
* Caching - memory  cache and distributed cache options
* support for [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) both as an authentication/authorization client and a user store. This will bring us support for [Jwt](https://jwt.io/) as an alternative to cookie authentication so we can more readily support SPA (Single Page Application) style web apps as well as authentication from mobile devices.
* Lots of miscellaneous smaller stuff

##### Other things we'd like to see:
* If this project were to become popular then it would provide a way for many people to build things on top of it that are compatible with each other, making it easier to assemble various functionality within a site
* cloudscribe.Cms? Take a look at [cloudscribe.SimpleContent](https://github.com/joeaudette/cloudscribe.SimpleContent)

##### Keep In Touch

We are collecting email addresses for a potential newsletter in the future, depending on whether this project becomes popular. If you would like to subscribe to this possible future newsletter, please send an email to subscribe [at] cloudscribe.com with the subject line "subscribe"

If you are interested in consulting or other support services related to cloudscribe, please send an email to info [at] cloudscribe.com.

I'm also on twitter @cloudscribeweb and @joeaudette
