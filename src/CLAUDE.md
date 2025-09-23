# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is **cloudscribe Core**, an ASP.NET Core multi-tenant web application platform that provides comprehensive user management, authentication, and security features. It's a modular framework designed to eliminate the need to implement user management functionality repeatedly across projects.

## Common Development Commands

### Build and Test
- **Build solution**: `dotnet build cloudscribe.sln`
- **Run tests**: `dotnet test tests/cloudscribe.Tests/cloudscribe.UnitTests.csproj`
- **Integration tests**: `dotnet test tests/cloudscribe.IntegrationTests/cloudscribe.IntegrationTests.csproj`
- **Run development app**: `dotnet run --project src/sourceDev.WebApp/sourceDev.WebApp.csproj`

### Frontend Asset Management (sourceDev.WebApp)
- **Install npm dependencies**: `npm install` (in `src/sourceDev.WebApp/`)
- **Build CSS themes**: `gulp buildCustom1ThemeCss`
- **Watch for SCSS changes**: `gulp` (default task)

### Cake Build System
- **Build with Cake**: `./build.ps1` (Windows) or `./build.sh` (Linux/Mac)
- **Run specific task**: `./build.ps1 --target=Test`

## Architecture Overview

### Multi-Tenant Architecture
The platform supports three multi-tenancy modes:
- **Single Tenant**: Traditional single-site application
- **Host-based**: Multiple sites distinguished by hostname/domain
- **Folder-based**: Multiple sites under folder paths (e.g., `/site1/`, `/site2/`)

### Core Components Structure

#### **Models Layer** (`cloudscribe.Core.Models`)
- Domain models for sites, users, roles, geography, and event handlers
- Interfaces for data access patterns
- Configuration models for security policies

#### **Identity System** (`cloudscribe.Core.Identity`)
- Custom ASP.NET Core Identity integration with multi-tenancy
- Site-specific user stores and role managers
- OAuth/social authentication configuration
- LDAP authentication support (Windows and cross-platform)
- Security features: cookie management, two-factor auth, OIDC integration

#### **Data Storage Abstraction**
Multiple storage implementations supporting:
- **Entity Framework**: SQL Server, MySQL, PostgreSQL, SQLite (`cloudscribe.Core.Storage.EFCore.*`)
- **NoDb**: File-system JSON storage (`cloudscribe.Core.Storage.NoDb`)

#### **IdentityServer Integration** (`cloudscribe.Core.IdentityServerIntegration`)
- IdentityServer4 integration for JWT/OAuth flows
- Multi-tenant client/scope management
- API authentication setup
- Admin UI for managing IdentityServer configuration

#### **Web Layer** (`cloudscribe.Core.Web`)
- MVC controllers and Razor Pages for user management
- Theme system with Bootstrap 4/5 support
- Compiled views for different UI frameworks
- Static file management and optimization

### Configuration Architecture

The application uses a sophisticated configuration system located in `src/sourceDev.WebApp/Configuration/`:
- **DataProtection.cs**: Configures ASP.NET Core data protection
- **CloudscribeFeatures.cs**: Sets up cloudscribe-specific features and data storage
- **IdentityServerIntegration.cs**: Configures IdentityServer integration and CORS
- **Authorization.cs**: Defines authorization policies
- **Localization.cs**: Internationalization setup
- **RoutingAndMvc.cs**: MVC routing and endpoint configuration

### Database Migration Strategy

The platform handles database initialization through:
- **EF Core**: Automatic migrations via `CoreEFStartup.InitializeDatabaseAsync()`
- **NoDb**: JSON file initialization via `CoreNoDbStartup.InitializeDataAsync()`
- **IdentityServer data seeding**: Configurable client and scope initialization

### Theme System

Supports multiple UI frameworks with compiled views:
- Bootstrap 4 themes in `cloudscribe.Core.CompiledViews.Bootstrap4`
- Bootstrap 5 themes in `cloudscribe.Core.CompiledViews.Bootstrap5`
- Custom SCSS compilation via Gulp in `sourceDev.WebApp`

### Query Tool Feature

Includes an optional database query tool (`cloudscribe.QueryTool.*`) for administrative database access with proper security controls.

## Key Development Patterns

### Dependency Injection Setup
All feature registration happens through extension methods in configuration classes, following the pattern:
```csharp
services.SetupDataStorage(configuration, environment);
services.SetupCloudscribeFeatures(configuration);
services.SetupIdentityServerIntegration(/*...*/);
```

