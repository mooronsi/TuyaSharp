#!/bin/bash

# Get the script's directory
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/../../" && pwd)"
CS_PROJ_PATH="$ROOT_DIR/src/TuyaSharp/TuyaSharp.csproj"

# Check if the version argument is provided
if [ -z "$1" ]; then
    echo "[Error] Version argument not provided. Usage: ./update_version.sh <new-version>"
    exit 1
fi

NEW_VERSION="$1"

# Check if the .csproj file exists
if [ ! -f "$CS_PROJ_PATH" ]; then
    echo "[Error] The file $CS_PROJ_PATH does not exist."
    exit 1
fi

# Temporary file for the updated .csproj content
TEMP_FILE=$(mktemp)

# Flag to check if <Version> was updated
UPDATED=0

# Read the .csproj file and update the version
while IFS= read -r line; do
    if echo "$line" | grep -q "<Version>"; then
        echo "    <Version>$NEW_VERSION</Version>" >> "$TEMP_FILE"
        UPDATED=1
    else
        echo "$line" >> "$TEMP_FILE"
    fi
done < "$CS_PROJ_PATH"

# If the <Version> tag was not found, add it inside the first <PropertyGroup>
if [ "$UPDATED" -eq 0 ]; then
    echo "Adding new <Version> tag..."
    TEMP_FILE_ADDITIONAL=$(mktemp)
    while IFS= read -r line; do
        if echo "$line" | grep -q "<PropertyGroup>"; then
            echo "$line" >> "$TEMP_FILE_ADDITIONAL"
            echo "    <Version>$NEW_VERSION</Version>" >> "$TEMP_FILE_ADDITIONAL"
        else
            echo "$line" >> "$TEMP_FILE_ADDITIONAL"
        fi
    done < "$TEMP_FILE"
    mv "$TEMP_FILE_ADDITIONAL" "$TEMP_FILE"
fi

# Overwrite the original .csproj file with the updated content
mv "$TEMP_FILE" "$CS_PROJ_PATH"

echo "Successfully updated version to $NEW_VERSION in $CS_PROJ_PATH."
exit 0
