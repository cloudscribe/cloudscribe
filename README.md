# cloudscribe

cloudscribe is a related set of projects and components for building cross platform web applications on ASP.NET Core. Get the big picture at [cloudscribe.com](https://www.cloudscribe.com)

The foundational set of projects in this repository, known as cloudscibe Core, provides support for single tenant or multi tenant management of sites, users, and roles. The other main cloudscribe projects are [cloudscribe SimpleContent](https://github.com/joeaudette/cloudscribe.SimpleContent), [cloudscribe Navigation](https://github.com/joeaudette/cloudscribe.Web.Navigation), and [cloudscribe Pagination](https://github.com/joeaudette/cloudscribe.Web.Pagination). You can find the [full list of projects here](https://github.com/joeaudette?tab=repositories).

If you have questions or just want to be social, say hello in our gitter chat room. I try to monitor that room on a regular basis while I'm working, but if I'm not around you can leave a message.

[![Join the chat at https://gitter.im/joeaudette/cloudscribe](https://badges.gitter.im/joeaudette/cloudscribe.svg)](https://gitter.im/joeaudette/cloudscribe?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

#### Why Start From Scratch?

Every web application or website project tends to need a certain amount of basic functionality, why build this over and over?

If you start a new web application project in Visual Studio 2015 using the standard project templates, what you get is just a basic implementation for user accounts via ASP.NET Identity. Those templates don't provide you any method for creating administrative users or creating roles or managing users and user role membership. You would typically have to implement that stuff yourself, and if you are like me, you don't want to have to implement that stuff again and again on every project. cloudscribe.Core aims to provide that for you with careful, well thought out implementations that adhere to [OWASP web security guidelines](https://www.owasp.org/index.php/Main_Page).

To get started building your own features and applications with cloudscribe, [please see our StarterKit repository](https://github.com/joeaudette/cloudscribe.StarterKits)

##### Currently Working Features:
* Site Definitions - multi-tenant based on either first folder segment or host names, default is by folder segment for now mainly because it is easier to try that without DNS, it is just a config option so it is easy to change
* Site is a conceptual container for users, permissions, and content. 
* Related sites mode setting allows shared users and roles across sites while still allowing permissions to be segmented/siloed by site.
* Authentication/Authorization Framework - Multi-Tenant Implementation of aspnet Identity
* Multi-Tenant User and Role Management
* Integration with [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) providing management of users, clients and scopes. This brings us support for [Jwt](https://jwt.io/) as an alternative to cookie authentication so we can more readily support SPA (Single Page Application) style web apps as well as authentication from mobile devices.
* System Logging - optional implementation of ILogger that logs to the database, and a UI for viewing the log
* [Custom RazorViewEngine view resolver, conventions](https://github.com/joeaudette/cloudscribe/wiki/Customizing-Views-and-Display-Templates)
* TagHelper for bootstrap modal
* TagHelper for pagination via [cloudscribe.Web.Pagination project](https://github.com/joeaudette/cloudscribe.Web.Pagination)
* ViewComponent for navigation menus, breadcrumbs - via [cloudscribe.Web.Navigation project](https://github.com/joeaudette/cloudscribe.Web.Navigation)
* Unobtrusive js for CKeditor
* Localization Support - documentation coming soon
* For data access, supports Entity Framework Core with either MSSQL, MySql, or PostgreSql. [NoDb](https://github.com/joeaudette/NoDb) file system storage is also supported for small sites or proptypes.
* Data and IO operations are async all the way down
* This project aims to follow the [OWASP](https://www.owasp.org/index.php/Main_Page) Guidelines for best practices in security
* Need Content? Take a look at [cloudscribe.SimpleContent](https://github.com/joeaudette/cloudscribe.SimpleContent), a simple yet flexible content and blogging engine that works with cloudscribe Core

##### Planned Features:
* Implement options for Security Questions and Answers [per OWASP guidelines](https://www.owasp.org/index.php/Forgot_Password_Cheat_Sheet)
* Implement tracking of password hash history to support scenarios where re-using old passwords is not allowed
* Implement a more traditional ADO.NET data layer with MSSQL and stored procedures
* MongoDB Support - I am interested in trying to implement the data repositories with MongoDB and would welcome help with that. 
* Lots of miscellaneous smaller stuff

##### Screenshots

![administration menu screen shot](https://github.com/joeaudette/cloudscribe/raw/master/screenshots/admin-menu.png)

##### Keep In Touch

We are collecting email addresses for a potential newsletter in the future, depending on whether this project becomes popular. If you would like to subscribe to this possible future newsletter, please send an email to subscribe [at] cloudscribe.com with the subject line "subscribe"

If you are interested in consulting or other support services related to cloudscribe, please send an email to info [at] cloudscribe.com.

I'm also on twitter @cloudscribeweb and @joeaudette