### Multi-Database Support
The platform abstracts data access to support multiple databases. When adding new features:
- Define interfaces in `cloudscribe.Core.Models`
- Implement EF Core version in `cloudscribe.Core.Storage.EFCore.Common`
- Create database-specific implementations in respective `cloudscribe.Core.Storage.EFCore.*` projects
- Implement NoDb version in `cloudscribe.Core.Storage.NoDb`

### Event-Driven Architecture
The system uses event handlers for cross-cutting concerns:
- **User lifecycle events**: 
  - `IHandleUserCreated` - After user creation
  - `IHandleUserPreUpdate` - Before user update  
  - `IHandleUserUpdated` - After user update
  - `IHandleUserPreDelete` - Before user deletion (validation/data capture)
  - `IHandleUserPostDelete` - **After successful user deletion** (cleanup operations)
  - `IHandleUserEmailConfirmed`, `IHandleUserEmailUpdated`
  - `IHandleUserAddedToRole`, `IHandleUserRemovedFromRole`
- **Site management events**: `IHandleSiteCreated`, `IHandleSiteUpdated`, `IHandleSitePreDelete`, etc.
- **Role management events**: `IHandleRoleUpdated`, `IHandleRoleDeleted`

#### User Deletion Event Handlers (Transactional Safety)
**Critical**: Use `IHandleUserPostDelete` for cleanup operations that should only occur after successful deletion:
- External system cleanup, file deletion, related data removal
- Only executes if `SiteUserManager.DeleteAsync()` succeeds
- Multiple handlers supported via dependency injection
- Error isolation - handler failures don't affect other handlers
- Automatic discovery: `services.AddScoped<IHandleUserPostDelete, MyCleanupHandler>();`

**Implementation Location**: `UserEvents` class in `cloudscribe.Core.Identity` orchestrates all handlers with proper error handling.

### Security-First Design
- Content Security Policy configuration
- OWASP compliance patterns
- IP address blocking/permitting
- Terms of service acceptance tracking
- GDPR compliance features

### Version Provider System (Service Collection Pattern)

The platform uses a sophisticated version tracking system that demonstrates the **Service Collection Pattern** for dependency injection:

#### Components
1. **`IVersionProvider` Interface**: Each module implements this to expose version information (Name, ApplicationId, CurrentVersion)

2. **Multiple Registration Pattern**: Components register their version providers individually:
   ```csharp
   services.AddScoped<IVersionProvider, CloudscribeCoreVersionProvider>();
   services.AddScoped<IVersionProvider, DataStorageVersionInfo>();
   services.AddScoped<IVersionProvider, QueryToolVersionProvider>();
   // ... many more registrations
   ```

3. **`VersionProviderFactory`** (Infrastructure Layer):
   - Receives ALL providers via constructor: `IEnumerable<IVersionProvider> versionProviders`
   - DI automatically collects all registered `IVersionProvider` instances
   - Provides storage (`VersionProviders` property) and lookup (`Get(name)` method)
   - Generic and reusable across any context needing version information

4. **`SystemInfoManager`** (Business Logic Layer):
   - Consumes `IVersionProviderFactory` to access providers
   - Adds domain-specific logic:
     - Separates core version from other components
     - Combines with system information (OS, Runtime, Database)
     - Filters and formats for UI presentation
   - Used by `SystemInfoController` for admin dashboard display

#### Data Flow
1. Each module registers its `IVersionProvider` implementation during startup
2. DI container collects all implementations when `IEnumerable<IVersionProvider>` is requested
3. `VersionProviderFactory` receives and stores the collection
4. `SystemInfoManager` uses factory to organize/present version data
5. `SystemInfoController` displays formatted information in admin UI

This pattern enables modular version tracking where each component independently reports its version without tight coupling.

### User Identity and Claims Access

cloudscribe provides extension methods in `cloudscribe.Core.Identity.ClaimsPrincipalExtensions` for accessing user information from the `ClaimsPrincipal`. **Always use these extension methods** instead of manually parsing claims:

#### Standard Extension Methods
```csharp
// Get user information from ClaimsPrincipal (User property in controllers)
var userId = User.GetUserId();                    // Gets NameIdentifier or "sub" claim
var userIdGuid = User.GetUserIdAsGuid();         // Converts userId to Guid
var email = User.GetEmail();                     // Gets "email" claim
var displayName = User.GetDisplayName();         // Gets "DisplayName" claim
var avatarUrl = User.GetAvatarUrl();             // Gets "AvatarUrl" claim

// For logging administrative actions
_log.LogWarning("Action performed - User: {DisplayName}, Email: {Email}, UserId: {UserId}, IP: {IpAddress}", 
    User.GetDisplayName() ?? User.Identity?.Name ?? "Unknown",
    User.GetEmail(),
    User.GetUserId(),
    HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown");
```

