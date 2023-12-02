REM %1 - Type of build
REM %2 - Version (such as 1.0.0.5)
REM %3 - API key

CD %~dp0
CD ..

IF "%1"=="publish" GOTO publish

dotnet publish --configuration Release -p:PublishReadyToRun=true;PublishSingleFile=true --runtime win-x64 --self-contained true --output Release\win-x64 VersionUpdate

IF "%1"=="release" GOTO release
GOTO finish

:release
dotnet publish --configuration Release -p:PublishReadyToRun=false;PublishSingleFile=true --runtime linux-x64 --self-contained true --output Release\linux-x64 VersionUpdate
dotnet publish --configuration Release -p:PublishReadyToRun=false;PublishSingleFile=true --runtime osx-x64 --self-contained true --output Release\osx-x64 VersionUpdate

CD Release\linux-x64
7z u VersionUpdate-linux-x64.zip .
MOVE VersionUpdate-linux-x64.zip ..

CD ..\osx-x64
7z u VersionUpdate-osx-x64.zip .
MOVE VersionUpdate-osx-x64.zip ..

CD ..\win-x64
7z u VersionUpdate-win-x64.zip .
MOVE VersionUpdate-win-x64.zip ..

CD ..
REM Unfortunately, the following command does not work from the windows command
REM console.  Use a bash terminal.
REM gh release create v%2 --notes %3 *.zip

:publish

if "%~2"=="" GOTO error1
if "%~3"=="" GOTO error2

CD VersionUtilities

msbuild -property:Configuration=Release -restore -target:rebuild;pack VersionUtilities.csproj

CD bin\Release
nuget push DigitalZenWorks.Common.VersionUtilities.%2.nupkg %3 -Source https://api.nuget.org/v3/index.json

CD ..\..\..

GOTO finsh

:error1
ECHO No version tag specified
GOTO end

:error2
ECHO No API key specified

:finish
