#Requires -Version 5.0

<#
.SYNOPSIS
    Inserts newly-added ResX entries from Shared folder string localization into CloudscribeCore.resx
.DESCRIPTION
    This script inserts all newly-added string resource entries into a user's local copy of CloudscribeCore.resx.
    Created during Shared folder string localization process on 2025-01-27.
    Most Shared folder files were already properly localized or use different localizers.
.PARAMETER ResxFilePath
    Path to the CloudscribeCore.resx file to update
.EXAMPLE
    .\InsertResxEntries_Shared.ps1 -ResxFilePath "C:\MyProject\GlobalResources\CloudscribeCore.resx"
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

# New keys to add (from KeysAdded_Shared.txt)
$newEntries = @(
    @{
        Name = "Learn More"
        Value = "Learn More"
        Comment = "verified - 2025-01-27 - Shared"
        File = "_CookieConsentPartial.cshtml"
    },
    @{
        Name = "Log in"
        Value = "Log in"
        Comment = "verified - 2025-01-27 - Shared"
        File = "_LoginPartial.cshtml/_LoginPartialWithDropdowns.cshtml"
    },
    @{
        Name = "Revoke Cookie Consent"
        Value = "Revoke Cookie Consent"
        Comment = "verified - 2025-01-27 - Shared"
        File = "_CookieConsentRevokePartial.cshtml"
    },
    @{
        Name = "Session Expiry"
        Value = "Session Expiry"
        Comment = "verified - 2025-01-27 - Shared"
        File = "_AutoLogoutWarningPartial.cshtml"
    },
    @{
        Name = "Toggle Side Menu"
        Value = "Toggle Side Menu"
        Comment = "verified - 2025-01-27 - Shared"
        File = "AdminSideNavToggle.cshtml"
    },
    @{
        Name = "user avatar"
        Value = "user avatar"
        Comment = "verified - 2025-01-27 - Shared"
        File = "_LoginPartialWithDropdowns.cshtml"
    }
)

Write-Host "Shared Folder String Localization - ResX Entry Insertion Script" -ForegroundColor Green
Write-Host "=================================================================" -ForegroundColor Green
Write-Host "Target ResX file: $ResxFilePath" -ForegroundColor Cyan
Write-Host "Total entries to add: $($newEntries.Count)" -ForegroundColor Cyan
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
    
    Write-Host "Added: $($entry.Name) (from $($entry.File))" -ForegroundColor Green
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