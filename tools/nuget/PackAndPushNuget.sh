#!/bin/bash

# Check if API_KEY is provided
if [ -z "$1" ]; then
    echo "[Error] API key not provided. Usage: ./pack_nuget.sh <API_KEY>"
    exit 1
fi

API_KEY=$1

# Define relative paths
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(dirname "$(dirname "$SCRIPT_DIR")")"
SRC_PATH="$ROOT_DIR/src/TuyaSharp/TuyaSharp.csproj"
OUTPUT_PATH="$ROOT_DIR/bin/Release"
NUGET_SOURCE="https://api.nuget.org/v3/index.json"

# Ensure dotnet CLI is available
if ! command -v dotnet &> /dev/null; then
    echo "[Error] dotnet CLI is not found. Make sure it is installed and added to the PATH."
    exit 1
fi

# Build and pack the project
echo "Building and packing the project..."
dotnet pack "$SRC_PATH" --configuration Release --output "$OUTPUT_PATH"
if [ $? -ne 0 ]; then
    echo "[Error] Failed to pack the project."
    exit 1
fi

# Search for the .nupkg file
NUGET_PACKAGE=$(find "$OUTPUT_PATH" -name "*.nupkg" | head -n 1)
if [ -z "$NUGET_PACKAGE" ]; then
    echo "[Error] NuGet package not found."
    exit 1
fi

# Push the package to NuGet.org
echo "Publishing package: $NUGET_PACKAGE..."
dotnet nuget push "$NUGET_PACKAGE" --api-key "$API_KEY" --source "$NUGET_SOURCE" --skip-duplicate
if [ $? -ne 0 ]; then
    echo "[Error] Failed to publish the NuGet package."
    exit 1
fi

echo "Successfully published NuGet package: $NUGET_PACKAGE."
exit 0
