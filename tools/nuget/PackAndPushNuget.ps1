# Check if API_KEY is provided as an argument
param (
    [Parameter(Mandatory = $true)]
    [string]$API_KEY
)

# Define relative paths
$RootDir = (Get-Location).Path | Split-Path -Parent | Split-Path -Parent
$SrcPath = Join-Path $RootDir "src\TuyaSharp\TuyaSharp.csproj"
$OutputPath = Join-Path $RootDir "bin\Release"
$NuGetSource = "https://api.nuget.org/v3/index.json"

# Ensure dotnet CLI is available
Write-Host "Checking dotnet CLI availability..."
try
{
    dotnet --version | Out-Null
}
catch
{
    Write-Error "dotnet CLI is not found. Make sure it is installed and added to the PATH."
    exit 1
}

# Build and pack the project
Write-Host "Building and packing the project..."
$packResult = dotnet pack $SrcPath --configuration Release --output $OutputPath
if ($LASTEXITCODE -ne 0)
{
    Write-Error "Failed to pack the project."
    exit 1
}

# Search for the .nupkg file
$nupkgFiles = Get-ChildItem -Path $OutputPath -Filter "*.nupkg" -Recurse
if ($nupkgFiles.Count -eq 0)
{
    Write-Error "NuGet package not found."
    exit 1
}
$NuGetPackage = $nupkgFiles[0].FullName

# Push the package to NuGet.org
Write-Host "Publishing package: $NuGetPackage..."
$pushResult = dotnet nuget push $NuGetPackage --api-key $API_KEY --source $NuGetSource --skip-duplicate
if ($LASTEXITCODE -ne 0)
{
    Write-Error "Failed to publish the NuGet package."
    exit 1
}

Write-Host "Successfully published NuGet package: $NuGetPackage."
exit 0
