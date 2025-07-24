###################
## PS script to implement a semantic versioning change from (say) 8.0.x to 8.1 
## across all interdependent cs packages

## Wherever we have <Version>8.0.n</Version> replace it to <Version>8.1.0</Version>     where n >= 0 

## Wherever we have <PackageReference Include="cloudscribe.Anything" Version="8.0.*" /> replace it to  <PackageReference Include="cloudscribe.Anything" Version="8.1.*" />

## Wherever we have <PackageReference Include="cloudscribe.Anything" Version="8.0.n" /> replace it to  <PackageReference Include="cloudscribe.Anything" Version="8.1.0" />  where n >= 0 

## Exclude cloudscribe.HtmlAgilityPack and DbHelpers because those ones are ancient and frozen
###################


# Define the directory containing the .csproj files
$directory = "src"

# Define the old & new versions
$oldVersion = '8\.2'   # slash needed !
$newVersion = "8.3.0"
$newWildcardVersion = "8.3.*"
	

# Get all .csproj files in the directory and subdirectories
$csprojFiles = Get-ChildItem -Path $directory -Recurse -Filter *.csproj

foreach ($file in $csprojFiles) {
    # Read the content of the .csproj file
    $content = Get-Content -Path $file.FullName

    # Update the version of cloudscribe package references, except for cloudscribe.HtmlAgilityPack and cloudscribe.DbHelpers
	
	$wildCardPattern = '(?<=<PackageReference Include="cloudscribe\.(?!HtmlAgilityPack|DbHelpers)[^"]+" Version=")' + $oldVersion + '\.\*'
	$updatedContent = $content -replace $wildCardPattern, $newWildcardVersion

	$digitPattern = '(?<=<PackageReference Include="cloudscribe\.(?!HtmlAgilityPack|DbHelpers)[^"]+" Version=")' + $oldVersion + '\.\d+'
	$updatedContent = $updatedContent -replace $digitPattern, $newVersion

    # Update the <Version> element if it matches the pattern
	$versionPattern = '<Version>' + $oldVersion + '\.\d+</Version>'
	$replacement = "<Version>$newVersion</Version>"
	$updatedContent = $updatedContent -replace $versionPattern, $replacement


    # Write the updated content back to the .csproj file
    Set-Content -Path $file.FullName -Value $updatedContent

    Write-Host "Updated $file.FullName"
}

Write-Host "All cloudscribe package references (except cloudscribe.HtmlAgilityPack and cloudscribe.DbHelpers) and <Version> elements have been updated to version $newVersion or $newWildcardVersion as appropriate."

