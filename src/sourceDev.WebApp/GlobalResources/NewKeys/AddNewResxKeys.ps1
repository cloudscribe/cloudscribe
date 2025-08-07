#Requires -Version 5.0

<#
.SYNOPSIS
    Adds specific ResX keys from cloudscribe localization project to any target ResX file
.DESCRIPTION
    This script contains 151 embedded ResX keys from the cloudscribe string localization project
    and adds them to a target ResX file. It checks for duplicates and only adds keys that 
    don't already exist in the target file. The keys are embedded in the script so no 
    external source file is needed.
.PARAMETER TargetResxFile
    Path to the target ResX file where keys will be added
.EXAMPLE
    .\AddNewResxKeys.ps1 -TargetResxFile "C:\MyProject\Resources\MyApp.en-FR.resx"
.EXAMPLE
    .\AddNewResxKeys.ps1
#>

param(
    [Parameter(Mandatory=$false)]
    [string]$TargetResxFile
)

# Function to prompt for ResX file if not provided
function Get-TargetResxFile {
    if (-not $TargetResxFile) {
        Write-Host "Add CloudScribe ResX Keys - Target File Selection" -ForegroundColor Green
        Write-Host "=============================================" -ForegroundColor Green
        Write-Host ""
        
        do {
            $TargetResxFile = Read-Host "Enter the path to the target ResX file"
            if (-not $TargetResxFile) {
                Write-Host "Please enter a valid file path." -ForegroundColor Red
            }
        } while (-not $TargetResxFile)
    }
    return $TargetResxFile
}

# Get target file path
$TargetResxFile = Get-TargetResxFile

# Validate target ResX file exists
if (-not (Test-Path $TargetResxFile)) {
    Write-Error "Target ResX file not found at: $TargetResxFile"
    exit 1
}

Write-Host "Add CloudScribe ResX Keys - Processing" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host "Target ResX file: $TargetResxFile" -ForegroundColor Cyan
Write-Host "Keys to process: 151 embedded keys" -ForegroundColor Cyan
Write-Host ""

# Parse the target ResX file as XML
try {
    [xml]$targetXml = Get-Content $TargetResxFile -Encoding UTF8
    Write-Host "✅ Target ResX file loaded successfully" -ForegroundColor Green
}
catch {
    Write-Error "Failed to load target ResX file as XML: $($_.Exception.Message)"
    exit 1
}