#### Role Checking Extension
```csharp
// Check if user is in comma or semicolon-separated role list
bool hasAccess = User.IsInRoles("Admin,ContentEditor");
bool hasAccess = User.IsInRoles("Admin;ContentEditor");  // Also supports semicolon
bool allUsers = User.IsInRoles("All Users");             // Special "All Users" role
```

#### Implementation Details
- `GetUserId()` first checks `ClaimTypes.NameIdentifier`, then falls back to "sub" claim (JWT scenarios)
- `GetUserIdAsGuid()` validates 36-character format and returns `Guid.Empty` for invalid values
- Extension methods handle null safety and return `null` for missing claims
- Located in: `src/cloudscribe.Core.Identity/ClaimsPrincipalExtensions.cs`

**Best Practice**: Use these extensions consistently in controllers, services, and middleware for user identity operations rather than direct claim access.

### View and Static File Architecture

cloudscribe follows a strict separation of concerns for views and static assets:

#### View Organization
- **Views belong in compiled view projects**: `cloudscribe.Core.CompiledViews.Bootstrap5`, not inline in controllers
- **No inline HTML in controllers**: Use proper MVC views with ViewModels
- **Compiled views become NuGet packages**: Views are distributed across multiple customer installations

#### JavaScript and CSS Organization  
- **All JavaScript/CSS in `cloudscribe.Web.StaticFiles`**: Centralized static asset management
- **Mark as `EmbeddedResource`**: All static files must be embedded resources for proper distribution
- **No inline JavaScript**: Always use separate `.js` files referenced via `<script src="">`
- **Environment-based loading**: Support both development (`script.js`) and production (`script.min.js`) versions

#### Implementation Pattern
```csharp
// ❌ Wrong - inline HTML in controller
return Content("<html>...</html>", "text/html");

// ✅ Correct - use proper view
return View("RestartingApplication", model);
```

```html
<!-- ❌ Wrong - inline script in view -->
<script>
  function doSomething() { ... }
</script>

<!-- ✅ Correct - reference external script -->
<script src="~/cr/js/restart-polling.min.js"></script>
```

#### Why This Matters
- **Multi-tenancy**: Views/scripts work across different customer configurations
- **Maintainability**: Centralized assets easier to update and debug  
- **Performance**: Proper caching and minification support
- **Distribution**: NuGet packages require embedded resources
- **Consistency**: Uniform approach across all cloudscribe features

**Key Rule**: If you're writing HTML strings or JavaScript in C# code, you're doing it wrong. Use the view system and static file architecture.

## Important Configuration Notes

- **Data Protection**: Review `Config/DataProtection.cs` for production deployment
- **Database Selection**: Configure via `appsettings.json` `DevOptions:DbPlatform` (ef/nodb)
- **IdentityServer**: Can be disabled via `AppSettings:DisableIdentityServer`
- **SSL Configuration**: Managed through `AppSettings:UseSsl`
- **Multi-tenancy Mode**: Set in `MultiTenantOptions.Mode`

This architecture enables rapid development of secure, multi-tenant web applications while maintaining flexibility for customization and extension.

## Summernote Element Path Plugin Implementation

