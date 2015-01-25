Cloudscribe Core - Web Application Framework - Why Start From Scratch?

Every web application or website project tends to need a certain amount of basic functionality, why build this over and over. When I start a new web project I want to use this functionality that is already built so I can jump right in on the specific feature I want to implement. 

Ideally I would either have a VS project template that would pull in all this functionality or at minimum there could be NuGet packages and documentation to setup their use. None of the reusable code should live directly in the web app project, it should all be in libraries and the developer should have complete ownership of the web app project. So the idea is that you could start with a standard MVC project and then pull in all the needed Nuget packages and minimal code will be needed within the web app project to integrate and wire it up. 

Views would need to ultimately live within the web app file system?, so we will need to organise the views and have a deployment strategy to put them in place (either Nuget or VS template?). Then the developer is free to implement whatever he wants within the web app project.

Target Features:
Setup/Upgrade System for running versioned upgrade scripts and triggering code execution for custom configuration code to plugin
Site Definitions - multi-tenant based on first folder segment or host names
Site is a conceptual container for users, permissions, and content. 
Related sites mode setting allows shared users and roles across sites while still allowing permissions to be segmented/siloed by site.
Authentication/Authorization Framework - Implementation of aspnet Identity sans Entity Framework
User and Role Management, Claim Management? 
Extensible User Profile System - plugin key value pairs as in mojoPortal
Application Permissions Table (SiteId, key, allowedRoles)
Settings Table a place to store key values with a collection guid and site guid
Plugable Routing System - only needed if attribute based routing doesn’t work
Redirect System
System Logging - file or database error logging options built in
System Profiling (via Glimpse) - only works in full trust hosting
Task Framework 
File Upload and storage file manager for user uploaded files
Core Skinning - Custom RazorViewEngine, view resolver, conventions
HtmlHelpers for CKeditor, TinyMce, Markdown, BBCode?
Localization Support

Project Development Principles and Strategy:
Separation of Concerns
DRY - Don't repeat yourself
Don't force a specific OR Mapper on anyone who uses this. By not using an OR Mapper for cloudscribe we leave it to others what OR Mapper, if any, to use in their own projects built on top of cloudscribe core
Migrate or refactor as much useful code from mojoPortal as possible
Borrow useful code from existing projects so long as the licensing is liberal enough
Though we are not currently doing Test Driven Development, it is a goal to write testable code and as time allows write tests for the most important code and more if possible. We could benefit from good community members who could bring more testing experience to the project and set some good examples for us.
