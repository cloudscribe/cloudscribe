#Requires -Version 5.0

<#
.SYNOPSIS
    Inserts newly-added ResX entries from Account folder string localization into CloudscribeCore.resx
.DESCRIPTION
    This script inserts all newly-added string resource entries into a user's local copy of CloudscribeCore.resx.
    Created during Account folder string localization process on 2025-08-07.
.PARAMETER ResxFilePath
    Path to the CloudscribeCore.resx file to update
.EXAMPLE
    .\InsertResxEntries_Account.ps1 -ResxFilePath "C:\MyProject\GlobalResources\CloudscribeCore.resx"
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

# New keys to add (from KeysAdded_Account.txt)
$newEntries = @(
    @{
        Name = "Or submit the code provided in the email."
        Value = "Or submit the code provided in the email."
        Comment = "verified - 2025-08-07"
        File = "EmailConfirmationRequired.cshtml"
    },
    @{
        Name = "You must enter the code sent to your email address"
        Value = "You must enter the code sent to your email address"
        Comment = "verified - 2025-08-07"
        File = "EmailConfirmationRequired.cshtml"
    },
    @{
        Name = "Associate your {0} account"
        Value = "Associate your {0} account"
        Comment = "verified - 2025-08-07"
        File = "ExternalLoginConfirmation.cshtml"
    },
    @{
        Name = "You've successfully authenticated with <strong>{0}</strong>."
        Value = "You've successfully authenticated with <strong>{0}</strong>."
        Comment = "verified - 2025-08-07"
        File = "ExternalLoginConfirmation.cshtml"
    },
    @{
        Name = "Email or Username"
        Value = "Email or Username"
        Comment = "verified - 2025-08-07"
        File = "Login.cshtml"
    }
)

Write-Host "Account Folder String Localization - ResX Entry Insertion Script" -ForegroundColor Green
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