### Overview
cloudscribe includes a custom Summernote plugin that displays an element path breadcrumb (similar to CKEditor's "body > h2 > strong" display) in the editor's status bar footer.

### Key Features
- **Real-time element path display**: Shows current cursor position in DOM hierarchy
- **Clickable breadcrumbs**: Click any element in path to select that element in editor
- **Comprehensive tag support**: 66 HTML tags including HTML5 semantic elements
- **Multi-editor support**: Works with multiple Summernote instances on same page
- **Source code mode awareness**: Hides gracefully when in HTML source editing mode
- **Responsive updates**: Updates immediately when using toolbar formatting, style dropdowns

### File Locations
- **Plugin**: `src/cloudscribe.Web.StaticFiles/js/summernote/plugin/elementpath/summernote-ext-elementpath.js`
- **Minified**: `src/cloudscribe.Web.StaticFiles/js/summernote/plugin/elementpath/summernote-ext-elementpath.min.js`
- **Config**: `src/cloudscribe.Web.StaticFiles/js/summernote-config.json`
- **View Reference**: `src/cloudscribe.Core.CompiledViews.Bootstrap5/Views/Shared/SummernoteScripts.cshtml`
- **Project File**: `src/cloudscribe.Web.StaticFiles/cloudscribe.Web.StaticFiles.csproj` (as EmbeddedResource)

### Configuration
Plugin behavior controlled via `summernote-config.json`:
```json
"elementPath": {
    "enabled": true,
    "separator": "  ",
    "clickToSelect": true,
    "showTags": ["P", "H1", "H2", "H3", "H4", "H5", "H6", "BLOCKQUOTE", 
                 "PRE", "LI", "OL", "UL", "TD", "TH", "TR", "TABLE", "DIV", 
                 "STRONG", "B", "EM", "I", "U", "S", "STRIKE", "SUP", "SUB", "A", "CODE", "SPAN",
                 "SMALL", "CITE", "CAPTION", "THEAD", "TBODY", "TFOOT", "ARTICLE", "DL", "DT", "DD",
                 "HEADER", "FOOTER", "MAIN", "SECTION", "ASIDE", "NAV", "FIGURE", "FIGCAPTION", 
                 "MARK", "HR", "FIELDSET", "LEGEND", "LABEL", "ADDRESS", "INPUT",
                 "DETAILS", "SUMMARY", "IFRAME", "AUDIO", "VIDEO", "SOURCE", "DIALOG", 
                 "PROGRESS", "METER", "OUTPUT", "CANVAS", "SVG"]
}
```

### Supported HTML Tags (66 total)
**Text Content**: P, H1-H6, BLOCKQUOTE, PRE, CODE, SPAN, STRONG, B, EM, I, U, S, STRIKE, SUP, SUB, A, SMALL, CITE, MARK
**Lists**: LI, OL, UL, DL, DT, DD
**Tables**: TABLE, TR, TD, TH, CAPTION, THEAD, TBODY, TFOOT
**HTML5 Semantic**: ARTICLE, SECTION, HEADER, FOOTER, MAIN, ASIDE, NAV, FIGURE, FIGCAPTION, ADDRESS
**Forms**: FIELDSET, LEGEND, LABEL, INPUT
**Interactive**: DETAILS, SUMMARY, DIALOG, PROGRESS, METER, OUTPUT
**Media**: AUDIO, VIDEO, SOURCE, CANVAS, SVG, IFRAME
**Structure**: DIV, HR

### Visual Styling
- **"body" element**: Light gray (#999) with 4px right padding
- **Other elements**: Dark gray (#303030), semi-bold font (600 weight)
- **Hover effect**: Light blue background (#e0e8f0)
- **Separator**: Double space between elements
- **Footer positioning**: 5px top padding, 8px left padding

### Technical Implementation Details

#### Plugin Architecture
- **UMD Pattern**: Universal Module Definition for AMD/CommonJS/Browser compatibility
- **jQuery Integration**: Extends `$.summernote.plugins`
- **Event-driven updates**: Responds to editor focus, clicks, keyboard, toolbar actions
- **Multi-instance safe**: Uses unique editor IDs to prevent conflicts

#### Selection and Click Handling
- **Element selection**: Uses `document.createRange()` and `window.getSelection()`
- **Summernote integration**: Calls `context.invoke('editor.saveRange')` to sync with editor
- **Event timing**: Various setTimeout delays (10-100ms) to handle browser timing issues
- **Toolbar responsiveness**: Special handling for style dropdown (H1, H2, etc.) selections

#### Source Code Mode Detection
- **Auto-hide**: Uses `visibility: hidden` when editor has `codeview` class
- **Height preservation**: Maintains footer height when hidden to prevent UI jumping
- **Automatic show/hide**: Responds to visual/source mode switching

#### Configuration Override Behavior
Config file settings **override** plugin defaults via `$.extend({}, defaultOptions, options.elementPath || {})`:
- Plugin provides fallback defaults if config missing
- Config file enables runtime customization without code changes
- Individual properties can be overridden (e.g., just change `showTags` array)

### Development Notes
- **Debug logging**: Commented out in source, single initialization log in minified version
- **Manual minification**: Minified version manually maintained, not auto-generated
- **EmbeddedResource requirement**: All static files must be marked as EmbeddedResource for NuGet distribution
- **Cross-browser compatibility**: Uses standard DOM APIs and jQuery for maximum compatibility

### Known Limitations
- **Sporadic first-click issue**: Occasionally first click on footer element doesn't select (subsequent clicks work)
- **AJAX activity timing**: Client-side path may lag behind rapid programmatic changes

## Cookie Consent System (Three-State Implementation)

### Architecture
cloudscribe now includes a three-state cookie consent system:
1. **Undecided** - No cookies, banner visible, no tracking
2. **Accepted** - Consent cookie set, banner hidden, tracking enabled  
3. **Declined** - Dismiss cookie set, banner hidden, tracking disabled

### Key Components
- **Consent Cookie**: `cookieconsent_status` (ASP.NET Core managed)
- **Dismiss Cookie**: `cookieconsent_dismissed` (custom implementation)
- **JavaScript Handler**: `cloudscribe-cookie-consent-three-state.js`
- **View**: `_CookieConsentPartial.cshtml` with Accept/Decline buttons
- **Controller Actions**: `ResetCookiePreferences`, `ShowCookieBanner`

### Critical Implementation Details
- **Static Files Must Be EmbeddedResource**: All JS/CSS in `cloudscribe.Web.StaticFiles` project
- **Cookie Path Consistency**: Always use `path=/` for dismiss cookie
- **SameSite Policy**: Recommend `SameSiteMode.Lax` not `None` for security
- **HTTPS Cookie Deletion**: Must match `Secure` flag between set/delete operations
- **Multi-Tenant Support**: Works with folder-based tenancy (`/s1`) and culture prefixes

### Recommended Configuration
```csharp
services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = cloudscribe.Core.Identity.SiteCookieConsent.NeedsConsent;
    options.MinimumSameSitePolicy = SameSiteMode.Lax; // NOT None
    options.ConsentCookie.Name = "cookieconsent_status";
});
```

### Framework Considerations
- Changes affect compiled views that become NuGet packages
- Cookie behavior must work across different customer configurations
- Environment-based script loading (dev vs production minified versions)

## Auto-Logout (Inactivity Timeout) System

### Overview
cloudscribe implements an automatic logout system that terminates user sessions after a configurable period of inactivity for security purposes.

### Configuration
- **Setting**: `MaximumInactivityInMinutes` in site settings (Security Settings admin page)
- **Implementation**: Sets authentication cookie with `ExpireTimeSpan` and `SlidingExpiration=true`
- **Location**: `cloudscribe.Core.Identity/SiteCookieAuthenticationOptions.cs:89-90`

### Key Components
1. **Client-Side JavaScript** (`cloudscribe-autologout-warning.js`)
   - Initializes countdown based on initial server-provided session time
   - Shows warning modal when < 60 seconds remain
   - Cannot re-poll server without extending session (see architectural flaw below)

2. **Server-Side Components**
   - `RemainingSessionTimeResolver.cs`: Calculates remaining time via `AuthenticateAsync()`
   - `AccountController.RemainingSessionTime()`: Returns seconds remaining (extends session as side-effect)
   - `AccountController.AutoLogoutNotification()`: Shows "timed out" page

3. **Warning Modal** (`_AutoLogoutWarningPartial.cshtml`)
   - Displays countdown seconds
   - "Stay logged in" button - refreshes session via AJAX
   - "Log out" button - immediate logout

### Critical Architectural Flaw: The Observer Effect
**The system has a fundamental catch-22**: Checking remaining session time via `RemainingSessionTimeResolver` calls `AuthenticateAsync()`, which automatically refreshes the sliding expiration. This means:
- You cannot check session status without extending it
- JavaScript can only get initial time at page load
- Client-side countdown becomes disconnected from server reality

### Known Weaknesses

#### 1. AJAX Activity Not Recognized as Activity
- **Problem**: AJAX API calls extend server-side session, but client-side countdown doesn't know
- **Result**: Users logged out despite active AJAX usage
- **Root Cause**: Cannot poll server for updated time without extending session

#### 2. Multi-Tab Session Conflicts
- **Problem**: Each browser tab runs independent countdown timer
- **Result**: Inactive tab logs user out even if active in another tab
- **Root Cause**: Tabs cannot check real session status without extending it

#### 3. Architecture Unsuitable for Modern Web Apps
The assumption that "checking session = activity" makes the system poorly suited for:
- Single-page applications with heavy AJAX usage
- Multi-tab browser usage patterns
- Accurate client-server session synchronization

### Potential Solutions (See AUTO-LOGOUT-ANALYSIS.md for details)
1. **Read-Only Session Check**: Endpoint that checks expiry without calling `AuthenticateAsync()`
2. **Activity Tracking Middleware**: Decouple activity tracking from authentication cookie
3. **Cross-Tab Communication**: Use localStorage events for tab coordination
4. **WebSocket/SignalR**: Push session updates to all tabs
5. **Different Cookie Strategy**: Fixed expiration with explicit refresh

### Development Considerations
- Test with multiple browser tabs when modifying
- Consider AJAX-heavy workflows in testing
- Be aware that any call to `RemainingSessionTime` endpoint extends the session
- The warning modal's "Stay logged in" button may need multiple attempts to refresh session