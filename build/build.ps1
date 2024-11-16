param ($version = "15.0.0",$versionSuffix = "")
# Params
# version = major.minor.patch
# versionSuffix = eg -beta1, -rc1 (should include leading -). Leave empty string if not a pre-release.

. ".\functions.ps1" #Includes funtions used in the script

$versionFull = $version + $versionSuffix

Write-Host "Building version  :" $versionFull

# Set version
Set-CsProjVersion ../src/Our.Umbraco.TheDashboard/Our.Umbraco.TheDashboard.csproj $version $versionSuffix
exitIfNotSuccess

# Update cachebuster in umbraco-package.json
Update-UmbracoPackageJsonFile -JsonFilePath "../src/Our.Umbraco.TheDashboard/wwwroot/App_Plugins/Our.Umbraco.TheDashboard/umbraco-package.json" -NewVersion $versionFull
exitIfNotSuccess

# Pack
dotnet pack ../src/Our.Umbraco.TheDashboard/Our.Umbraco.TheDashboard.csproj --output Artifacts --configuration Release
exitIfNotSuccess
