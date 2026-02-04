#Requires -Version 5.0

<#
.SYNOPSIS
    Adds new 2FA-related ResX entries to CloudscribeCore ResX files that don't already have them.
    
.DESCRIPTION
    This script adds three new ResX entries for 2FA localization to any CloudscribeCore ResX files
    that don't already contain them. The entries use placeholders for HTML injection to maintain
    separation between content and markup.
    
    The three entries added are:
    - authenticator_download_instructions (8 placeholders for app store links)
    - authenticator_scan_qr_instructions (1 placeholder for key display)
    - disable_2fa_warning_message (2 placeholders for reset link)
    
.PARAMETER ResxFilePath
    Path to a specific ResX file to update. If not provided, the script will search for
    CloudscribeCore*.resx files in the current directory and subdirectories.
    
.PARAMETER SearchPath
    Directory path to search for CloudscribeCore*.resx files. Defaults to current directory.
    
.PARAMETER WhatIf
    Shows what would be done without actually making changes.
    
.EXAMPLE
    .\new2FAentries.ps1
    Searches current directory for CloudscribeCore*.resx files and adds missing entries.
    
.EXAMPLE
    .\new2FAentries.ps1 -ResxFilePath "GlobalResources\CloudscribeCore.resx"
    Updates a specific ResX file.
    
.EXAMPLE
    .\new2FAentries.ps1 -SearchPath "C:\MyProject" -WhatIf
    Shows what files would be updated in the specified directory.
#>

param(
    [Parameter(Mandatory=$false)]
    [string]$ResxFilePath,
    
    [Parameter(Mandatory=$false)]
    [string]$SearchPath = ".",
    
    [Parameter(Mandatory=$false)]
    [switch]$WhatIf
)

# Define the new ResX entries to add
$newEntries = @(
    @{
        Name = "authenticator_download_instructions"
        Value = "Download a two-factor authenticator app like Microsoft Authenticator for {0}Android{1} and {2}iOS{3} or Google Authenticator for {4}Android{5} and {6}iOS{7}."
        Comment = "verified - 2025-01-27 - Uses placeholders for HTML links"
    },
    @{
        Name = "authenticator_scan_qr_instructions"
        Value = "Scan the QR Code or enter this key {0} into your two factor authenticator app. Spaces and casing do not matter."
        Comment = "verified - 2025-01-27 - Uses placeholder for key display"
    },
    @{
        Name = "disable_2fa_warning_message"
        Value = "Disabling 2FA does not change the keys used in authenticator apps. If you wish to change the key used in an authenticator app you should {0}reset your authenticator keys.{1}"
        Comment = "verified - 2025-01-27 - Uses placeholders for HTML link"
    }
)

function Add-ResXEntries {
    param(
        [string]$FilePath,
        [array]$Entries,
        [bool]$WhatIfPreference
    )
    
    if (-not (Test-Path $FilePath)) {
        Write-Error "ResX file not found at: $FilePath"
        return $false
    }
    
    Write-Host "Processing: $FilePath" -ForegroundColor Cyan
    
    try {
        # Load the ResX file as XML
        [xml]$resxXml = Get-Content $FilePath -Encoding UTF8
        
        # Check if this looks like a valid ResX file
        if ($resxXml.root -eq $null) {
            Write-Warning "File does not appear to be a valid ResX file (no root element): $FilePath"
            return $false
        }
        
        # Check which entries are missing
        $missingEntries = @()
        $existingEntries = @()
        
        foreach ($entry in $Entries) {
            $existingEntry = $resxXml.root.data | Where-Object { $_.name -eq $entry.Name }
            if ($existingEntry) {
                $existingEntries += $entry.Name
            } else {
                $missingEntries += $entry
            }
        }
        
        if ($existingEntries.Count -gt 0) {
            Write-Host "  Already exists: $($existingEntries -join ', ')" -ForegroundColor Yellow
        }
        
        if ($missingEntries.Count -eq 0) {
            Write-Host "  All entries already present - no changes needed" -ForegroundColor Green
            return $true
        }
        
        Write-Host "  Adding $($missingEntries.Count) missing entries:" -ForegroundColor Green
        foreach ($entry in $missingEntries) {
            Write-Host "    - $($entry.Name)" -ForegroundColor Green
        }
        
        if ($WhatIfPreference) {
            Write-Host "  [WhatIf] Would add entries to: $FilePath" -ForegroundColor Magenta
            return $true
        }
        
        # Create backup
        $backupPath = "$FilePath.backup.$(Get-Date -Format 'yyyyMMdd-HHmmss')"
        Copy-Item $FilePath $backupPath -Force
        Write-Host "  Backup created: $backupPath" -ForegroundColor DarkGray
        
        # Add missing entries
        foreach ($entry in $missingEntries) {
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
            
            # Add to root (before closing tag)
            $resxXml.root.AppendChild($dataElement)
        }
        
        # Save the updated ResX file
        $resxXml.Save($FilePath)
        Write-Host "  Successfully updated: $FilePath" -ForegroundColor Green
        
        return $true
        
    } catch {
        Write-Error "Failed to process $FilePath : $($_.Exception.Message)"
        return $false
    }
}

# Main execution
Write-Host "New 2FA ResX Entries Script" -ForegroundColor Blue
Write-Host "==============================" -ForegroundColor Blue
Write-Host ""

$filesToProcess = @()

if ($ResxFilePath) {
    # Process specific file
    if (Test-Path $ResxFilePath) {
        $filesToProcess += (Resolve-Path $ResxFilePath).Path
    } else {
        Write-Error "Specified ResX file not found: $ResxFilePath"
        exit 1
    }
} else {
    # Search for CloudscribeCore*.resx files
    Write-Host "Searching for CloudscribeCore*.resx files in: $SearchPath" -ForegroundColor Gray
    
    $foundFiles = Get-ChildItem -Path $SearchPath -Recurse -Filter "CloudscribeCore*.resx" -File | Select-Object -ExpandProperty FullName
    
    if ($foundFiles.Count -eq 0) {
        Write-Warning "No CloudscribeCore*.resx files found in: $SearchPath"
        exit 0
    }
    
    $filesToProcess = $foundFiles
    Write-Host "Found $($filesToProcess.Count) ResX file(s)" -ForegroundColor Gray
}

Write-Host ""

# Process each file
$successCount = 0
$totalCount = $filesToProcess.Count

foreach ($file in $filesToProcess) {
    $result = Add-ResXEntries -FilePath $file -Entries $newEntries -WhatIfPreference $WhatIf
    if ($result) {
        $successCount++
    }
    Write-Host ""
}

# Summary
Write-Host "===============================================" -ForegroundColor Blue
Write-Host "Summary:" -ForegroundColor Blue
Write-Host "  Files processed: $successCount / $totalCount" -ForegroundColor $(if ($successCount -eq $totalCount) { 'Green' } else { 'Yellow' })

if ($WhatIf) {
    Write-Host "  [WhatIf mode] No actual changes were made" -ForegroundColor Magenta
}

if ($successCount -eq $totalCount) {
    Write-Host "  ✅ All files processed successfully!" -ForegroundColor Green
} else {
    Write-Host "  ⚠️  Some files had issues - please review above output" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "New entries added:" -ForegroundColor Gray
foreach ($entry in $newEntries) {
    Write-Host "  - $($entry.Name)" -ForegroundColor Gray
}