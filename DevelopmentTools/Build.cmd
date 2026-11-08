@ECHO OFF
SETLOCAL
SET "versionUtilitiesBuildExitCode=0"

PUSHD "%~dp0.."
IF ERRORLEVEL 1 EXIT /B 1

IF /I "%~1"=="publish" GOTO publish
IF /I "%~1"=="release" GOTO release

dotnet build --configuration Debug -p:PlatformTarget=AnyCPU -p:Prefer32Bit=false
SET "versionUtilitiesBuildExitCode=%ERRORLEVEL%"
GOTO end

:publish
WHERE gh >NUL 2>&1
SET "versionUtilitiesBuildExitCode=%ERRORLEVEL%"
IF NOT "%versionUtilitiesBuildExitCode%"=="0" SET "message=GitHub CLI is required. Install gh and run gh auth login."
IF NOT "%versionUtilitiesBuildExitCode%"=="0" GOTO error

CD "%~dp0..\VersionUtilities"

dotnet build --configuration Release -p:PlatformTarget=AnyCPU -p:Prefer32Bit=false
SET "versionUtilitiesBuildExitCode=%ERRORLEVEL%"
IF NOT "%versionUtilitiesBuildExitCode%"=="0" GOTO end

SET "versionUtilitiesProjectVersion="
FOR /F "delims=" %%V IN (
    'dotnet msbuild -nologo -getProperty:Version'
) DO SET "versionUtilitiesProjectVersion=%%V"

IF NOT DEFINED versionUtilitiesProjectVersion SET "message=Unable to read the project version."
IF NOT DEFINED versionUtilitiesProjectVersion GOTO error

ECHO Requesting publication of version %versionUtilitiesProjectVersion% from main.
::gh workflow run dotnet.yml --ref main -F publish=true -f version="%versionUtilitiesProjectVersion%" --repo jamesjohnmcguire/VersionUtilities
ECHO ***** Would Have Puplished *****

SET "versionUtilitiesBuildExitCode=%ERRORLEVEL%"
IF NOT "%versionUtilitiesBuildExitCode%"=="0" SET "message=Problem Running: gh workflow run dotnet.yml"
IF NOT "%versionUtilitiesBuildExitCode%"=="0" GOTO error

ECHO Workflow requested. GitHub will build, test, and publish the package.
ECHO Use gh run watch from the Utilities repository to follow its progress.
GOTO end

REM Usage: build.cmd release
:release
SET "versionUtilitiesProjectVersion="
FOR /F "delims=" %%V IN (
    'dotnet msbuild VersionUpdate\VersionUpdate.csproj -nologo -getProperty:Version'
) DO SET "versionUtilitiesProjectVersion=%%V"

IF NOT DEFINED versionUtilitiesProjectVersion SET "message=Unable to read the executable project version."
IF NOT DEFINED versionUtilitiesProjectVersion GOTO error

WHERE gh >NUL 2>&1
IF ERRORLEVEL 1 SET "message=GitHub CLI is required. Install gh and run gh auth login."
IF ERRORLEVEL 1 GOTO error
WHERE zip.exe >NUL 2>&1
IF ERRORLEVEL 1 SET "message=zip.exe is required on PATH."
IF ERRORLEVEL 1 GOTO error

dotnet publish --configuration Release -p:PublishReadyToRun=true;PublishSingleFile=true;IncludeNativeLibrariesForSelfExtract=true --runtime win-x64 --self-contained true --output Release\win-x64 VersionUpdate
IF ERRORLEVEL 1 GOTO release-fail

dotnet publish --configuration Release -p:PublishReadyToRun=false;PublishSingleFile=true;IncludeNativeLibrariesForSelfExtract=true --runtime linux-x64 --self-contained true --output Release\linux-x64 VersionUpdate
IF ERRORLEVEL 1 GOTO release-fail

dotnet publish --configuration Release -p:PublishReadyToRun=false;PublishSingleFile=true;IncludeNativeLibrariesForSelfExtract=true --runtime osx-x64 --self-contained true --output Release\osx-x64 VersionUpdate
IF ERRORLEVEL 1 GOTO release-fail

PUSHD "Release\win-x64"
IF ERRORLEVEL 1 GOTO release-fail
zip.exe -r -FS "..\VersionUpdate-win-x64.zip" .
SET "versionUtilitiesZipExitCode=%ERRORLEVEL%"
POPD
IF NOT "%versionUtilitiesZipExitCode%"=="0" GOTO release-fail

PUSHD "Release\linux-x64"
IF ERRORLEVEL 1 GOTO release-fail
zip.exe -r -FS "..\VersionUpdate-linux-x64.zip" .
SET "versionUtilitiesZipExitCode=%ERRORLEVEL%"
POPD
IF NOT "%versionUtilitiesZipExitCode%"=="0" GOTO release-fail

PUSHD "Release\osx-x64"
IF ERRORLEVEL 1 GOTO release-fail
zip.exe -r -FS "..\VersionUpdate-osx-x64.zip" .
SET "versionUtilitiesZipExitCode=%ERRORLEVEL%"
POPD
IF NOT "%versionUtilitiesZipExitCode%"=="0" GOTO release-fail

::gh release create "v%versionUtilitiesProjectVersion%" --notes "%versionUtilitiesProjectVersion%" --repo jamesjohnmcguire/VersionUtilities "Release\VersionUpdate-win-x64.zip" "Release\VersionUpdate-linux-x64.zip" "Release\VersionUpdate-osx-x64.zip"
SET "versionUtilitiesBuildExitCode=%ERRORLEVEL%"
GOTO end

:release-fail
SET "message=Release failed. No further release steps were run."
GOTO error

:error
ECHO %message%
SET "versionUtilitiesBuildExitCode=1"

:end
POPD
EXIT /B %versionUtilitiesBuildExitCode%