# Define all 151 ResX keys as embedded data
$resxKeys = @(
    @{ Name = "Or submit the code provided in the email."; Value = "Or submit the code provided in the email."; Comment = "verified - 2025-08-07" },
    @{ Name = "You must enter the code sent to your email address"; Value = "You must enter the code sent to your email address"; Comment = "verified - 2025-08-07" },
    @{ Name = "Associate your {0} account"; Value = "Associate your {0} account"; Comment = "verified - 2025-08-07" },
    @{ Name = "You've successfully authenticated with <strong>{0}</strong>."; Value = "You've successfully authenticated with <strong>{0}</strong>."; Comment = "verified - 2025-08-07" },
    @{ Name = "Email or Username"; Value = "Email or Username"; Comment = "verified - 2025-08-07" },
    @{ Name = "Password must be different from current password"; Value = "Password must be different from current password"; Comment = "verified - 2025-08-07" },
    @{ Name = "2 Factor Enabled"; Value = "2 Factor Enabled"; Comment = "verified - 2025-08-07" },
    @{ Name = "Add Roles"; Value = "Add Roles"; Comment = "verified - 2025-08-07" },
    @{ Name = "Cancel"; Value = "Cancel"; Comment = "verified - 2025-08-07" },
    @{ Name = "Confirm Delete"; Value = "Confirm Delete"; Comment = "verified - 2025-08-07" },
    @{ Name = "Confirm Disable Two Factor Authentication"; Value = "Confirm Disable Two Factor Authentication"; Comment = "verified - 2025-08-07" },
    @{ Name = "Disable Two Factor Auth."; Value = "Disable Two Factor Auth."; Comment = "verified - 2025-08-07" },
    @{ Name = "Email an account activation link to the user"; Value = "Email an account activation link to the user"; Comment = "verified - 2025-08-07" },
    @{ Name = "Export Users"; Value = "Export Users"; Comment = "verified - 2025-08-07" },
    @{ Name = "Failed Logins"; Value = "Failed Logins"; Comment = "verified - 2025-08-07" },
    @{ Name = "Last Login"; Value = "Last Login"; Comment = "verified - 2025-08-07" },
    @{ Name = "Last Password Change"; Value = "Last Password Change"; Comment = "verified - 2025-08-07" },
    @{ Name = "Password Reset Email"; Value = "Password Reset Email"; Comment = "verified - 2025-08-07" },
    @{ Name = "Passwords must have at least one digit ('0'-'9')."; Value = "Passwords must have at least one digit ('0'-'9')."; Comment = "verified - 2025-08-07" },
    @{ Name = "Passwords must have at least one lowercase character ('a'-'z')."; Value = "Passwords must have at least one lowercase character ('a'-'z')."; Comment = "verified - 2025-08-07" },
    @{ Name = "Passwords must have at least one non alphanumeric character"; Value = "Passwords must have at least one non alphanumeric character"; Comment = "verified - 2025-08-07" },
    @{ Name = "Passwords must have at least one uppercase character ('A'-'Z')."; Value = "Passwords must have at least one uppercase character ('A'-'Z')."; Comment = "verified - 2025-08-07" },
    @{ Name = "Search Users"; Value = "Search Users"; Comment = "verified - 2025-08-07" },
    @{ Name = "Select"; Value = "Select"; Comment = "verified - 2025-08-07" },
    @{ Name = "Send"; Value = "Send"; Comment = "verified - 2025-08-07" },
    @{ Name = "Social Logins"; Value = "Social Logins"; Comment = "verified - 2025-08-07" },
    @{ Name = "Two Factor Authentication"; Value = "Two Factor Authentication"; Comment = "verified - 2025-08-07" },
    @{ Name = "User must change password"; Value = "User must change password"; Comment = "verified - 2025-08-07" },
    @{ Name = "Username is already in use"; Value = "Username is already in use"; Comment = "verified - 2025-08-07" },
    @{ Name = "View User Activity"; Value = "View User Activity"; Comment = "verified - 2025-08-07" },
    @{ Name = "123.0.0.0,Spamming website"; Value = "123.0.0.0,Spamming website"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "124.0.0.0"; Value = "124.0.0.0"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "124.1.1.1,Another reason"; Value = "124.1.1.1,Another reason"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Add New IP Address"; Value = "Add New IP Address"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Add one or more IP addresses here to prevent them accessing the site. If an IP address is both blocked and permitted, the permit will take precedence. Note that once any IP is specifically permitted, all other IPs are blocked unless specific blocks are defined here."; Value = "Add one or more IP addresses here to prevent them accessing the site. If an IP address is both blocked and permitted, the permit will take precedence. Note that once any IP is specifically permitted, all other IPs are blocked unless specific blocks are defined here."; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "AddThis.com ProfileId"; Value = "AddThis.com ProfileId"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "AliasId:"; Value = "AliasId:"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Allow email for login in addition to username"; Value = "Allow email for login in addition to username"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Allow user to change email address"; Value = "Allow user to change email address"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Are you sure you want to delete this IP address?"; Value = "Are you sure you want to delete this IP address?"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Auto logout time (in minutes, leave blank to disable)"; Value = "Auto logout time (in minutes, leave blank to disable)"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Blocked IP"; Value = "Blocked IP"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Blocked IP Address"; Value = "Blocked IP Address"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Blocked IP Address Range"; Value = "Blocked IP Address Range"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Browse Server"; Value = "Browse Server"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Bulk Upload IP Addresses"; Value = "Bulk Upload IP Addresses"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Check the system log for errors if the message does not arrive."; Value = "Check the system log for errors if the message does not arrive."; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Choose a site that you wish to clone the content from"; Value = "Choose a site that you wish to clone the content from"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Clear Logo"; Value = "Clear Logo"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Clone Existing Site"; Value = "Clone Existing Site"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Cookie Policy Summary"; Value = "Cookie Policy Summary"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Create From Source Site"; Value = "Create From Source Site"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Created Date"; Value = "Created Date"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Danger, be very cautious about the setting below. If you check this box you may not be able to login again. Verify that you have LDAP Authentication working and that at least one Administrator can login using LDAP before disabling database authentication. If you do get locked out the only way to fix it is to change this setting directly in the database."; Value = "Danger, be very cautious about the setting below. If you check this box you may not be able to login again. Verify that you have LDAP Authentication working and that at least one Administrator can login using LDAP before disabling database authentication. If you do get locked out the only way to fix it is to change this setting directly in the database."; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Danger, be very cautious about the setting below. If you want to use Social Authentication only, then you can disable database authentication, but make sure that you as administrator can login with a social account before disabling database authentication. You can easily get yourself locked out with this setting, and the only way to fix it is to change the setting directly in the database."; Value = "Danger, be very cautious about the setting below. If you want to use Social Authentication only, then you can disable database authentication, but make sure that you as administrator can login with a social account before disabling database authentication. You can easily get yourself locked out with this setting, and the only way to fix it is to change the setting directly in the database."; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Delete Confirmation"; Value = "Delete Confirmation"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Disable database authentication"; Value = "Disable database authentication"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Drop file here or click to browse device for file."; Value = "Drop file here or click to browse device for file."; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Email Api Endpoint"; Value = "Email Api Endpoint"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Email Api Key"; Value = "Email Api Key"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Email Sender"; Value = "Email Sender"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Email addresses (csv) to notify of new users"; Value = "Email addresses (csv) to notify of new users"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Enforce one active browser session per user"; Value = "Enforce one active browser session per user"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Footer Content"; Value = "Footer Content"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Google Analytics Tracking Id"; Value = "Google Analytics Tracking Id"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Header Content"; Value = "Header Content"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Hostname, or a comma separated list of hostnames"; Value = "Hostname, or a comma separated list of hostnames"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "LDAP / Active Directory Settings"; Value = "LDAP / Active Directory Settings"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "LDAP Domain / Base DN"; Value = "LDAP Domain / Base DN"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "LDAP Domain\username (Active Directory using sAMAccountName)"; Value = "LDAP Domain\username (Active Directory using sAMAccountName)"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "LDAP Password"; Value = "LDAP Password"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "LDAP Server Port"; Value = "LDAP Server Port"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "LDAP Server(s)"; Value = "LDAP Server(s)"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "LDAP User DN Format"; Value = "LDAP User DN Format"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "LDAP Username"; Value = "LDAP Username"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "LDAP Uses SSL"; Value = "LDAP Uses SSL"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Last Updated"; Value = "Last Updated"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Maximum invalid password attempts before account is locked"; Value = "Maximum invalid password attempts before account is locked"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Message"; Value = "Message"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Minimum required password length (the system will not allow a number less than seven)"; Value = "Minimum required password length (the system will not allow a number less than seven)"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "New Site Administrator"; Value = "New Site Administrator"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "New Site Name"; Value = "New Site Name"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "No IP addresses have been blocked."; Value = "No IP addresses have been blocked."; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "No IP addresses have been permitted."; Value = "No IP addresses have been permitted."; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Note that this installation is using related sites mode, so any new sites will use the same users and roles as the master site. Therefore the administrative user you are creating here will not really be able to login. The purpose of this user is just as a fail safe measure in case you later change the configuration to not use related sites mode, then you would be able to login with this account."; Value = "Note that this installation is using related sites mode, so any new sites will use the same users and roles as the master site. Therefore the administrative user you are creating here will not really be able to login. The purpose of this user is just as a fail safe measure in case you later change the configuration to not use related sites mode, then you would be able to login with this account."; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Number of days until password expires, 0 to disable"; Value = "Number of days until password expires, 0 to disable"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Ok"; Value = "Ok"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "OpenIdConnect Scopes (comma separated)"; Value = "OpenIdConnect Scopes (comma separated)"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Password Settings"; Value = "Password Settings"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Password expiry alert display (days)"; Value = "Password expiry alert display (days)"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Permitted IP"; Value = "Permitted IP"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Permitted IP Address"; Value = "Permitted IP Address"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Permitted IP Address Range"; Value = "Permitted IP Address Range"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Permitting one or more IP addresses here will automatically block access from all other IP addresses, unless specific blocked IP addresses or ranges have been defined. If an IP address is both blocked and permitted, the permit will take precedence."; Value = "Permitting one or more IP addresses here will automatically block access from all other IP addresses, unless specific blocked IP addresses or ranges have been defined. If an IP address is both blocked and permitted, the permit will take precedence."; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Please note that adding header or footer content can mess up the layout of your page and may require CSS changes to accomodate your content."; Value = "Please note that adding header or footer content can mess up the layout of your page and may require CSS changes to accomodate your content."; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Please select one"; Value = "Please select one"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Privacy Policy"; Value = "Privacy Policy"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Reason"; Value = "Reason"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Registration Permitted Top Level Domain(s)"; Value = "Registration Permitted Top Level Domain(s)"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Requested Site Folder Name is not available, please try another value"; Value = "Requested Site Folder Name is not available, please try another value"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Require cookie consent?"; Value = "Require cookie consent?"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Require passwords to have at least one digit"; Value = "Require passwords to have at least one digit"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Require passwords to have at least one lowercase character"; Value = "Require passwords to have at least one lowercase character"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Require passwords to have at least one non-alphanumeric character"; Value = "Require passwords to have at least one non-alphanumeric character"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Require passwords to have at least one uppercase character"; Value = "Require passwords to have at least one uppercase character"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Require users to configure 2 factor authentication. Recommended ONLY for extreme security needs not for most sites, because a user must install an authenticator app on their phone and take a picture of the QR code using the authenticator app to get an access code. Note this will only be enforced when using https and it won't be enforced for users in Administrators role. Presumably administrators can and will voluntarily enable 2 factor authentication, but we don't want administrators to get locked out."; Value = "Require users to configure 2 factor authentication. Recommended ONLY for extreme security needs not for most sites, because a user must install an authenticator app on their phone and take a picture of the QR code using the authenticator app to get an access code. Note this will only be enforced when using https and it won't be enforced for users in Administrators role. Presumably administrators can and will voluntarily enable 2 factor authentication, but we don't want administrators to get locked out."; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "SMTP Settings"; Value = "SMTP Settings"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Save Crop"; Value = "Save Crop"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Send Test Message"; Value = "Send Test Message"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Show Site Name Link in Header"; Value = "Show Site Name Link in Header"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Site Logo"; Value = "Site Logo"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Subject"; Value = "Subject"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Template Heading"; Value = "Template Heading"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Test"; Value = "Test"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Test LDAP Authentication"; Value = "Test LDAP Authentication"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "To CSV"; Value = "To CSV"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Upload IP Addresses"; Value = "Upload IP Addresses"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Usually 389, or 636 if using SSL"; Value = "Usually 389, or 636 if using SSL"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "You should verify that the LDAP settings are valid and working by testing with a valid LDAP username and password."; Value = "You should verify that the LDAP settings are valid and working by testing with a valid LDAP username and password."; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Your CSV file should be comma separated, starting with the IP Address or range, comma, reason for adding (optional), new line, repeat. E.g.:"; Value = "Your CSV file should be comma separated, starting with the IP Address or range, comma, reason for adding (optional), new line, repeat. E.g.:"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Your IP Address:"; Value = "Your IP Address:"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "uid=username,Base DN (Open LDAP / 389 Directory Server)"; Value = "uid=username,Base DN (Open LDAP / 389 Directory Server)"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "username@LDAP Domain (Active Directory using userPrincipalName)"; Value = "username@LDAP Domain (Active Directory using userPrincipalName)"; Comment = "verified - 2025-08-07 - SiteAdmin" },
    @{ Name = "Learn More"; Value = "Learn More"; Comment = "verified - 2025-01-27 - Shared" },
    @{ Name = "Revoke Cookie Consent"; Value = "Revoke Cookie Consent"; Comment = "verified - 2025-01-27 - Shared" },
    @{ Name = "Session Expiry"; Value = "Session Expiry"; Comment = "verified - 2025-01-27 - Shared" },
    @{ Name = "Toggle Side Menu"; Value = "Toggle Side Menu"; Comment = "verified - 2025-01-27 - Shared" },
    @{ Name = "user avatar"; Value = "user avatar"; Comment = "verified - 2025-01-27 - Shared" },
    @{ Name = "New password is required"; Value = "New password is required"; Comment = "verified - 2025-01-27" },
    @{ Name = "Change Email"; Value = "Change Email"; Comment = "verified - 2025-01-27" },
    @{ Name = "Change Email Address"; Value = "Change Email Address"; Comment = "verified - 2025-01-27" },
    @{ Name = "The site is not configured to allow email changing."; Value = "The site is not configured to allow email changing."; Comment = "verified - 2025-01-27" },
    @{ Name = "This user account is not currently approved."; Value = "This user account is not currently approved."; Comment = "verified - 2025-01-27" },
    @{ Name = "You must re-enter your password to change your email address."; Value = "You must re-enter your password to change your email address."; Comment = "verified - 2025-01-27" },
    @{ Name = "Delete Personal Data"; Value = "Delete Personal Data"; Comment = "verified - 2025-01-27" },
    @{ Name = "Delete data and close my account"; Value = "Delete data and close my account"; Comment = "verified - 2025-01-27" },
    @{ Name = "Deleting this data will permanently remove your account, and this cannot be recovered."; Value = "Deleting this data will permanently remove your account, and this cannot be recovered."; Comment = "verified - 2025-01-27" },
    @{ Name = "Disabling 2FA does not change the keys used in authenticator apps. If you wish to change the key used in an authenticator app you should {0}reset your authenticator keys{1}."; Value = "Disabling 2FA does not change the keys used in authenticator apps. If you wish to change the key used in an authenticator app you should {0}reset your authenticator keys{1}."; Comment = "verified - 2025-01-27" },
    @{ Name = "An Email Address is Required"; Value = "An Email Address is Required"; Comment = "verified - 2025-01-27" },
    @{ Name = "Download a two-factor authenticator app like Microsoft Authenticator for {0}Android{1} and {2}iOS{3} or Google Authenticator for {4}Android{5} and {6}iOS{7}."; Value = "Download a two-factor authenticator app like Microsoft Authenticator for {0}Android{1} and {2}iOS{3} or Google Authenticator for {4}Android{5} and {6}iOS{7}."; Comment = "verified - 2025-01-27" },
    @{ Name = "Scan the QR Code or enter this key {0} into your two factor authenticator app. Spaces and casing do not matter."; Value = "Scan the QR Code or enter this key {0} into your two factor authenticator app. Spaces and casing do not matter."; Comment = "verified - 2025-01-27" },
    @{ Name = "Clear Image"; Value = "Clear Image"; Comment = "verified - 2025-01-27" },
    @{ Name = "Don't forget to click the Update button below if you change your profile image."; Value = "Don't forget to click the Update button below if you change your profile image."; Comment = "verified - 2025-01-27" },
    @{ Name = "Download"; Value = "Download"; Comment = "verified - 2025-01-27" },
    @{ Name = "Email address:"; Value = "Email address:"; Comment = "verified - 2025-01-27" },
    @{ Name = "Not you?"; Value = "Not you?"; Comment = "verified - 2025-01-27" },
    @{ Name = "Other Personal Data"; Value = "Other Personal Data"; Comment = "verified - 2025-01-27" },
    @{ Name = "Personal Data"; Value = "Personal Data"; Comment = "verified - 2025-01-27" },
    @{ Name = "Profile Image/Avatar"; Value = "Profile Image/Avatar"; Comment = "verified - 2025-01-27" },
    @{ Name = "The administrators of this site require all users to enable 2 factor authentication to meet the security requirements of this site."; Value = "The administrators of this site require all users to enable 2 factor authentication to meet the security requirements of this site."; Comment = "verified - 2025-01-27" },
    @{ Name = "Your account contains personal data that you have given us. This page allows you to download or delete that data."; Value = "Your account contains personal data that you have given us. This page allows you to download or delete that data."; Comment = "verified - 2025-01-27" }
)

Write-Host "Embedded keys loaded: $($resxKeys.Count)" -ForegroundColor Cyan
Write-Host ""

# Get existing keys from target file for duplicate checking
$existingKeys = @{}
foreach ($dataElement in $targetXml.root.data) {
    if ($dataElement.name) {
        $existingKeys[$dataElement.name] = $true
    }
}

Write-Host "Existing keys in target file: $($existingKeys.Count)" -ForegroundColor Cyan
Write-Host ""

# Process each embedded key
$added = 0
$skipped = 0
$errors = 0

foreach ($keyData in $resxKeys) {
    $keyName = $keyData.Name
    
    # Check for duplicates
    if ($existingKeys.ContainsKey($keyName)) {
        Write-Host "⏭️ Skipping duplicate key: `"$($keyName.Substring(0, [Math]::Min($keyName.Length, 50)))$($keyName.Length -gt 50 ? '...' : '')`"" -ForegroundColor Yellow
        $skipped++
        continue
    }
    
    try {
        # Create new data element in target XML
        $newDataElement = $targetXml.CreateElement("data")
        $newDataElement.SetAttribute("name", $keyName)
        $newDataElement.SetAttribute("xml:space", "preserve")
        
        # Create value element
        $valueElement = $targetXml.CreateElement("value")
        $valueElement.InnerText = $keyData.Value
        $newDataElement.AppendChild($valueElement)
        
        # Create comment element
        $commentElement = $targetXml.CreateElement("comment")
        $commentElement.InnerText = $keyData.Comment
        $newDataElement.AppendChild($commentElement)
        
        # Add to target XML
        $targetXml.root.AppendChild($newDataElement)
        
        # Update existing keys tracking
        $existingKeys[$keyName] = $true
        
        Write-Host "✅ Added: `"$($keyName.Substring(0, [Math]::Min($keyName.Length, 50)))$($keyName.Length -gt 50 ? '...' : '')`"" -ForegroundColor Green
        $added++
    }
    catch {
        Write-Host "❌ Error adding key `"$keyName`": $($_.Exception.Message)" -ForegroundColor Red
        $errors++
    }
}

Write-Host ""
Write-Host "=== PROCESSING COMPLETE ===" -ForegroundColor Green
Write-Host "Keys added: $added" -ForegroundColor Green
Write-Host "Keys skipped (duplicates): $skipped" -ForegroundColor Yellow
Write-Host "Errors encountered: $errors" -ForegroundColor $(if ($errors -gt 0) { "Red" } else { "Green" })

# Save the updated target file if any keys were added
if ($added -gt 0) {
    try {
        # Create backup
        $backupPath = "$TargetResxFile.backup.$(Get-Date -Format 'yyyyMMdd-HHmmss')"
        Copy-Item $TargetResxFile $backupPath
        Write-Host ""
        Write-Host "📁 Backup created: $backupPath" -ForegroundColor Cyan
        
        # Save updated file
        $targetXml.Save($TargetResxFile)
        Write-Host "💾 Target ResX file updated successfully!" -ForegroundColor Green
        
        # Validate the updated file
        [xml]$validationXml = Get-Content $TargetResxFile -Encoding UTF8
        $finalKeyCount = $validationXml.root.data.Count
        Write-Host "📊 Final key count in target file: $finalKeyCount" -ForegroundColor Cyan
    }
    catch {
        Write-Error "Failed to save updated ResX file: $($_.Exception.Message)"
        exit 1
    }
} else {
    Write-Host ""
    Write-Host "ℹ️ No keys were added - target file unchanged" -ForegroundColor Blue
}

Write-Host ""
Write-Host "🎉 Script completed successfully!" -ForegroundColor Green

# Display summary
Write-Host ""
Write-Host "=== SUMMARY ===" -ForegroundColor White
Write-Host "Embedded keys: 151 CloudScribe localization keys"
Write-Host "Target: $TargetResxFile" 
Write-Host "Result: $added new keys added, $skipped duplicates skipped"
if ($added -gt 0) {
    Write-Host "Backup: $backupPath"
}