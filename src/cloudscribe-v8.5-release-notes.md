# cloudscribe v8.5 Release Notes

## 🎉 Major Licensing Change

### Commercial Components Now Free
- **Announcement**: All cloudscribe commercial components are now available for free use
- **Components Included**:
  - TalkAbout Commenting System
  - TalkAbout Forums
  - Membership Paywall
  - Newsletter Management
  - Forms and Surveys
  - Stripe Payment Integration
- **Impact**: License key requirements have been completely removed for all commercial components
- **Source Code**: Components remain proprietary (subject to potential future open-sourcing)
- **Benefit**: Full cloudscribe ecosystem now accessible without licensing barriers

## New Features

### 🏗️ Admin Application Restart
- **Feature**: Added capability for administrators to restart the application directly from the cloudscribe admin interface
- **Benefit**: Eliminates need for direct server access when application restart is required
- **Configuration**: Controlled via appsettings.json configuration boolean for security
- **Issue**: [#1102](https://github.com/cloudscribe/cloudscribe/issues/1102)

### 📝 Enhanced Summernote Editor

#### Element Path Display
- **Feature**: New element path breadcrumb display showing current cursor position in DOM hierarchy (similar to CKEditor)
- **Functionality**: Real-time updates with clickable breadcrumbs for easy navigation
- **Implementation**: Custom Summernote plugin with comprehensive HTML5 tag support
- **Issue**: [#1208](https://github.com/cloudscribe/cloudscribe/issues/1208)

#### Improved Link Behavior
- **Change**: Hyperlinks no longer open in new windows by default
- **Configuration**: Controlled via `linkTargetBlank: false` in summernote-config.json
- **Issue**: [#1209](https://github.com/cloudscribe/cloudscribe/issues/1209)

### 📰 RSS Feed Styling Support
- **Feature**: Added ability to style RSS feeds with custom CSS stylesheets
- **Implementation**: Support for XML stylesheet meta tags in RSS feeds
- **Functionality**: Automatic XSL and CSS file deployment with user override protection
- **Benefit**: RSS feeds can now match site branding and provide better user experience
- **Documentation**: New documentation available at https://www.cloudscribe.com/cloudscribesyndication
- **Issue**: [cloudscribe.Syndication #7](https://github.com/cloudscribe/cloudscribe.Syndication/issues/7)

## Enhancements

### 🔒 Enhanced Auto-Logout System
- **Improvement**: Resolved session timeout issues for users actively using JavaScript API endpoints
- **Features**: 
  - Server-side middleware for intelligent session activity tracking
  - Client-side JavaScript for cross-tab session management
  - Configurable timeout thresholds
- **Benefit**: Prevents unexpected logouts during active user workflows while maintaining security
- **Issue**: [#1204](https://github.com/cloudscribe/cloudscribe/issues/1204)

### 📊 System Information Improvements
- **Enhancement**: Updated System Information page to include previously missing packages
- **Added**: Compiled views, static files, integration packages, and Bootstrap components
- **Fixed**: Removed duplicate "cloudscribe.Email.Templating.Web" entry
- **Benefit**: Improved visibility for troubleshooting and support scenarios
- **Issue**: [#698](https://github.com/cloudscribe/cloudscribe/issues/698)

## Bug Fixes

### 🔧 IdentityServer4 Support Resolution
- **Fixed**: Resolved token creation issues caused by dependency version conflicts
- **Root Cause**: System.IdentityModel.Tokens.Jwt version 8.2.* breaking changes
- **Solution**: Updated dependency chain management and explicit package references
- **Impact**: Restored proper JWT signature validation and metadata endpoint functionality
- **Issue**: [#1205](https://github.com/cloudscribe/cloudscribe/issues/1205)

### 📧 Email Queue Background Task Exception Handling
- **Fixed**: Resolved cancellation exception thrown during app pool recycling
- **Error**: "A task was canceled" in EmailQueueBackgroundTask.ExecuteAsync
- **Solution**: Improved cancellation token handling in background services
- **Impact**: Eliminates log noise during normal application lifecycle events
- **Issue**: [cloudscribe.Messaging #13](https://github.com/GreatHouseBarn/cloudscribe.Messaging/issues/13)

---
