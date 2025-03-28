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

# Define the new version
$newVersion = "8.1.0"
$newWildcardVersion = "8.1.*"

# Get all .csproj files in the directory and subdirectories
$csprojFiles = Get-ChildItem -Path $directory -Recurse -Filter *.csproj

foreach ($file in $csprojFiles) {
    # Read the content of the .csproj file
    $content = Get-Content -Path $file.FullName

    # Update the version of cloudscribe package references, except for cloudscribe.HtmlAgilityPack and cloudscribe.DbHelpers
    $updatedContent = $content -replace '(?<=<PackageReference Include="cloudscribe\.(?!HtmlAgilityPack|DbHelpers)[^"]+" Version=")8\.0\.\*', $newWildcardVersion
    $updatedContent = $updatedContent -replace '(?<=<PackageReference Include="cloudscribe\.(?!HtmlAgilityPack|DbHelpers)[^"]+" Version=")8\.0\.\d+', $newVersion

    # Update the <Version> element if it matches the 8.0.* pattern
    $updatedContent = $updatedContent -replace '<Version>8\.0\.\d+</Version>', "<Version>$newVersion</Version>"

    # Write the updated content back to the .csproj file
    Set-Content -Path $file.FullName -Value $updatedContent

    Write-Host "Updated $file.FullName"
}

Write-Host "All cloudscribe package references (except cloudscribe.HtmlAgilityPack and cloudscribe.DbHelpers) and <Version> elements have been updated to version $newVersion or $newWildcardVersion as appropriate."

