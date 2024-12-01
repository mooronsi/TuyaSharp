@echo off
REM Get the script's directory
set SCRIPT_DIR=%~dp0
set ROOT_DIR=%SCRIPT_DIR%..\..
set CS_PROJ_PATH=%ROOT_DIR%\src\TuyaSharp\TuyaSharp.csproj

REM Check if the version argument is provided
if "%~1"=="" (
    echo [Error] Version argument not provided. Usage: update_version.bat <new-version>
    exit /b 1
)

set NEW_VERSION=%~1

REM Check if the .csproj file exists
if not exist "%CS_PROJ_PATH%" (
    echo [Error] The file "%CS_PROJ_PATH%" does not exist.
    exit /b 1
)

REM Read the .csproj file and update the version
set TEMP_FILE=%TEMP%\csproj_temp.xml
set UPDATED=0

(for /f "delims=" %%i in ('type "%CS_PROJ_PATH%"') do (
    echo %%i | find "<Version>" >nul
    if not errorlevel 1 (
        echo     <Version>%NEW_VERSION%</Version> >> "%TEMP_FILE%"
        set UPDATED=1
    ) else (
        echo %%i >> "%TEMP_FILE%"
    )
))

REM If the <Version> tag was not found, add it inside the first <PropertyGroup>
if %UPDATED%==0 (
    echo Adding new <Version> tag...
    for /f "delims=" %%i in ('type "%CS_PROJ_PATH%"') do (
        echo %%i | find "<PropertyGroup>" >nul
        if not errorlevel 1 (
            echo %%i >> "%TEMP_FILE%"
            echo     <Version>%NEW_VERSION%</Version> >> "%TEMP_FILE%"
        ) else (
            echo %%i >> "%TEMP_FILE%"
        )
    )
)

REM Overwrite the original file with the updated content
move /y "%TEMP_FILE%" "%CS_PROJ_PATH%" >nul

echo Successfully updated version to %NEW_VERSION% in %CS_PROJ_PATH%.
exit /b 0
