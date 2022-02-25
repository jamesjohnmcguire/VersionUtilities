CD %~dp0
CD ..

CALL dotnet publish --configuration Release -p:PublishReadyToRun=false;PublishSingleFile=true --runtime linux-x64 --self-contained true --output Release\linux-x64 VersionUpdate
CALL dotnet publish --configuration Release -p:PublishReadyToRun=false;PublishSingleFile=true --runtime osx-x64 --self-contained true --output Release\osx-x64 VersionUpdate
CALL dotnet publish --configuration Release -p:PublishReadyToRun=true;PublishSingleFile=true --runtime win-x64 --self-contained true --output Release\win-x64 VersionUpdate

IF "%1"=="release" GOTO release
GOTO end

:release
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

:end
