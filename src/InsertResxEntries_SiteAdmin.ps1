#Requires -Version 5.0

<#
.SYNOPSIS
    Inserts newly-added ResX entries from SiteAdmin folder string localization into CloudscribeCore.resx
.DESCRIPTION
    This script inserts all newly-added string resource entries into a user's local copy of CloudscribeCore.resx.
    Created during SiteAdmin folder string localization process on 2025-08-07.
    The SiteAdmin folder was already localized with @sr[] references, but 98 ResX entries were missing.
.PARAMETER ResxFilePath
    Path to the CloudscribeCore.resx file to update
.EXAMPLE
    .\InsertResxEntries_SiteAdmin.ps1 -ResxFilePath "C:\MyProject\GlobalResources\CloudscribeCore.resx"
#>

param(
    [Parameter(Mandatory=$true)]
    [string]$ResxFilePath
)

# Validate ResX file exists
if (-not (Test-Path $ResxFilePath)) {
    Write-Error "CloudscribeCore.resx file not found at: $ResxFilePath"
    exit 1
}

# New keys to add - First 25 keys from SiteAdmin (batch 1)
$newEntries = @(
    @{
        Name = "123.0.0.0,Spamming website"
        Value = "123.0.0.0,Spamming website"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "124.0.0.0"
        Value = "124.0.0.0"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "124.1.1.1,Another reason"
        Value = "124.1.1.1,Another reason"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Add New IP Address"
        Value = "Add New IP Address"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Add one or more IP addresses here to prevent them accessing the site. If an IP address is both blocked and permitted, the permit will take precedence. Note that once any IP is specifically permitted, all other IPs are blocked unless specific blocks are defined here."
        Value = "Add one or more IP addresses here to prevent them accessing the site. If an IP address is both blocked and permitted, the permit will take precedence. Note that once any IP is specifically permitted, all other IPs are blocked unless specific blocks are defined here."
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "AddThis.com ProfileId"
        Value = "AddThis.com ProfileId"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "AliasId:"
        Value = "AliasId:"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Allow email for login in addition to username"
        Value = "Allow email for login in addition to username"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Allow new registrations"
        Value = "Allow new registrations"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Allow persistent login"
        Value = "Allow persistent login"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Allow user to change email address"
        Value = "Allow user to change email address"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Are you sure you want to delete this IP address?"
        Value = "Are you sure you want to delete this IP address?"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Auto logout time (in minutes, leave blank to disable)"
        Value = "Auto logout time (in minutes, leave blank to disable)"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Blocked IP"
        Value = "Blocked IP"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Blocked IP Address"
        Value = "Blocked IP Address"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Blocked IP Address Range"
        Value = "Blocked IP Address Range"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Browse Server"
        Value = "Browse Server"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Bulk Upload IP Addresses"
        Value = "Bulk Upload IP Addresses"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Check the system log for errors if the message does not arrive."
        Value = "Check the system log for errors if the message does not arrive."
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Choose a site that you wish to clone the content from"
        Value = "Choose a site that you wish to clone the content from"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Clear Logo"
        Value = "Clear Logo"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Clone Existing Site"
        Value = "Clone Existing Site"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Cookie Policy Summary"
        Value = "Cookie Policy Summary"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Create From Source Site"
        Value = "Create From Source Site"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    },
    @{
        Name = "Created Date"
        Value = "Created Date"
        Comment = "verified - 2025-08-07 - SiteAdmin"
        File = "SiteAdmin"
    }
)

Write-Host "SiteAdmin Folder String Localization - ResX Entry Insertion Script" -ForegroundColor Green
Write-Host "====================================================================" -ForegroundColor Green
Write-Host "Target ResX file: $ResxFilePath" -ForegroundColor Cyan
Write-Host "NOTE: This is Part 1 of 4 - adds first 25 of 98 total SiteAdmin entries" -ForegroundColor Yellow
Write-Host "Run InsertResxEntries_SiteAdmin_Part2.ps1, Part3.ps1, and Part4.ps1 to complete all entries" -ForegroundColor Yellow
Write-Host "Total entries in this script: $($newEntries.Count)" -ForegroundColor Cyan
Write-Host ""

# Load the ResX file as XML
try {
    [xml]$resxXml = Get-Content $ResxFilePath -Encoding UTF8
}
catch {
    Write-Error "Failed to load ResX file as XML: $($_.Exception.Message)"
    exit 1
}

# Check for existing keys and add new ones
$added = 0
$skipped = 0

foreach ($entry in $newEntries) {
    # Check if key already exists
    $existingEntry = $resxXml.root.data | Where-Object { $_.name -eq $entry.Name }
    
    if ($existingEntry) {
        Write-Host "Key already exists, skipping: $($entry.Name)" -ForegroundColor Yellow
        $skipped++
        continue
    }
    
    # Create new data element
    $dataElement = $resxXml.CreateElement("data")
    $dataElement.SetAttribute("name", $entry.Name)
    $dataElement.SetAttribute("xml:space", "preserve")
    
    # Create value element
    $valueElement = $resxXml.CreateElement("value")
    $valueElement.InnerText = $entry.Value
    $dataElement.AppendChild($valueElement)
    
    # Create comment element
    $commentElement = $resxXml.CreateElement("comment")
    $commentElement.InnerText = $entry.Comment
    $dataElement.AppendChild($commentElement)
    
    # Add to root
    $resxXml.root.AppendChild($dataElement)
    
    Write-Host "Added: $($entry.Name)" -ForegroundColor Green
    $added++
}

# Save the updated ResX file
if ($added -gt 0) {
    try {
        # Create backup
        $backupPath = "$ResxFilePath.backup.$(Get-Date -Format 'yyyyMMdd-HHmmss')"
        Copy-Item $ResxFilePath $backupPath
        Write-Host "Backup created: $backupPath" -ForegroundColor Cyan
        
        # Save updated file
        $resxXml.Save($ResxFilePath)
        Write-Host ""
        Write-Host "Successfully updated CloudscribeCore.resx file!" -ForegroundColor Green
        Write-Host "Entries added: $added" -ForegroundColor Green
        Write-Host "Entries skipped (already exist): $skipped" -ForegroundColor Yellow
        Write-Host ""
        Write-Host "IMPORTANT: Run the remaining Part 2, 3, and 4 scripts to complete all 98 SiteAdmin entries!" -ForegroundColor Magenta
    }
    catch {
        Write-Error "Failed to save ResX file: $($_.Exception.Message)"
        exit 1
    }
} else {
    Write-Host ""
    Write-Host "No new entries were added (all keys already exist)" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Script completed successfully!" -ForegroundColor Green