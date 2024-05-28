
function Update-UmbracoPackageJsonFile {
    param (
        [string]$JsonFilePath,
        [string]$NewVersion
    )

    Write-Host "Setting package json version" $NewVersion

    try {
        # Read the JSON content from the file
        $jsonContent = Get-Content -Path $JsonFilePath | ConvertFrom-Json

        # Update the "version" value
        $jsonContent.version = $NewVersion

        # Update the URL in the "js" property of the first element in the "extensions" array
        if ($jsonContent.extensions.Count -gt 0) {

            $currentValue = $jsonContent.extensions[0].js

            Write-Host "Cur Value " $currentValue

            # Define the regex pattern to match the URL with the version
            $pattern = "(.js\?v).*"

            $versionUrlSafe = $NewVersion -replace '\.' -replace '-'

            Write-Host "UrlSafe" $versionUrlSafe

            # Replace the matched version number with the new version
            $updatedContent = $currentValue -replace $pattern,('.js?v'+ $versionUrlSafe)

            Write-Host "New Value " $updatedContent

            $jsonContent.extensions[0].js = $updatedContent
        }

        # Convert the updated JSON content back to a string
        $updatedJsonContent = $jsonContent | ConvertTo-Json -Depth 100

        # Write the updated JSON content back to the file

        Set-Content -Path $JsonFilePath -Value $updatedJsonContent

        Write-Output "JSON file '$JsonFilePath' updated successfully."
    } catch {
        Write-Error "An error occurred: $_"
    }
}

Function Set-CsProjVersion($filePath, $versionPrefix, $versionSuffix)
{
    $assemblyVersion = $versionPrefix
    $packageVersion = $versionPrefix + $versionSuffix

    Write-Host "Setting version(s) for: $filePath"
    Write-Host "AssemblyVersion: $assemblyVersion"
    Write-Host "PackageVersion: $packageVersion"

    $xml=New-Object XML
    $xml.Load($filePath)
    $versionNode = $xml.Project.PropertyGroup.Version
    if ($versionNode -eq $null) {
        # If you have a new project and have not changed the version number the Version tag may not exist
        $versionNode = $xml.CreateElement("Version")
        $xml.Project.PropertyGroup.AppendChild($versionNode)
        Write-Host "Version-node XML tag added to the csproj"
    }

    $packageVersionNode = $xml.Project.PropertyGroup.PackageVersion
    if ($packageVersionNode -eq $null) {
        # If you have a new project and have not changed the version number the Version tag may not exist
        $packageVersionNode = $xml.CreateElement("PackageVersion")
        $xml.Project.PropertyGroup.AppendChild($packageVersionNode)
        Write-Host "PackageVersion-node XML tag added to the csproj"
    }

    $informationalVersionNode = $xml.Project.PropertyGroup.InformationalVersion
    if ($informationalVersionNode -eq $null) {
        # If you have a new project and have not changed the version number the Version tag may not exist
        $informationalVersionNode = $xml.CreateElement("InformationalVersion")
        $xml.Project.PropertyGroup.AppendChild($informationalVersionNode)
        Write-Host "InformationalVersion-node XML tag added to the csproj"
    }

    # Settings Properties
    $xml.Project.PropertyGroup.Version = $assemblyVersion
    $xml.Project.PropertyGroup.PackageVersion = $packageVersion
    $xml.Project.PropertyGroup.InformationalVersion = $packageVersion

    $xml.Save($filePath)

    Write-Host "Version updated in csproj"
}

Function exitIfNotSuccess()
{
    if($LASTEXITCODE -eq 1) {
        Write-Host "Last action failed, aborting." -ForegroundColor Red
        exit;
    }
}



