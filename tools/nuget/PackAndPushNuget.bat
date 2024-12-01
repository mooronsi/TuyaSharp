@echo off
REM Check if API_KEY is provided as an argument
if "%~1"=="" (
    echo [Error] API key not provided. Usage: pack_nuget.bat <API_KEY>
    exit /b 1
)
set API_KEY=%~1

REM Define relative paths
set ROOT_DIR=%~dp0..\..
set SRC_PATH=%ROOT_DIR%\src\TuyaSharp\TuyaSharp.csproj
set OUTPUT_PATH=%ROOT_DIR%\bin\Release
set NUGET_SOURCE=https://api.nuget.org/v3/index.json

REM Navigate to the root directory
pushd %ROOT_DIR%
if %ERRORLEVEL% NEQ 0 (
    echo [Error] Failed to navigate to the root directory: %ROOT_DIR%.
    exit /b 1
)

REM Check if dotnet CLI is available
echo Checking dotnet CLI availability...
dotnet --version >nul 2>&1
IF %ERRORLEVEL% NEQ 0 (
    echo [Error] dotnet CLI is not found. Make sure it is installed and added to the PATH.
    popd
    exit /b 1
)

REM Build and pack the project
echo Building and packing the project...
dotnet pack "%SRC_PATH%" --configuration Release --output "%OUTPUT_PATH%"
IF %ERRORLEVEL% NEQ 0 (
    echo [Error] Failed to pack the project.
    popd
    exit /b 1
)

REM Search for the .nupkg file
for /r "%OUTPUT_PATH%" %%f in (*.nupkg) do (
    set NUGET_PACKAGE=%%f
)

REM Check if the NuGet package exists
if not exist "%NUGET_PACKAGE%" (
    echo [Error] NuGet package not found.
    popd
    exit /b 1
)

REM Push the package to NuGet.org
echo Publishing package: %NUGET_PACKAGE%...
dotnet nuget push "%NUGET_PACKAGE%" --api-key %API_KEY% --source %NUGET_SOURCE% --skip-duplicate
IF %ERRORLEVEL% NEQ 0 (
    echo [Error] Failed to publish the NuGet package.
    popd
    exit /b 1
)

echo Successfully published NuGet package: %NUGET_PACKAGE%.
popd
exit /b 0
