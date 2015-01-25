# Cloudscribe Core 

#### Why Start From Scratch?

Every web application or website project tends to need a certain amount of basic functionality, why build this over and over. When I start a new web project I want to use this functionality that is already built so I can jump right in on the specific feature I want to implement. 

Target Features:
* Setup/Upgrade System for running versioned upgrade scripts and triggering code execution for custom configuration code to plugin
* Site Definitions - multi-tenant based on first folder segment or host names
* Site is a conceptual container for users, permissions, and content. 
* Related sites mode setting allows shared users and roles across sites while still allowing permissions to be segmented/siloed by site.
* Authentication/Authorization Framework - Implementation of aspnet Identity sans Entity Framework
* User and Role Management, Claim Management?
* Extensible User Profile System - plugin key value pairs as in mojoPortal
* Application Permissions Table (SiteId, key, allowedRoles)
* Settings Table a place to store key values with a collection guid and site guid
* Plugable Routing System - only needed if attribute based routing doesn’t work
* System Logging - file or database error logging options built in
* System Profiling (via Glimpse) - only works in full trust hosting
* File Upload and storage file manager for user uploaded files
* Core Skinning - Custom RazorViewEngine, view resolver, conventions
* HtmlHelpers for CKeditor, TinyMce, Markdown, BBCode?
* Localization Support
