# Check if the version argument is provided
param (
    [Parameter(Mandatory = $true)]
    [string]$NewVersion
)

# Get the script directory
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RootDir = Join-Path $ScriptDir "..\.."

# Define the relative path to the .csproj file
$CsProjPath = Join-Path $RootDir "src\TuyaSharp\TuyaSharp.csproj"

# Check if the .csproj file exists
if (!(Test-Path -Path $CsProjPath))
{
    Write-Error "The file $CsProjPath does not exist."
    exit 1
}

# Read the .csproj file content
$CsProjContent = Get-Content -Path $CsProjPath

# Check if the <Version> tag exists
if ($CsProjContent -match "<Version>.*<\/Version>")
{
    # Replace the existing version
    $CsProjContent = $CsProjContent -replace "<Version>.*<\/Version>", "<Version>$NewVersion</Version>"
    Write-Host "Updated existing <Version> tag to $NewVersion."
}
else
{
    # Add a <Version> tag inside the first <PropertyGroup>
    $CsProjContent = $CsProjContent -replace "(<PropertyGroup>)", "`$1`n    <Version>$NewVersion</Version>"
    Write-Host "Added <Version> tag with value $NewVersion."
}

# Save the updated content back to the .csproj file
$CsProjContent | Set-Content -Path $CsProjPath

Write-Host "Successfully updated version to $NewVersion in $CsProjPath."
exit 0
