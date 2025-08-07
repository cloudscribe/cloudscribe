#Requires -Version 5.0

<#
.SYNOPSIS
    Inserts newly-added ResX entries from UserAdmin folder string localization into CloudscribeCore.resx
.DESCRIPTION
    This script inserts all newly-added string resource entries into a user's local copy of CloudscribeCore.resx.
    Created during UserAdmin folder string localization process on 2025-08-07.
.PARAMETER ResxFilePath
    Path to the CloudscribeCore.resx file to update
.EXAMPLE
    .\InsertResxEntries_UserAdmin.ps1 -ResxFilePath "C:\MyProject\GlobalResources\CloudscribeCore.resx"
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

# New keys to add (from KeysAdded_UserAdmin.txt)
$newEntries = @(
    @{
        Name = "Password must be different from current password"
        Value = "Password must be different from current password"
        Comment = "verified - 2025-08-07"
        File = "ChangeUserPassword.cshtml"
    },
    @{
        Name = "2 Factor Enabled"
        Value = "2 Factor Enabled"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Add Roles"
        Value = "Add Roles"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Cancel"
        Value = "Cancel"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Confirm Delete"
        Value = "Confirm Delete"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Confirm Disable Two Factor Authentication"
        Value = "Confirm Disable Two Factor Authentication"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Disable Two Factor Auth."
        Value = "Disable Two Factor Auth."
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Email an account activation link to the user"
        Value = "Email an account activation link to the user"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Export Users"
        Value = "Export Users"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Failed Logins"
        Value = "Failed Logins"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Last Login"
        Value = "Last Login"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Last Password Change"
        Value = "Last Password Change"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Password Reset Email"
        Value = "Password Reset Email"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Passwords must have at least one digit ('0'-'9')."
        Value = "Passwords must have at least one digit ('0'-'9')."
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Passwords must have at least one lowercase character ('a'-'z')."
        Value = "Passwords must have at least one lowercase character ('a'-'z')."
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Passwords must have at least one non alphanumeric character"
        Value = "Passwords must have at least one non alphanumeric character"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Passwords must have at least one uppercase character ('A'-'Z')."
        Value = "Passwords must have at least one uppercase character ('A'-'Z')."
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Search Users"
        Value = "Search Users"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Select"
        Value = "Select"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Send"
        Value = "Send"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Social Logins"
        Value = "Social Logins"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Two Factor Authentication"
        Value = "Two Factor Authentication"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "User must change password"
        Value = "User must change password"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "Username is already in use"
        Value = "Username is already in use"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    },
    @{
        Name = "View User Activity"
        Value = "View User Activity"
        Comment = "verified - 2025-08-07"
        File = "UserAdmin"
    }
)

Write-Host "UserAdmin Folder String Localization - ResX Entry Insertion Script" -ForegroundColor Green
Write-Host "====================================================================" -ForegroundColor Green
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
        Write-Host "Successfully updated CloudscribeCore.resx file\!" -ForegroundColor Green
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
Write-Host "Script completed successfully\!" -ForegroundColor Green
