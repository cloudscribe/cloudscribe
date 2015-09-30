# cloudscibe.Core.Web

#### Why Start From Scratch?

Every web application or website project tends to need a certain amount of basic functionality, why build this over and over. When I start a new web project I want to use this functionality that is already built so I can jump right in on the specific feature I want to implement. 

Target Features:
* Setup/Upgrade System for running versioned upgrade scripts and triggering code execution for custom configuration code to plugin
* Site Definitions - multi-tenant based on first folder segment or host names
* Site is a conceptual container for users, permissions, and content. 
* Related sites mode setting allows shared users and roles across sites while still allowing permissions to be segmented/siloed by site.
* Authentication/Authorization Framework - Mutli-Tenant Implementation of aspnet Identity without Entity Framework
* Multi-Tenant Social Login middleware
* Multi-Tenant User and Role Management
* System Logging - implementation of ILogger that logs to the database
* System Profiling (via Glimpse) 
* Core Skinning - Custom RazorViewEngine, view resolver, conventions
* TagHelpers/HtmlHelpers for CKeditor, TinyMce, Markdown, BBCode?
* Localization Support
* Currently the project has repository implementations for MSSQL, MySql, PostgreSql, Firebird, SQLCe, and SQLite, however only MSSQL is currently supported under dnxcore50 because current ADO drivers for the other db platofrms are only working in dnx451
* Possibly later we could implement repositories based on Entity Framework 7 but a goal of the project for me is to not force a specific ORM on anyone.
