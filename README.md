# cloudscribe

cloudscribe is a related set of projects and components for building cross platform web applications on ASP.NET Core. Get the big picture at [cloudscribe.com](https://www.cloudscribe.com/docs/introduction)

The foundational set of projects in this repository, known as cloudscibe Core, provides support for single tenant or multi tenant management of sites, users, and roles. The other main cloudscribe project is [cloudscribe SimpleContent](https://www.cloudscribe.com/docs/cloudscribe-simplecontent). There are a lot of smaller useful libraries as well, you can find the [full list of projects here](https://www.cloudscribe.com/docs/complete-list-of-cloudscribe-libraries).

If you have questions or just want to be social, say hello in our gitter chat room. I try to monitor that room on a regular basis while I'm working, but if I'm not around you can leave a message.

### Build Status

| Windows  | Linux/Mac |
| ------------- | ------------- |
| [![Build status](https://ci.appveyor.com/api/projects/status/jt9c0022x3odacar/branch/master?svg=true)](https://ci.appveyor.com/project/joeaudette/cloudscribe/branch/master)  | [![Build Status](https://travis-ci.org/joeaudette/cloudscribe.svg?branch=master)](https://travis-ci.org/joeaudette/cloudscribe)  |

[![Join the chat at https://gitter.im/joeaudette/cloudscribe](https://badges.gitter.im/joeaudette/cloudscribe.svg)](https://gitter.im/joeaudette/cloudscribe?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

#### Why Start From Scratch?

Every web application or website project tends to need a certain amount of basic functionality, why build this over and over?

If you start a new web application project in Visual Studio using the standard project templates, what you get is just a basic implementation for user accounts via ASP.NET Identity. Those templates don't provide you any method for creating administrative users or creating roles or managing users and user role membership. You would typically have to implement that stuff yourself, and if you are like me, you don't want to have to implement that stuff again and again on every project. cloudscribe.Core aims to provide that for you with careful, well thought out implementations that adhere to [OWASP web security guidelines](https://www.owasp.org/index.php/Main_Page).

To get started building your own features and applications with cloudscribe, [please see our StarterKit repository](https://github.com/joeaudette/cloudscribe.StarterKits)

#### Documentation

[See the full documentation at cloudscribe.com](https://www.cloudscribe.com/docs) (work in progress)

[Introduction](https://www.cloudscribe.com/docs/introduction) - get the big picture

##### What Is Included?:

* Login and registration, with support for social authentication configured from the UI. With options for recaptcha on the login and registration pages
* Support for extra content on the login page
* Support for extra content and a terms of use section on the registration page. If you populate the terms of use then users will be required to check a box indicating that they accept the terms in order to register and login. Also if you change the terms later you can optionally force all users to re-accept the changed terms.
* User Management (optionally [multi-tenant](https://www.cloudscribe.com/docs/multi-tenant-support) user management) you can create and manage user accounts, create and manage roles and user role membership, and add custom claims to users all from the UI. You can optionally disable self serve user registration so that only users that you add are allowed. 
* If you change a user's role membership, the role cookie will be updated automatically so the changes are effective right away.
* If you lock a user account or delete a user, the user will be signed out automatically.
* [A theme system that supports both shared themes and per tenant themes](https://www.cloudscribe.com/docs/themes-and-web-design). You can set the theme from a dropdown list in Administration > Site Settings, and the starter kits have a bunch of bootstrap themes included, and you can also make your own themes.
* Support for "Site is Closed" - you can set a site as closed and users will not be able to navigate any pages in the site, they will only see the message you provide on the closed page. Users can still login but only members of the Administrators or Content Administrators roles will be allowed to navigate the site, all other users will be redirected to the closed message.
* You can optionally require a confirmed email address for users if you add SMTP settings for email. A confirmation email will be sent to the user and the user will not be able to login until they click the link to confirm their email address.
* You can optionally require approval of new accounts before a user can login, and you can get notification when new users register so you can decide whether to approve the account. There is a separate page to make it easy to find users who have not yet been approved or who have not yet confirmed their email address.
* If you setup social authentication, you can optionally make social authentication the only allowed way to sign in.
* You can configure SMS settings for Twilio, and then users can enable 2 factor authentication using their phone.
* There is a company information section where you can define company name, address, email etc, and then you could show that information in the footer for example by customizing the layout. SiteContext is already injected into the layout and the company information are just properties on that so you can wrap your own markup around whichever of those properties you want to show.
* Integration with [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) providing management of users, clients and scopes. This brings us support for [Jwt](https://jwt.io/) as an alternative to cookie authentication so we can more readily support SPA (Single Page Application) style web apps as well as authentication from mobile devices.
* [Localization Support](https://www.cloudscribe.com/docs/localization)
* For data access, supports Entity Framework Core with either MSSQL, MySql, or PostgreSql. [NoDb](https://github.com/joeaudette/NoDb) file system storage is also supported for small sites or proptypes.
* Data and IO operations are async all the way down
* This project aims to follow the [OWASP](https://www.owasp.org/index.php/Main_Page) Guidelines for best practices in security

##### Need Content? 

Take a look at [cloudscribe.SimpleContent](https://github.com/joeaudette/cloudscribe.SimpleContent), a simple yet flexible content and blogging engine that works with cloudscribe Core

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
